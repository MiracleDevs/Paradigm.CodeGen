﻿@include "../shared.cs"
@{	
	var entityName = Raw(Model.Definition.Name);
	var name = $"I{entityName}Table";	
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
using MiracleDevs.ORM.Data.Attributes;

namespace @Model.Configuration["Namespace"]
{@Raw(GetAttributes(Model.Definition.Attributes, "\t"))
	public interface @name
    {
        #region Properties
		@foreach(var property in properties)
		{
			@if (property.Attributes.All(x => x.Name != "NavigationAttribute"))
			{
<text>	@Raw(GetAttributes(property.Attributes, "\t\t"))
		@Raw(GetModelName(Model, property.Type, true)) @property.Name { get; }
</text>
			}
		}

		#endregion
    }
}