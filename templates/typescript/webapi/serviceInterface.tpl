@include "shared.cs"
@{
	var Name = Raw(GetServiceName(Model.Definition, isInterface: true));
	var contracts = GetServiceRelatedContracts(Model, Model.Definition);
}//////////////////////////////////////////////////////////////////////////////////
//  @(Name + ".ts")
//
//  Generated with the Paradigm.CodeGen tool.
//  Do not modify this file in any way.
//
//  Copyright (c) 2016 miracledevs. All rights reserved.
//////////////////////////////////////////////////////////////////////////////////

///<reference path="@Raw(Model.Configuration["TypingsPath"])" />
@foreach(var contract in contracts)
{
<text>///<reference path="@Raw(GetRelativeModelDirectoryForServices(Model, Model.Configuration, contract, isInterface: true))" />
</text>
}

module @Model.Configuration["Namespace"]
{
	import IHttpPromise = angular.IHttpPromise;
	@foreach(var contract in contracts)
	{
		var modelName = GetModelName(Model, contract, isInterface: true);
<text>	import @Raw(modelName) = @Raw(Model.Configuration["ModelNamespace"] + "." +  modelName);
</text>	
	}

	export interface @Name
	{
	@foreach(var method in (Model.Definition as Paradigm.CodeGen.Input.Models.Definitions.StructDefinition).Methods)
	{	
<text>
		@Raw(MethodFirm(Model, method, true))
</text>
	}
	}
}