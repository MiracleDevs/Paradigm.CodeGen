using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Paradigm.CodeGen.Input.Models.Definitions;
using Paradigm.CodeGen.Output.Razor;

public static class Functions
{
    public static string GetReadableString(string value)
    {
        return string.IsNullOrWhiteSpace(value) ? "" : Regex.Replace(value, "(\\B[A-Z])", " $1").ToLower();
    }

    public static string GetModelName(GenerationItemModel model, ObjectDefinitionBase objectDefinition, bool isInterface = false)
    {
        if (objectDefinition is EnumDefinition)
            isInterface = false;

        var translator = model.GetTranslator(objectDefinition);

        if (translator == null)
        {
            return objectDefinition.IsArray && objectDefinition.InnerObject != null
                    ? $"{(isInterface ? "IReadOnlyCollection" : "List")}<{GetModelName(model, objectDefinition.InnerObject, isInterface: isInterface)}>"
                    : $"{(isInterface ? "I" : "")}{(isInterface ? objectDefinition.Name.Replace("View", string.Empty) : objectDefinition.Name)}";
        }

        return translator.Translation;
    }

    public static bool IsNullable(ObjectDefinitionBase objectDefinition)
    {
        return objectDefinition.Name.StartsWith("Nullable<") || objectDefinition.Name == "String" || objectDefinition.IsArray;
    }

    public static bool IsGenericNullable(ObjectDefinitionBase objectDefinition)
    {
        return objectDefinition.Name.StartsWith("Nullable<");
    }

    public static bool RequiresQuotationMarks(ObjectDefinitionBase objectDefinition)
    {
        var name = IsNullable(objectDefinition) && objectDefinition.InnerObject != null ? objectDefinition.InnerObject.Name : objectDefinition.Name;

        return name == "String" ||
                name == "Char" ||
                name == "DateTime" ||
                name == "TimeSpan" ||
                name == "Object";
    }

    public static string GetMapMethod(ObjectDefinitionBase objectDefinition)
    {
        var type = IsNullable(objectDefinition) && objectDefinition.InnerObject != null ? objectDefinition.InnerObject : objectDefinition;

        if (type.Name == typeof(Byte[]).Name || type.Name == typeof(SByte[]).Name)
            return "GetBytes";

        if (type.Name == typeof(float).Name || type.Name == typeof(System.Single).Name)
            return "GetFloat";

        if (type.Name == typeof(DateTimeOffset).Name)
            return "GetFieldValue<DateTimeOffset>";

        return $"Get{type.Name}";
    }

    public static List<PropertyDefinition> GetColumnProperties(StructDefinition structDefinition, bool includeAutoIncrement = true)
    {
        var properties = structDefinition.Properties.Where(x => x.Attributes.Any(a => a.Name == "ColumnAttribute")).ToList();

        if (!includeAutoIncrement)
            properties.RemoveAll(x => x.Attributes.Any(a => a.Name == "IdentityAttribute"));

        return properties;
    }

    public static List<PropertyDefinition> GetPrimaryKeys(StructDefinition structDefinition)
    {
        return structDefinition.Properties.Where(x => x.Attributes.Any(a => a.Name == "PrimaryKeyAttribute") && x.Attributes.Any(a => a.Name == "ColumnAttribute")).ToList();
    }

    public static string GetTableName(GenerationItemModel model)
    {
        var structDefinition = model.Definition as StructDefinition;

        if (structDefinition == null)
            return string.Empty;

        var tableAttribute = structDefinition.Attributes.FirstOrDefault(x => x.Name == "TableAttribute");
        return tableAttribute == null ? structDefinition.Name : tableAttribute.Parameters[0].Value;
    }

    public static string GetColumnNames(GenerationItemModel model, bool includeAutoIncrement = true)
    {
        var structDefinition = model.Definition as StructDefinition;
        var newLineLength = System.Environment.NewLine.Length;

        if (structDefinition == null)
            return string.Empty;

        var builder = new System.Text.StringBuilder();
        var properties = GetColumnProperties(structDefinition, includeAutoIncrement);

        foreach (var property in properties)
        {
            var attribute = property.Attributes.First(x => x.Name == "ColumnAttribute");
            builder.AppendFormat("`{0}`, ", attribute.Parameters[0].Value);
        }

        return builder.Remove(builder.Length - newLineLength, newLineLength).ToString();
    }

