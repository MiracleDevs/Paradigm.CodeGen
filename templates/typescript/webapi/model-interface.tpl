@include "shared.cs"
@{
	var name = GetModelName(Model, Model.Definition, isInterface: true);
	var properties = GetProperties(Model.Definition);
	var contracts = GetModelRelatedContracts(Model, Model.Definition, properties);
}//////////////////////////////////////////////////////////////////////////////////
//  @Raw(name + ".ts")
//
//  Generated with the Paradigm.CodeGen tool.
//  Do not modify this file in any way.
//
//  Copyright (c) 2016 miracledevs. All rights reserved.
//////////////////////////////////////////////////////////////////////////////////

@foreach(var contract in contracts)
{
<text>import { @Raw(GetModelName(Model, contract, isInterface: true)) } from "./@Raw(GetFileName(Model, contract, isInterface: true))";
</text>
}

export interface @Raw(name)
{
	@foreach(var property in properties)
	{
<text>	@ToCamelCase(property.Name): @Raw(GetModelName(Model, property.Type, isInterface: true));
</text>
	}
}
