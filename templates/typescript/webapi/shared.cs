using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using Paradigm.CodeGen.Input.Models.Definitions;
using Paradigm.CodeGen.Output.Razor;
using Paradigm.CodeGen.Output.Models.Configuration;
using Paradigm.CodeGen.Output.NamingRules;

public static class Functions
{
    public static ObjectDefinitionBase GetDefinition(ObjectDefinitionBase objectDefinition, GenerationItemModel model)
    {
        if (objectDefinition.FullName.StartsWith("System.Threading.Tasks.Task"))
        {
            var structDefinition = objectDefinition as StructDefinition;
            return structDefinition.GenericArguments.FirstOrDefault() ?? model.Definitions.FirstOrDefault(x => x.FullName == "System.Void");
        }

        return objectDefinition;
    }

    public static string GetModelName(GenerationItemModel model, ObjectDefinitionBase objectDefinition, bool isInterface)
    {
        objectDefinition = GetDefinition(objectDefinition, model);

        if (objectDefinition is EnumDefinition)
            isInterface = false;

        var translator = model.GetTranslator(objectDefinition);

        if (translator == null)
        {
            if (objectDefinition.IsArray && objectDefinition.InnerObject != null)
                return string.Format("{0}[]", GetModelName(model, objectDefinition.InnerObject, isInterface: isInterface));

            return string.Format("{0}{1}", isInterface && !objectDefinition.IsInterface ? "I" : "", objectDefinition.Name.Replace("View", ""));
        }

        return translator.Translation;
    }

    public static string GetFileName(GenerationItemModel model, ObjectDefinitionBase objectDefinition, bool isInterface)
    {
        var namingRule = new SeparateWordsNamingRule();
        var name = GetModelName(model, objectDefinition, isInterface: false);

        if (objectDefinition.IsInterface)
            name = name.Substring(1, name.Length - 1);

        var newName = namingRule.Execute(name, new NamingRuleConfiguration() { Parameters = new[] { "-" } }).ToLowerInvariant();


        if (isInterface && objectDefinition is StructDefinition)
            return newName + ".interface";

        if (isInterface && objectDefinition is EnumDefinition)
            return newName + ".enum";

        return newName;
    }

    public static string GetServiceFileName(GenerationItemModel model, ObjectDefinitionBase objectDefinition, bool isInterface)
    {
        var namingRule = new SeparateWordsNamingRule();
        var name = GetServiceName(objectDefinition, isInterface: false);
        name = name.Replace("Service", "");

        if (objectDefinition.IsInterface)
            name = name.Substring(1, name.Length - 1);

        var newName = namingRule.Execute(name, new NamingRuleConfiguration() { Parameters = new[] { "-" } }).ToLowerInvariant() + ".service";

        if (isInterface && objectDefinition is StructDefinition)
            return newName + ".interface";

        return newName;
    }

    public static string GetServiceName(ObjectDefinitionBase objectDefinition, bool isInterface)
    {
        return string.Format("{0}{1}", isInterface ? "I" : "", objectDefinition.Name.Replace("Controller", "Service"));
    }

    public static string ToCamelCase(string value)
    {
        if (value == null)
            return null;

        if (value.Length == 0)
            return value;

        return char.ToLower(value[0]) + value.Substring(1, value.Length - 1);
    }

    public static List<PropertyDefinition> GetProperties(ObjectDefinitionBase definition)
    {
        return (definition as StructDefinition).Properties.Where(x => x.Attributes.Any(a => a.Name == "DataMemberAttribute")).ToList();
    }

    public static IEnumerable<ObjectDefinitionBase> GetServiceRelatedContracts(GenerationItemModel model, ObjectDefinitionBase definition)
    {
        var contracts = new Dictionary<string, ObjectDefinitionBase>();
        var classDefinition = definition as StructDefinition;

        if (classDefinition == null)
            return contracts.Values;

        foreach (var method in classDefinition.Methods)
        {
            var objectDefinition = GetDefinition(method.ReturnType, model);

            if (objectDefinition.IsArray)
                objectDefinition = objectDefinition.InnerObject;

            var translator = model.GetTranslator(objectDefinition);

            if (!contracts.ContainsKey(objectDefinition.Name) &&
                 translator == null)
            {
                contracts.Add(objectDefinition.Name, objectDefinition);
            }

            foreach (var parameter in method.Parameters)
            {
                objectDefinition = GetDefinition(parameter.Type, model);

                if (objectDefinition.IsArray)
                    objectDefinition = objectDefinition.InnerObject;

                translator = model.GetTranslator(objectDefinition);

                if (!contracts.ContainsKey(objectDefinition.Name) &&
                     translator == null)
                {
                    contracts.Add(objectDefinition.Name, objectDefinition);
                }
            }
        }

        return contracts.Values.ToList();
    }