    public static string MapProperties(GenerationItemModel model, string objectName, string readerName, bool isAsync = false, bool includeAutoIncrement = false)
    {
        var structDefinition = model.Definition as StructDefinition;

        if (structDefinition == null)
            return string.Empty;

        var builder = new System.Text.StringBuilder();
        var properties = GetColumnProperties(structDefinition, includeAutoIncrement);
        var i = 0;
        builder.AppendLine();

        foreach (var property in properties)
        {
            var asyncPostfix = (isAsync ? "Async" : "");
            var awaitPrefix = (isAsync ? "await" : "");
            var typeName = GetModelName(model, property.Type);

            if (IsNullable(property.Type))
            {
                builder.AppendFormat($"\t\t\t{objectName}.{property.Name} = {awaitPrefix} {readerName}.IsDBNull{asyncPostfix}({i}) ? default({typeName}) : {readerName}.{GetMapMethod(property.Type)}({i});\n");
            }
            else
            {
                builder.AppendFormat($"\t\t\t{objectName}.{property.Name} = {readerName}.{GetMapMethod(property.Type)}({i});\n");
            }

            i++;
        }

        return builder.ToString();
    }

    public static string GetAttributes(List<AttributeDefinition> attributes, string tabs)
    {
        var builder = new System.Text.StringBuilder();
        var newLineLength = System.Environment.NewLine.Length;

        builder.AppendLine();

        foreach (var attribute in attributes)
        {
            if (attribute.Parameters.Any())
                builder.AppendFormat("{0}[{1}({2})]", tabs, attribute.Name.Replace("Attribute", ""), string.Join(", ", attribute.Parameters.Select(x => x.IsNumeric ? x.Value : $"\"{x.Value}\"")));
            else
                builder.AppendFormat("{0}[{1}]", tabs, attribute.Name.Replace("Attribute", ""));

            builder.AppendLine();
        }

        return builder.Remove(builder.Length - newLineLength, newLineLength).ToString();
    }

    public static string GetValidationAttributes(PropertyDefinition property, string resourceClass, string tabs)
    {
        var builder = new System.Text.StringBuilder();
        var newLineLength = System.Environment.NewLine.Length;

        builder.AppendLine();

        var notNullable = property.Attributes.FirstOrDefault(x => x.Name == "NotNullableAttribute");
        var foreignKey = property.Attributes.FirstOrDefault(x => x.Name == "ForeignKeyAttribute");
        var numeric = property.Attributes.FirstOrDefault(x => x.Name == "NumericAttribute");
        var size = property.Attributes.FirstOrDefault(x => x.Name == "SizeAttribute");
        var range = property.Attributes.FirstOrDefault(x => x.Name == "RangeAttribute");

        if (notNullable != null)
        {
            builder.AppendFormat("{0}[Required(\"{1}\", typeof({2}))]", tabs, property.Name, resourceClass);
            builder.AppendLine();
        }

        if (foreignKey != null && property.Type.Name == "Int32")
        {
            builder.AppendFormat("{0}[NonZero(\"{1}\", typeof({2}))]", tabs, property.Name, resourceClass);
            builder.AppendLine();
        }

        if (numeric != null)
        {
            builder.AppendFormat("{0}[Numeric(\"{1}\", {2}, {3}, typeof({4}))]", tabs, property.Name, numeric.Parameters[0].Value, numeric.Parameters[1].Value, resourceClass);
            builder.AppendLine();
        }

        if (size != null)
        {
            builder.AppendFormat("{0}[MaxSize(\"{1}\", {2}, typeof({3}))]", tabs, property.Name, size.Parameters[0].Value, resourceClass);
            builder.AppendLine();
        }

        if (range != null && (property.Type.Name == nameof(System.DateTime) || property.Type.Name == nameof(System.DateTimeOffset)))
        {
            builder.AppendFormat("{0}[DateTime(\"{1}\", \"{2}\",  \"{3}\", typeof({4}))]", tabs, property.Name, range.Parameters[0].Value, range.Parameters[1].Value, resourceClass);
            builder.AppendLine();
        }

        var text = builder.Remove(builder.Length - newLineLength, newLineLength).ToString();
        return string.IsNullOrEmpty(text)? "//There are no validation properties" : text;
    }

