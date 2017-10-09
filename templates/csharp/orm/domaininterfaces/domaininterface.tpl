@include "../shared.cs"
@{
	var name = Raw(Model.Definition.Name);
	var interfaceName = $"I{name}";
	var properties = (Model.Definition as MiracleDevs.CodeGenerator.Input.Models.Definitions.StructDefinition).Properties;
}//////////////////////////////////////////////////////////////////////////////////
//  @(name + ".cs")
//
//  Generated with the MiracleDevs.CodeGenerator tool.
//  Do not modify this file in any way.
//
//  Copyright (c) 2016 miracledevs. All rights reserved.
//////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using MiracleDevs.Framework.Interfaces;
using MiracleDevs.Framework.Interfaces.Attributes;

namespace @Model.Configuration["Namespace"]
{
	public interface @interfaceName: IDomainInterface
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