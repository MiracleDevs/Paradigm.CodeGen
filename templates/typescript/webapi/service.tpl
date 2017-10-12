@include "shared.cs"
@{
	var InterfaceName = Raw(GetServiceName(Model.Definition, isInterface: true));
	var Name = Raw(GetServiceName(Model.Definition, isInterface: false));
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
///<reference path="@Raw(Model.Configuration["HttpServicesPath"])" />
///<reference path="@Raw(Model.Configuration["HttpServiceBasePath"])" />
///<reference path="@(InterfaceName + ".ts")" />

module @Model.Configuration["Namespace"]
{
	import IHttpPromise = angular.IHttpPromise;
	import IHttpService = angular.IHttpService;
	import AngularServices = @(Raw(Model.Configuration["MiracleAngularNamespace"])).Services.AngularServices;
	import IServiceRegister = @(Raw(Model.Configuration["MiracleAngularNamespace"])).Interfaces.IServiceRegister;
	import BuildInfo = @(Raw(Model.Configuration["MiracleAngularNamespace"])).BuildInfo;

	@foreach(var contract in contracts)
	{
		var modelName = GetModelName(Model, contract, isInterface: true);
<text>	import @Raw(modelName) = @Raw(Model.Configuration["ModelNamespace"] + "." +  modelName);
</text>	
	}

	export class @Name extends HttpServiceBase implements @InterfaceName
	{
		public static register: IServiceRegister = 
		{
			name: HttpServices.@Raw(ToCamelCase(GetServiceName(Model.Definition, isInterface: false))),
            factory: @(Name).factory,
            dependencies: [AngularServices.http]
		};
	@foreach(var method in (Model.Definition as Paradigm.CodeGen.Input.Models.Definitions.StructDefinition).Methods)
	{	
<text>
		@Raw(MethodFirm(Model, method, false))
		{
			@Raw(MethodBody(Model, method, GetServiceName(Model.Definition, isInterface: false)))
		}
</text>
	}

		static factory($http: IHttpService): @Name
        {
            return new @Name ($http, BuildInfo.instance.getData<string>("host"));
        }
	}

	////////////////////////////////////////////////////////////
    // Register service
    ////////////////////////////////////////////////////////////
    Application.instance.registerService(@(Name).register);
}