    public static bool ImplementsDomainInterface(GenerationItemModel model)
    {
        var name = model.Definition.Name;
        var entityName = name.ToString().Replace("View", "");

        return model.Definitions.Any(x => x.Name == entityName && x.Attributes.Any(a => a.Name == nameof(TableAttribute)));
    }

    public static string GetInheritance(GenerationItemModel model)
    {
        var name = model.Definition.Name;
        var entityName = name.ToString().Replace("View", "");
        var tableName = $"I{name}Table";

        return ImplementsDomainInterface(model)
            ? $"DomainBase<I{entityName}, {name}>, I{entityName}, {tableName}"
            : $"DomainBase, {tableName}";
    }

    public static string GetPrivateName(PropertyDefinition property)
    {
        return $"_{char.ToLower(property.Name[0])}{property.Name.Substring(1)}";
    }

    public static string GetDomainTracker(PropertyDefinition property)
    {
        return $"DomainTracker<{(property.Type.InnerObject != null ? property.Type.InnerObject.Name : property.Type.Name)}>";
    }

    public static string GetDomainTrackerName(PropertyDefinition property)
    {
        return $"{(property.Type.InnerObject != null ? property.Type.InnerObject.Name : property.Type.Name)}DomainTracker";
    }

    public static string GetIgnoreProperties(string ignoreProperties, List<PropertyDefinition> properties)
    {
        var configProperties = ignoreProperties.Split(',').Where(x => properties.Any(p => p.Name == x)).Select(x => $"nameof({x.Trim()})");
        var navigationProperties = properties.Where(x => x.Attributes.Any(a => a.Name == "NavigationAttribute")).Select(x => $"nameof({x.Name})");
        var allProperties = configProperties.Union(navigationProperties).ToList();

        if (allProperties.Any())
        {
            return $"this.ValidateEntity(new[] {{ {string.Join(", ", allProperties)} }}.Union(this.GetPropertiesToIgnoreInValidation()));";
        }

        return $"this.ValidateEntity(this.GetPropertiesToIgnoreInValidation());";
    }

    public static string GetIsNewCondition(ObjectDefinitionBase definition)
    {
        var structDefinition = definition as StructDefinition;

        if (structDefinition == null)
            return string.Empty;

        var primaryKeys = GetPrimaryKeys(structDefinition);

        if (!primaryKeys.Any())
            return "true";

        var builder = new System.Text.StringBuilder();

        foreach (var property in primaryKeys)
        {
            builder.AppendFormat("this.{0} == default({1}) || ", property.Name, property.Type.Name);
        }

        return builder.Remove(builder.Length - 4, 4).ToString();
    }

    public static string GetIsEqualCondition(ObjectDefinitionBase definition, string source, string destination)
    {
        var structDefinition = definition as StructDefinition;

        if (structDefinition == null)
            return string.Empty;

        var primaryKeys = GetPrimaryKeys(structDefinition);

        if (!primaryKeys.Any())
            return "true";

        var builder = new System.Text.StringBuilder();

        foreach (var property in primaryKeys)
        {
            builder.AppendFormat("{0}.{1} == {2}.{1} && ", source, property.Name, destination);
        }

        return builder.Remove(builder.Length - 4, 4).ToString();
    }

