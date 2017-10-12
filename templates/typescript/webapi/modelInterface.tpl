@include "shared.cs"
@{
	var name = Raw(GetModelName(Model, Model.Definition, isInterface: true));
	var properties = GetProperties(Model.Definition);
	var contracts = GetModelRelatedContracts(Model, Model.Definition, properties);
}//////////////////////////////////////////////////////////////////////////////////
//  @(name + ".ts")
//
//  Generated with the Paradigm.CodeGen tool.
//  Do not modify this file in any way.
//
//  Copyright (c) 2016 miracledevs. All rights reserved.
//////////////////////////////////////////////////////////////////////////////////

@foreach(var contract in contracts)
{
<text>///<reference path="@Raw(GetModelName(Model, contract, isInterface: true) + ".ts")" />
</text>
}

module @Model.Configuration["Namespace"]
{
	export interface @name
	{
	@foreach(var property in properties)
	{	
<text>
		@ToCamelCase(property.Name): @Raw(GetModelName(Model, property.Type, isInterface: true));
</text>
	}
	}
}