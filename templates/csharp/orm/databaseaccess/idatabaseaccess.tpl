@include "../shared.cs"
@{
	var name = $"I{Model.Definition.Name}DatabaseAccess";
	var entityName = Model.Definition.Name;
	var properties = (Model.Definition as Paradigm.CodeGen.Input.Models.Definitions.StructDefinition).Properties;
	var navigationProperties = properties.Where(x => x.Attributes.Any(a => a.Name == "NavigationAttribute")).ToList();

}//////////////////////////////////////////////////////////////////////////////////
//  @(name + ".cs")
//
//  Generated with the Paradigm.CodeGen tool.
//  Do not modify this file in any way.
//
//  Copyright (c) 2016 miracledevs. All rights reserved.
//////////////////////////////////////////////////////////////////////////////////

using Paradigm.ORM.Data.DatabaseAccess.Generic;
using @Model.Configuration["DomainNamespace"];

namespace @Model.Configuration["Namespace"]
{
    /// <summary>
    /// Provides an interface for @Raw(GetReadableString(Model.Definition.Name)) database access objects
    /// that allows to create, update, delete and select objects from the database.
    /// </summary>
	public partial interface @Raw(name) : IDatabaseAccess<@Raw(entityName)>
	{
		@if (navigationProperties.Any())
		{
<text>		#region Properties
</text>
			foreach(var property in navigationProperties)
			{
				var typeName = (property.Type.IsArray ? property.Type.InnerObject.Name : property.Type.Name) + "DatabaseAccess";
				var interfaceTypeName = "I" + typeName;
<text>
        /// @Raw("<summary>")
        /// Gets or sets the @Raw(GetReadableString(typeName)).
        /// @Raw("</summary>")
		@Raw(interfaceTypeName) @Raw(typeName) { get; }
</text>
			}
<text>
		#endregion
</text>
		}
	}
}