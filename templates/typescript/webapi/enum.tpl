@include "shared.cs"
@{
	var Name = Raw(Model.Definition.Name);
}//////////////////////////////////////////////////////////////////////////////////
//  @(Name + ".ts")
//
//  Generated with the Paradigm.CodeGen tool.
//  Do not modify this file in any way.
//
//  Copyright (c) 2016 miracledevs. All rights reserved.
//////////////////////////////////////////////////////////////////////////////////

module @Model.Configuration["Namespace"]
{
	export enum @Name
	{
	@foreach(var value in (Model.Definition as Paradigm.CodeGen.Input.Models.Definitions.EnumDefinition).Values)
	{	
<text>
		@Raw(value.Name) = @Raw(value.Value),
</text>
	}
	}
}