    public static IEnumerable<ObjectDefinitionBase> GetModelRelatedContracts(GenerationItemModel model, ObjectDefinitionBase definition, List<PropertyDefinition> properties)
    {
        var contracts = new Dictionary<string, ObjectDefinitionBase>();

        foreach (var property in properties)
        {
            var objectDefinition = GetDefinition(property.Type, model);

            if (objectDefinition.IsArray)
                objectDefinition = objectDefinition.InnerObject;

            var translator = model.GetTranslator(objectDefinition);

            if (objectDefinition.Name != definition.Name &&
                !contracts.ContainsKey(objectDefinition.Name) &&
                translator == null)
            {
                contracts.Add(objectDefinition.Name, objectDefinition);
            }
        }

        return contracts.Values.ToList();
    }

    public static string MethodFirm(GenerationItemModel model, MethodDefinition method, bool includeSemicolon)
    {
        var parameters = string.Empty;

        foreach (var parameter in method.Parameters)
        {
            parameters += parameter.Name + ": " + GetModelName(model, parameter.Type, isInterface: true) + ", ";
        }

        if (parameters.EndsWith(", "))
        {
            parameters = parameters.Substring(0, parameters.Length - 2);
        }

        return string.Format("{0}({1}): IHttpPromise<{2}>{3}", ToCamelCase(method.Name), parameters, GetModelName(model, method.ReturnType, isInterface: true), includeSemicolon ? ";" : "");
    }

    public static string GetMethodVerb(MethodDefinition method)
    {
        if (method.Attributes.Any(x => x.Name == "HttpGetAttribute"))
            return "get";

        if (method.Attributes.Any(x => x.Name == "HttpPostAttribute"))
            return "post";

        if (method.Attributes.Any(x => x.Name == "HttpPutAttribute"))
            return "put";

        if (method.Attributes.Any(x => x.Name == "HttpPatchAttribute"))
            return "patch";

        if (method.Attributes.Any(x => x.Name == "HttpDeleteAttribute"))
            return "delete";

        if (method.Attributes.Any(x => x.Name == "HttpOptionsAttribute"))
            return "options";

        if (method.Name.ToLower() == "get")
            return "get";

        if (method.Name.ToLower() == "post")
            return "post";

        if (method.Name.ToLower() == "put")
            return "put";

        if (method.Name.ToLower() == "patch")
            return "patch";

        if (method.Name.ToLower() == "delete")
            return "delete";

        if (method.Name.ToLower() == "options")
            return "options";

        return "get";
    }

    public static bool VerbAllowsBodyData(string verb)
    {
        switch (verb)
        {
            case "get":
            case "delete":
            case "options":
                return false;
            default:
                return true;
        }
    }

    public static string GetControllerTemplate(GenerationItemModel model)
    {
        var route = model.Definition.Attributes.FirstOrDefault(x => x.Name == "RouteAttribute")?.Parameters[0]?.Value ?? string.Empty;
        return route.Replace("[controller]", model.Definition.Name.Replace("Controller", string.Empty));
    }

    public static string GetMethodTemplate(MethodDefinition method)
    {
        var attribute = method.Attributes.FirstOrDefault(x => x.Name == "HttpGetAttribute") ??
                        method.Attributes.FirstOrDefault(x => x.Name == "HttpPostAttribute") ??
                        method.Attributes.FirstOrDefault(x => x.Name == "HttpPutAttribute") ??
                        method.Attributes.FirstOrDefault(x => x.Name == "HttpDeleteAttribute");

        return attribute?.Parameters?.FirstOrDefault(x => x.Name.ToLower() == "template")?.Value ?? string.Empty;
    }

    public static string MethodBody(GenerationItemModel model, MethodDefinition method, string serviceName)
    {
        var parameters = string.Empty;
        var callParameters = string.Empty;
        var data = string.Empty;
        var verb = GetMethodVerb(method);
        var controllerRoute = GetControllerTemplate(model);
        var methodRoute = GetMethodTemplate(method);
        var route = string.IsNullOrWhiteSpace(methodRoute) ? controllerRoute : $"{controllerRoute}/{methodRoute}";

        foreach (var parameter in method.Parameters)
        {
            if (parameter.Attributes.Any(x => x.Name == "FromBodyAttribute"))
                data = string.Format("{0}", parameter.Name);
            else
                callParameters += string.Format("{0}: {0}, ", parameter.Name);
        }

        if (!string.IsNullOrWhiteSpace(data) && !VerbAllowsBodyData(verb))
            throw new Exception($"The action '{method.Name}' in controller '{serviceName}' does not allow body data due to its verb '{verb}' does not allows message body data.");

        if (callParameters.Any())
            callParameters = string.Format("{{ {0} }}", callParameters.Substring(0, callParameters.Length - 2));

        return $"return this.{verb}<{GetModelName(model, method.ReturnType, isInterface: true)}>('{route}', {(callParameters.Any() ? callParameters : "null")}{(data.Any() ? ", " + data : "")});";
    }
}