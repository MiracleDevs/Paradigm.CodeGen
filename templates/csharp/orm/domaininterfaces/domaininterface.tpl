@include "../shared.cs"
@{
	var name = Model.Definition.Name;
	var interfaceName = $"I{name}";
	var properties = (Model.Definition as Paradigm.CodeGen.Input.Models.Definitions.StructDefinition).Properties;
}//////////////////////////////////////////////////////////////////////////////////
//  @(name + ".cs")
//
//  Generated with the Paradigm.CodeGen tool.
//  Do not modify this file in any way.
//
//  Copyright (c) 2016 miracledevs. All rights reserved.
//////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using Paradigm.Services.Interfaces;
using Paradigm.Services.Interfaces.Attributes;
using @Raw(Model.Configuration["ValidationResourceTypeNamespace"]);

namespace @Model.Configuration["Namespace"]
{
    /// <summary>
    /// Provides an interface for domain and view objects modeling a @Raw(GetReadableString(Model.Definition.Name)).
    /// </summary>
	public partial interface @Raw(interfaceName): IDomainInterface
    {
        #region Properties
		@foreach(var property in properties)
		{
<text>
        /// @Raw("<summary>")
        /// Gets or sets the @Raw(GetReadableString(property.Name)).
        /// @Raw("</summary>")@Raw(GetValidationAttributes(property, Model.Configuration["ValidationResourceType"], "\t\t"))
		@Raw(GetModelName(Model, property.Type, true)) @property.Name { get; }
</text>
		}

		#endregion
    }
}