    public static string GetPrivateFieldList(GenerationItemModel model, List<PropertyDefinition> properties, string tabs)
    {
        var builder = new System.Text.StringBuilder();
        var newLine = System.Environment.NewLine;

        builder.AppendFormat("{0}#region Private Fields{1}{1}", tabs, newLine);

        foreach (var property in properties)
        {
            builder.AppendLine($"{tabs}/// <summary>");
            builder.AppendLine($"{tabs}/// The {GetReadableString(property.Name)}.");
            builder.AppendLine($"{tabs}/// </summary>");
            builder.AppendFormat("{0}private {1}{2} {3};{4}{4}", tabs, property.Type.IsArray ? "readonly " : "", GetModelName(model, property.Type, false), GetPrivateName(property), newLine);
        }

        builder.AppendFormat("{0}#endregion{1}{1}", tabs, newLine);
        return builder.ToString();
    }

    public static string GetPublicPropertyList(GenerationItemModel model, List<PropertyDefinition> properties, string tabs)
    {
        var builder = new System.Text.StringBuilder();
        var newLine = System.Environment.NewLine;

        builder.AppendFormat("{0}#region Public Properties{1}{1}", tabs, newLine);

        foreach (var property in properties)
        {
            var isSimple = property.Attributes.All(x => x.Name != "NavigationAttribute");
            var modelNameInterface = GetModelName(model, property.Type, true);
            var modelName = GetModelName(model, property.Type, false);

            if (isSimple)
            {
                builder.AppendLine($"{tabs}/// <summary>");
                builder.AppendLine($"{tabs}/// Gets or sets the {GetReadableString(property.Name)}.");
                builder.AppendLine($"{tabs}/// </summary>");
                builder.AppendLine($"{tabs}[DataMember]");
                builder.AppendLine($"{tabs}public {modelNameInterface} {property.Name} {{ get; set; }}");
            }
            else
            {
                var privateName = GetPrivateName(property);

                builder.AppendLine($"{tabs}/// <summary>");
                builder.AppendLine($"{tabs}/// Gets or sets the {GetReadableString(property.Name)}.");
                builder.AppendLine($"{tabs}/// </summary>");
                builder.AppendLine($"{tabs}[DataMember]");

                foreach (var navigation in property.Attributes.Where(x => x.Name == "NavigationAttribute"))
                {
                    var referencedType = navigation.Parameters[0].Value;
                    var sourceProperty = navigation.Parameters[1].Value;
                    var referencedProperty = navigation.Parameters[2].Value;

                    builder.AppendLine($"{tabs}[Navigation(typeof({referencedType}), \"{sourceProperty}\", \"{referencedProperty}\")]");
                }

                builder.AppendLine((!property.Type.IsArray)
                    ? $"{tabs}public {modelNameInterface} {property.Name} {{ get => this.{privateName}; private set => this.{privateName} = value as {modelName}; }}"
                    : $"{tabs}public {modelNameInterface} {property.Name} => this.{privateName};");

                builder.AppendLine();
                builder.AppendLine($"{tabs}/// <summary>");
                builder.AppendLine($"{tabs}/// The {GetReadableString(GetDomainTrackerName(property))}.");
                builder.AppendLine($"{tabs}/// </summary>");
                builder.AppendLine($"{tabs}[IgnoreDataMember]");
                builder.AppendLine($"{tabs}public {GetDomainTracker(property)} {GetDomainTrackerName(property)} {{ get; }}");
            }

            builder.AppendLine();
        }

        builder.AppendFormat("{0}#endregion{1}{1}", tabs, newLine);
        return builder.ToString();
    }

    public static string GetConstructorList(GenerationItemModel model, List<PropertyDefinition> properties, string tabs)
    {
        var builder = new System.Text.StringBuilder();
        var newLineLength = System.Environment.NewLine.Length;

        builder.AppendLine();

        foreach (var property in properties)
        {
            builder.AppendFormat("{0}this.{1} = new {2}();", tabs, GetPrivateName(property), GetModelName(model, property.Type, false));
            builder.AppendLine();
        }

        foreach (var property in properties)
        {
            builder.AppendFormat("{0}this.{1} = new {2}();", tabs, GetDomainTrackerName(property), GetDomainTracker(property));
            builder.AppendLine();
        }

        var text = builder.Remove(builder.Length - newLineLength, newLineLength).ToString();
        return string.IsNullOrEmpty(text)? "// No properties to initialize." : text;
    }

