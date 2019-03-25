@include "../shared.cs"
@{
	var name = $"{Model.Definition.Name}DatabaseAccess";
	var entityName = Model.Definition.Name;
	var properties = (Model.Definition as Paradigm.CodeGen.Input.Models.Definitions.StructDefinition).Properties;
	var navigationProperties = properties.Where(x => x.Attributes.Any(a => a.Name == "NavigationAttribute")).ToList();

}//////////////////////////////////////////////////////////////////////////////////
//  @(Raw(name) + ".cs")
//
//  Generated with the Paradigm.CodeGen tool.
//  Do not modify this file in any way.
//
//  Copyright (c) 2016 miracledevs. All rights reserved.
//////////////////////////////////////////////////////////////////////////////////

using System;
using System.Linq;
using Paradigm.ORM.Data.DatabaseAccess.Generic;
using Paradigm.ORM.Data.Database;
using @Model.Configuration["DomainNamespace"];
using @Model.Configuration["ConnectorNamespace"];

namespace @Model.Configuration["Namespace"]
{
    /// <summary>
    /// Represents a database access object
    /// that allows to create, update, delete and select @Raw(GetReadableString(Model.Definition.Name)) from the database.
    /// </summary>
	public partial class @Raw(name) : DatabaseAccess<@Raw(entityName)>, @("I" + name)
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
		public @Raw(interfaceTypeName) @Raw(typeName) { get; private set; }
</text>
			}

<text>
		#endregion

</text>
		}
		#region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="@Raw(name)"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="connector">The database connector.</param>
        public @(name)(IServiceProvider serviceProvider, @Model.Configuration["Connector"] connector) : base(serviceProvider, connector)
		{
		}

		#endregion
		@if (navigationProperties.Any())
		{
<text>
		#region Protected Methods

        /// @Raw("<summary>")
        /// Executes right after the database access object has been initialized.
        /// @Raw("</summary>")
		protected override void AfterInitialize()
		{
			base.AfterInitialize();
</text>
			foreach(var property in navigationProperties)
			{
				var className = Model.Definition.Name;
				var typeName = (property.Type.IsArray ? property.Type.InnerObject.Name : property.Type.Name) + "DatabaseAccess";
				var interfaceTypeName = $"I{typeName}";
				var propertyName = $"{className}.{property.Name}";
<text>
			this.@Raw(typeName) = this.NavigationDatabaseAccesses.FirstOrDefault(x => x.NavigationPropertyDescriptor.PropertyName == nameof(@Raw(propertyName)))?.DatabaseAccess as @Raw(interfaceTypeName);

            if (this.@Raw(typeName) == null)
                throw new Exception("@Raw(name) couldn't retrieve a reference to @typeName.");
</text>
			}

<text>
		}

		#endregion
</text>
		}
	}
}