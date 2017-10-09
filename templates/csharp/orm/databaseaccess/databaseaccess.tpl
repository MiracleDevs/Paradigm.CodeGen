@include "../shared.cs"
@{
	var name = Raw($"{Model.Definition.Name}DatabaseAccess");
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

using System;
using System.Linq;
using MiracleDevs.ORM.Data.DatabaseAccess.Generic;
using @Model.Configuration["DomainNamespace"];

namespace @Model.Configuration["Namespace"]
{
	public partial class @name : DatabaseAccess<@entityName>, @("I" + name)
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
		public @interfaceTypeName @typeName { get; private set; }
</text>
			}

<text>		
		#endregion

</text>
		}
		#region Constructor

		public @(name)(IServiceProvider provider) : base(provider)
		{
		}

		#endregion		
		@if (navigationProperties.Any())
		{
<text>		
		#region Protected Methods

		protected override void AfterInitialize()
		{
			base.AfterInitialize();		
</text>
			foreach(var property in navigationProperties)
			{
				var className = Model.Definition.Name;
				var typeName = Raw((property.Type.IsArray ? property.Type.InnerObject.Name : property.Type.Name) + "DatabaseAccess");
				var interfaceTypeName = $"I{typeName}";
				var propertyName = Raw($"{className}.{property.Name}");				
<text>
			this.@typeName = this.NavigationDatabaseAccesses.FirstOrDefault(x => x.NavigationPropertyDescriptor.PropertyName == nameof(@propertyName))?.DatabaseAccess as @interfaceTypeName;

            if (this.@typeName == null)
                throw new Exception("@name couldn't retrieve a reference to @typeName.");
</text>
			}

<text>		
		}

		#endregion
</text>
		}
	}
}