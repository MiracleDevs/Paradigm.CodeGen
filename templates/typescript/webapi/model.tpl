@include "shared.cs"
@{
	var interfaceName = Raw(GetModelName(Model, Model.Definition, isInterface: true));
	var properties = GetProperties(Model.Definition);
	var name = Raw(GetModelName(Model, Model.Definition, isInterface: false));
}//////////////////////////////////////////////////////////////////////////////////
//  @(name + ".ts")
//
//  Generated with the MiracleDevs.CodeGenerator tool.
//  Do not modify this file in any way.
//
//  Copyright (c) 2016 miracledevs. All rights reserved.
//////////////////////////////////////////////////////////////////////////////////

///<reference path="@Raw(Model.Configuration["TypingsPath"])" />
///<reference path="@(interfaceName + ".ts")" />

module @Model.Configuration["Namespace"]
{
	import ModelBase = @(Raw(Model.Configuration["MiracleAngularNamespace"])).Models.ModelBase;	

	export class @name extends ModelBase implements @interfaceName
	{
	@foreach(var property in properties)
	{	
<text>
		@ToCamelCase(property.Name): @Raw(GetModelName(Model, property.Type, isInterface: true));
</text>
	}
	}
}