    public static string GetIgnorePropertyList(GenerationItemModel model, List<PropertyDefinition> properties, string tabs)
    {
        if (!properties.Any())
            return "// No properties to ignore.";

        var builder = new System.Text.StringBuilder();

        builder.AppendLine();

        foreach (var property in properties)
        {
            builder.AppendFormat("{0}configuration.Ignore(x => x.{1});", tabs, property.Name);
            builder.AppendLine();
        }

        var text = builder.ToString();
        return string.IsNullOrEmpty(text) ? "// No properties to ignore." : text;
    }

    public static string GetMapPropertyList(GenerationItemModel model, List<PropertyDefinition> properties, string tabs)
    {
        if (!properties.Any())
            return  "// No properties to map.";

        var builder = new System.Text.StringBuilder();

        builder.AppendLine();

        foreach (var property in properties)
        {
            var isArray = property.Type.IsArray;
            var type = isArray ? property.Type.InnerObject : property.Type;
            var typeName = isArray ? property.Name : GetModelName(model, type, false).Replace("View", string.Empty);
            var parameter = isArray ? string.Empty : $"this.{GetPrivateName(property)}, ";

            builder.AppendFormat("{0}this.Map{1}({2}model.{3});", tabs, typeName, parameter, property.Name);
            builder.AppendLine();
        }

        var text = builder.ToString();
        return string.IsNullOrEmpty(text) ? "// No properties to map." : text;
    }

    public static string GetPropertyValidations(GenerationItemModel model, List<PropertyDefinition> properties, string tabs)
    {
        if (!properties.Any())
            return "// No properties validations.";

        var builder = new System.Text.StringBuilder();

        builder.AppendLine();

        foreach (var property in properties)
        {
            var isArray = property.Type.IsArray;
            if (isArray)
            {
                builder.AppendLine($"{tabs}foreach(var child in this.{GetPrivateName(property)})");
                builder.AppendLine($"{tabs}    child.Validate();");
            }
            else
            {
                builder.AppendLine($"{tabs}this.{GetPrivateName(property)}.Validate();");
            }
        }

        var text = builder.ToString();
        return string.IsNullOrEmpty(text) ? "// No properties validations." : text;
    }

    public static string GetCrudMethods(GenerationItemModel model, List<PropertyDefinition> properties, string tabs)
    {
        if (!properties.Any())
            return string.Empty;

        var newLineLength = System.Environment.NewLine.Length;
        var builder = new System.Text.StringBuilder();
        builder.AppendLine();

        foreach (var property in properties)
        {
            var isArray = property.Type.IsArray;
            var type = isArray ? property.Type.InnerObject : property.Type;
            var typeName = GetModelName(model, type, false);
            var methodName = typeName.Replace("View", string.Empty);

            ////////////////////////////////////////////////////
            // Add Method
            ////////////////////////////////////////////////////
            builder.AppendLine($"{tabs}/// <summary>");
            builder.AppendLine($"{tabs}/// Adds a new {GetReadableString(typeName)}.");
            builder.AppendLine($"{tabs}/// </summary>");
            builder.AppendLine($"{tabs}public void Add{methodName}({typeName} entity)");
            builder.AppendLine($"{tabs}{{");
            builder.AppendLine(isArray
                                ? $"{tabs}	this.{GetPrivateName(property)}.Add(entity);"
                                : $"{tabs}	this.{GetPrivateName(property)} = entity;");
            builder.AppendLine($"{tabs}	this.{GetDomainTrackerName(property)}.Add(entity);");
            builder.AppendLine($"{tabs}}}");
            builder.AppendLine();

            ////////////////////////////////////////////////////
            // Remove Method
            ////////////////////////////////////////////////////
            builder.AppendLine($"{tabs}/// <summary>");
            builder.AppendLine($"{tabs}/// Removes the {GetReadableString(typeName)}.");
            builder.AppendLine($"{tabs}/// </summary>");
            builder.AppendLine($"{tabs}public void Remove{methodName}({typeName} entity)");
            builder.AppendLine($"{tabs}{{");
            builder.AppendLine(isArray
                                ? $"{tabs}	this.{GetPrivateName(property)}.Remove(entity);"
                                : $"{tabs}	this.{GetPrivateName(property)} = null;");
            builder.AppendLine($"{tabs}	this.{GetDomainTrackerName(property)}.Remove(entity);");
            builder.AppendLine($"{tabs}}}");
            builder.AppendLine();
        }

        return builder.Remove(builder.Length - newLineLength, newLineLength).ToString();
    }

