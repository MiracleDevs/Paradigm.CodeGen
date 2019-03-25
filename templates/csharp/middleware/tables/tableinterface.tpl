@include "../shared.cs"
@{
	var entityName = Model.Definition.Name;
	var name = $"I{entityName}Table";
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
using Paradigm.ORM.Data.Attributes;

namespace @Model.Configuration["Namespace"]
{@Raw(GetAttributes(Model.Definition.Attributes, "\t"))
	public interface @Raw(name)
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