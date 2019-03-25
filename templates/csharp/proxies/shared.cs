using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using Paradigm.CodeGen.Input.Models.Definitions;
using Paradigm.CodeGen.Output.Razor;
using Paradigm.CodeGen.Output.Models.Configuration;
using Paradigm.CodeGen.Output.NamingRules;
using System.Text.RegularExpressions;

public static class Functions
{
    public static string GetReadableString(string value)
    {
        return Regex.Replace(value, "(\\B[A-Z])", " $1").ToLower();
    }

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
		        return string.Format("List<{0}>", GetModelName(model, objectDefinition.InnerObject, isInterface: isInterface));

            if (objectDefinition.IsInterface)
                return objectDefinition.Name.Substring(1, objectDefinition.Name.Length - 1).Replace("View", "");

            if (objectDefinition is EnumDefinition)
                return objectDefinition.FullName;

			return string.Format("{0}{1}", isInterface && !objectDefinition.IsInterface ?  "I" : "", objectDefinition.Name.Replace("View", ""));
		}

        return translator.Translation;
	}

	public static string GetServiceName(ObjectDefinitionBase objectDefinition, bool isInterface)
	{
		return string.Format("{0}{1}", isInterface ?  "I" : "", objectDefinition.Name.Replace("Controller", "Service"));
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

	public static string MethodFirm(GenerationItemModel model, MethodDefinition method, bool includeSemicolon)
	{
		var parameters = string.Empty;

		foreach(var parameter in method.Parameters)
		{
			parameters += $"{GetModelName(model, parameter.Type, isInterface: false)} {parameter.Name}, ";
		}

		if (parameters.EndsWith(", "))
		{
			parameters = parameters.Substring(0, parameters.Length - 2);
		}

        var result = GetModelName(model, method.ReturnType, isInterface: false);
        var isVoid = result == "void" || result == "System.Void";

        if (!isVoid)
            result = $"Task<{result}>";
        else
            result = "Task";

		return $"{(includeSemicolon? "" : "async ")}{result} {method.Name}Async({parameters}){(includeSemicolon ? ";" : "")}";
	}

	public static string GetMethodVerb(MethodDefinition method)
	{
		if (method.Attributes.Any(x => x.Name == "HttpGetAttribute"))
			return "GetAsync";

		if (method.Attributes.Any(x => x.Name == "HttpPostAttribute"))
			return "PostAsync";

		if (method.Attributes.Any(x => x.Name == "HttpPutAttribute"))
			return "PutAsync";

		if (method.Attributes.Any(x => x.Name == "HttpPatchAttribute"))
			throw new Exception("Patch verb not supported.");

		if (method.Attributes.Any(x => x.Name == "HttpDeleteAttribute"))
			return "DeleteAsync";

		if (method.Attributes.Any(x => x.Name == "HttpOptionsAttribute"))
			throw new Exception("Options verb not supported.");


		if (method.Name.ToLower() == "get")
			return "GetAsync";

		if (method.Name.ToLower() == "post")
			return "PostAsync";

		if (method.Name.ToLower() == "put")
			return "PutAsync";

		if (method.Name.ToLower() == "patch")
			throw new Exception("Patch verb not supported.");

		if (method.Name.ToLower() == "delete")
			return "DeleteAsync";

		if (method.Name.ToLower() == "options")
		throw new Exception("Options verb not supported.");

		return "GetAsync";
	}

	public static bool VerbAllowsBodyData(string verb)
	{
		switch(verb)
		{
			case "GetAsync":
			case "DeleteAsync":
			case "OptionsAsync":
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
        var dataType = string.Empty;
		var verb = GetMethodVerb(method);
	    var controllerRoute = GetControllerTemplate(model);
	    var methodRoute = GetMethodTemplate(method);
	    var route = string.IsNullOrWhiteSpace(methodRoute) ? controllerRoute : $"{controllerRoute}/{methodRoute}";
        var result = GetModelName(model, method.ReturnType, isInterface: false);
        var isVoid = result == "void" || result == "System.Void";

		foreach(var parameter in method.Parameters)
		{
			if (parameter.Attributes.Any(x => x.Name == "FromBodyAttribute"))
            {
				data = parameter.Name;
                dataType = GetModelName(model, parameter.Type, false);
            }
			else
				callParameters += $"{{ \"{parameter.Name}\", {parameter.Name} }}, ";
		}

		if (!string.IsNullOrWhiteSpace(data) && !VerbAllowsBodyData(verb))
			throw new Exception($"The action '{method.Name}' in controller '{serviceName}' does not allow body data due to its verb '{verb}' does not allows message body data.");

		if (callParameters.Any())
			callParameters = $",  new Dictionary<string, object> {{ {callParameters.Substring(0, callParameters.Length - 2)} }}";

        var generics = "";

        if (!isVoid)
            generics += result;

        if (callParameters.Any())
        {
            if (generics.Any())
            {
                generics += ",";
            }

            generics += "Dictionary<string, object>";
        }

        if (data.Any())
        {
            if (generics.Any())
            {
                generics += ",";
            }

            generics += dataType;
        }

		return $"{(isVoid ? "" : "return ")}await this.{verb}<{generics}>(\"{route}\"{(callParameters.Any() ? callParameters : "")}{(data.Any() ? ", " + data : "")});";
    }
}