    public static string GetPrivateMethods(GenerationItemModel model, List<PropertyDefinition> properties, string tabs)
    {
        if (!properties.Any())
            return string.Empty;

        var builder = new System.Text.StringBuilder();
        var newLine = System.Environment.NewLine;

        var singleMethods = GetSingleMapMethods(model, properties, tabs);
        var listMethods = GetListMapMethods(model, properties, tabs);

        builder.AppendFormat("{0}#region Private Properties{1}{1}", tabs, newLine);

        if (!string.IsNullOrWhiteSpace(singleMethods))
            builder.Append(singleMethods);

        if (!string.IsNullOrWhiteSpace(listMethods))
            builder.Append(listMethods);

        builder.AppendFormat("{0}#endregion{1}{1}", tabs, newLine);
        return builder.ToString();
    }

    public static string GetSingleMapMethods(GenerationItemModel model, List<PropertyDefinition> properties, string tabs)
    {
        if (!properties.Any())
            return string.Empty;

        var builder = new System.Text.StringBuilder();

        foreach (var property in properties)
        {
            var isArray = property.Type.IsArray;
            var type = isArray ? property.Type.InnerObject : property.Type;
            var typeName = GetModelName(model, type, false);
            var methodName = typeName.Replace("View", string.Empty);
            var typeInterfaceName = GetModelName(model, type, true);

            builder.AppendLine($"{tabs}/// <summary>");
            builder.AppendLine($"{tabs}/// Maps the {GetReadableString(typeName)} from a domain interface.");
            builder.AppendLine($"{tabs}/// </summary>");
            builder.AppendLine($"{tabs}private void Map{methodName}({typeName} entity, {typeInterfaceName} model)");
            builder.AppendLine($"{tabs}{{");
            builder.AppendLine($"{tabs}	entity.MapFrom(model);");
            builder.AppendLine($"{tabs}	this.{GetDomainTrackerName(property)}.Edit(entity);");
            builder.AppendLine($"{tabs}}}");
            builder.AppendLine();
        }

        return builder.ToString();
    }

