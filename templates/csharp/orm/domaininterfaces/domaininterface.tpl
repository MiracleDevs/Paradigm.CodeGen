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
	public interface @Raw(interfaceName): IDomainInterface
    {
        #region Properties
		@foreach(var property in properties)
		{
<text>	@Raw(GetValidationAttributes(property, Model.Configuration["ValidationResourceType"], "\t\t"))
		@Raw(GetModelName(Model, property.Type, true)) @property.Name { get; }
</text>
		}

		#endregion
    }
}