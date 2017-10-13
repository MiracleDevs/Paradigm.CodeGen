@include "shared.cs"
@{
	var name = Model.Definition.name;
}//////////////////////////////////////////////////////////////////////////////////
//  @(name + ".ts")
//
//  Generated with the Paradigm.CodeGen tool.
//  Do not modify this file in any way.
//
//  Copyright (c) 2016 miracledevs. All rights reserved.
//////////////////////////////////////////////////////////////////////////////////

export enum @Raw(name)
{
	@foreach(var value in (Model.Definition as Paradigm.CodeGen.Input.Models.Definitions.EnumDefinition).Values)
	{
<text>	@Raw(value.name) = @Raw(value.Value),
</text>
	}
}