    public static string GetListMapMethods(GenerationItemModel model, List<PropertyDefinition> properties, string tabs)
    {
        var structDefinition = model.Definition as StructDefinition;

        if (structDefinition == null)
            return string.Empty;

        var primaryKeys = GetPrimaryKeys(structDefinition);

        if (!primaryKeys.Any())
            return string.Empty;

        var builder = new System.Text.StringBuilder();

        foreach (var property in properties.Where(x => x.Type.IsArray))
        {
            var innerType = property.Type.InnerObject;
            var isArray = property.Type.IsArray;
            var type = isArray ? property.Type.InnerObject : property.Type;
            var typeName = GetModelName(model, type, false);
            var methodName = typeName.Replace("View", string.Empty);

            builder.AppendLine($"{tabs}/// <summary>");
            builder.AppendLine($"{tabs}/// Maps a list of {GetReadableString(property.Name)} from a list of domain interfaces.");
            builder.AppendLine($"{tabs}/// </summary>");
            builder.AppendLine($"{tabs}private void Map{property.Name}({GetModelName(model, property.Type, true)} childContracts)");
            builder.AppendLine($"{tabs}{{");

            //////////////////////////////////////////////////
            // Insert Update foreach
            //////////////////////////////////////////////////
            builder.AppendLine($"{tabs}	foreach(var childContract in childContracts)");
            builder.AppendLine($"{tabs}	{{");
            builder.AppendLine($"{tabs}		if (!childContract.IsNew())");
            builder.AppendLine($"{tabs}		{{");
            builder.AppendLine($"{tabs}			var childModel = this.{GetPrivateName(property)}.FirstOrDefault(x => {GetIsEqualCondition(innerType, "x", "childContract")});");
            builder.AppendLine($"{tabs}			");
            builder.AppendLine($"{tabs}			if (childModel == null)");
            builder.AppendLine($"{tabs}				throw new Exception(\"{innerType.Name} not found.\");");
            builder.AppendLine($"{tabs}			");
            builder.AppendLine($"{tabs}			this.Map{methodName}(childModel, childContract);");
            builder.AppendLine($"{tabs}		}}");
            builder.AppendLine($"{tabs}		else");
            builder.AppendLine($"{tabs}		{{");
            builder.AppendLine($"{tabs}			this.Add{methodName}({innerType.Name}.FromInterface(this.ServiceProvider, childContract));");
            builder.AppendLine($"{tabs}		}}");
            builder.AppendLine($"{tabs}	}}");
            builder.AppendLine();

            //////////////////////////////////////////////////
            // Remove Update
            //////////////////////////////////////////////////
            builder.AppendLine($"{tabs}	foreach (var childModel in this.{GetPrivateName(property)}.ToList())");
            builder.AppendLine($"{tabs}	{{");
            builder.AppendLine($"{tabs}		if (!childContracts.Any(x => {GetIsEqualCondition(innerType, "x", "childModel")}))");
            builder.AppendLine($"{tabs}		{{");
            builder.AppendLine($"{tabs}			this.Remove{methodName}(childModel);");
            builder.AppendLine($"{tabs}		}}");
            builder.AppendLine($"{tabs}	}}");

            builder.AppendLine($"{tabs}}}");
            builder.AppendLine();
        }

        return builder.ToString();
    }

    public static string GetPrimaryKeyForRepositories(GenerationItemModel model)
    {
        var structDefinition = model.Definition as StructDefinition;

        if (structDefinition == null)
            return string.Empty;

        var primaryKeys = GetPrimaryKeys(structDefinition);

        if (!primaryKeys.Any())
            return string.Empty;

        if (primaryKeys.Count == 1)
            return GetModelName(model, primaryKeys.First().Type, true);

        return $"Tuple<{string.Join(", ", primaryKeys.Select(x => GetModelName(model, x.Type, true)))}>";
    }

    public static string GetRemoveMethods(GenerationItemModel model, List<PropertyDefinition> properties, string tabs)
    {
        var builder = new System.Text.StringBuilder();
        var entityName = model.Definition.Name;

        builder.AppendLine();
        builder.AppendLine($"{tabs}#region Private Methods");
        builder.AppendLine();

        foreach (var property in properties)
        {
            var type = (property.Type.IsArray ? property.Type.InnerObject : property.Type);

            builder.AppendLine($"{tabs}/// <summary>");
            builder.AppendLine($"{tabs}/// Removes the specified enumeration of {GetReadableString(type.Name)}.");
            builder.AppendLine($"{tabs}/// </summary>");
            builder.AppendLine($"{tabs}private void Remove(IEnumerable<{GetModelName(model, type, false)}> entities)");
            builder.AppendLine($"{tabs}{{");
            builder.AppendLine($"{tabs}    this.GetRepository<I{type.Name}Repository>().Remove(entities);");
            builder.AppendLine($"{tabs}}}");
            builder.AppendLine();
            builder.AppendLine($"{tabs}/// <summary>");
            builder.AppendLine($"{tabs}/// Removes the specified enumeration of {GetReadableString(type.Name)}.");
            builder.AppendLine($"{tabs}/// </summary>");
            builder.AppendLine($"{tabs}private async FrameworkTask RemoveAsync(IEnumerable<{GetModelName(model, type, false)}> entities)");
            builder.AppendLine($"{tabs}{{");
            builder.AppendLine($"{tabs}    await this.GetRepository<I{type.Name}Repository>().RemoveAsync(entities);");
            builder.AppendLine($"{tabs}}}");
            builder.AppendLine();
        }

        builder.AppendLine($"{tabs}#endregion");
        return builder.ToString();
    }

