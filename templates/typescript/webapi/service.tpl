@include "shared.cs"
@{
	var interfaceName = GetServiceName(Model.Definition, isInterface: true);
	var name = GetServiceName(Model.Definition, isInterface: false);
	var contracts = GetServiceRelatedContracts(Model, Model.Definition);
}//////////////////////////////////////////////////////////////////////////////////
//  @Raw(name + ".ts")
//
//  Generated with the Paradigm.CodeGen tool.
//  Do not modify this file in any way.
//
//  Copyright (c) 2016 miracledevs. All rights reserved.
//////////////////////////////////////////////////////////////////////////////////

import { IHttpPromise, IHttpService } from "angular";
import { AngularServices, Service } from "@@miracledevs/paradigm-ui-web-angularjs";
import { HttpServiceBase } from "@Raw(Model.Configuration["HttpServiceBasePath"])";

@foreach(var contract in contracts)
{
<text>import { @Raw(GetModelName(Model, contract, isInterface: true)) } from "./models/@Raw(GetFileName(Model, contract, isInterface: true))";
</text>
}

@@Service({
	name: "@Raw(name)",
	dependencies: [AngularServices.http]
})
export class @Raw(name) extends HttpServiceBase
{
	constructor(http: IHttpService)
	{
		super(http);
	}
	@foreach(var method in (Model.Definition as Paradigm.CodeGen.Input.Models.Definitions.StructDefinition).Methods)
	{
<text>
	@Raw(MethodFirm(Model, method, false))
	{
		@Raw(MethodBody(Model, method, GetServiceName(Model.Definition, isInterface: false)))
	}
</text>
	}

	static factory(http: IHttpService): @Raw(name)
	{
		return new @(name)(http);
	}
}
