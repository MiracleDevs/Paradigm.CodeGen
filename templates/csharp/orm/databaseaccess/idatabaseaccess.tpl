@include "../shared.cs"
@{
	var name = Raw($"I{Model.Definition.Name}DatabaseAccess");
	var entityName = @Raw(Model.Definition.Name);
	var properties = (Model.Definition as MiracleDevs.CodeGenerator.Input.Models.Definitions.StructDefinition).Properties;
	var navigationProperties = properties.Where(x => x.Attributes.Any(a => a.Name == "NavigationAttribute")).ToList();

}//////////////////////////////////////////////////////////////////////////////////
//  @(name + ".cs")
//
//  Generated with the MiracleDevs.CodeGenerator tool.
//  Do not modify this file in any way.
//
//  Copyright (c) 2016 miracledevs. All rights reserved.
//////////////////////////////////////////////////////////////////////////////////

using MiracleDevs.ORM.Data.DatabaseAccess.Generic;
using @Model.Configuration["DomainNamespace"];

namespace @Model.Configuration["Namespace"]
{
	public partial interface @name : IDatabaseAccess<@entityName>
	{	
		@if (navigationProperties.Any())
		{
<text>		#region Properties
</text>
			foreach(var property in navigationProperties)
			{
				var typeName = Raw((property.Type.IsArray ? property.Type.InnerObject.Name : property.Type.Name) + "DatabaseAccess");
				var interfaceTypeName = "I" + typeName;
<text>
		@interfaceTypeName @typeName { get; }
</text>
			}
<text>		
		#endregion
</text>
		}
	}
}