    public static string GetSingleEditRemoval(GenerationItemModel model, List<PropertyDefinition> properties, string tabs, string prefix = "", string postfix = "")
    {
        var builder = new System.Text.StringBuilder();
        var newLineLength = System.Environment.NewLine.Length;

        builder.AppendLine();

        foreach (var property in properties)
        {
            builder.AppendLine($"{tabs}{prefix}this.Remove{postfix}(entity.{GetDomainTrackerName(property)}.Removed);");
        }

        var text = builder.Remove(builder.Length - newLineLength, newLineLength).ToString();
        return string.IsNullOrEmpty(text)? "// There are no properties to remove." : text;
    }

    public static string GetMultiEditRemoval(GenerationItemModel model, List<PropertyDefinition> properties, string tabs, string prefix = "", string postfix = "")
    {
        var builder = new System.Text.StringBuilder();
        var newLineLength = System.Environment.NewLine.Length;

        builder.AppendLine();

        foreach (var property in properties)
        {
            builder.AppendLine($"{tabs}{prefix}this.Remove{postfix}(entityList.SelectMany(x => x.{GetDomainTrackerName(property)}.Removed));");
        }

        var text = builder.Remove(builder.Length - newLineLength, newLineLength).ToString() ?? "<missing>";
        return string.IsNullOrEmpty(text)? "// There are no properties to remove." : text;

    }

    public static bool IsDataReaderProcedure(GenerationItemModel model)
    {
        return model.Definition.Attributes.FirstOrDefault(x => x.Name == "StoredProcedureTypeAttribute")?.Parameters[0]?.Value == "DataReader";
    }

    public static string GetStoredProcedureInterface(GenerationItemModel model)
    {
        var procedureType = model.Definition.Attributes.FirstOrDefault(x => x.Name == "StoredProcedureTypeAttribute");
        var parameters = $"{model.Definition.Name}Parameters";

        if (procedureType == null)
            return string.Empty;

        switch (procedureType.Parameters[0].Value)
        {
            case "Reader":
                return $"IReaderStoredProcedure<{parameters}, {GetStoredProcedureResults(model)}>";

            case "Scalar":
                return $"IScalarStoredProcedure<{parameters}, {GetStoredProcedureResults(model)}>";

            case "NonQuery":
                return $"INonQueryStoredProcedure<{parameters}>";
        }

        return string.Empty;
    }

    public static string GetStoredProcedureBaseClass(GenerationItemModel model)
    {
        var procedureType = model.Definition.Attributes.FirstOrDefault(x => x.Name == "StoredProcedureTypeAttribute");
        var name = model.Definition.Name;
        var parameters = $"{name}Parameters";

        if (procedureType == null)
            return string.Empty;

        switch (procedureType.Parameters[0].Value)
        {
            case "Reader":
                return $"ReaderStoredProcedure<{parameters}, {GetStoredProcedureResults(model)}>";

            case "Scalar":
                return $"ScalarStoredProcedure<{parameters}, {GetStoredProcedureResults(model)}>";

            case "NonQuery":
                return $"NonQueryStoredProcedure<{parameters}>";
        }

        return string.Empty;
    }

    public static string GetStoredProcedureResults(GenerationItemModel model)
    {
        return string.Join(",", model.Definition.Attributes.Where(x => x.Name == "RoutineResultAttribute").Select(x => x.Parameters[0].Value));
    }

    public static string GetStoredProcedureMapperDefinition(GenerationItemModel model)
    {
        return string.Join(",", model.Definition.Attributes.Where(x => x.Name == "RoutineResultAttribute").Select((x, i) => $"I{x.Parameters[0].Value}DatabaseReaderMapper mapper{i + 1}"));
    }

    public static string GetStoredProcedureMapperNames(GenerationItemModel model)
    {
        return string.Join(",", model.Definition.Attributes.Where(x => x.Name == "RoutineResultAttribute").Select((x, i) => $"mapper{i + 1}"));
    }
}