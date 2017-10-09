@include "../shared.cs"
@{	
	var entityName = Raw(Model.Definition.Name);
	var name = $"{entityName}StoredProcedure";	
	var interfaceName = $"I{entityName}StoredProcedure";	
	var parameters = $"{entityName}Parameters";
	var routine = Model.Definition.Attributes.FirstOrDefault(x => x.Name == "RoutineAttribute");

}//////////////////////////////////////////////////////////////////////////////////
//  @(name + ".cs")
//
//  Generated with the MiracleDevs.CodeGenerator tool.
//  Do not modify this file in any way.
//
//  Copyright (c) 2016 miracledevs. All rights reserved.
//////////////////////////////////////////////////////////////////////////////////

using System;
using System.Runtime.Serialization;
using MiracleDevs.ORM.Data.StoredProcedures;
using @Model.Configuration["DomainNamespace"];
using @Model.Configuration["MapperNamespace"];

namespace @Model.Configuration["Namespace"]
{
	[DataContract]
	public class @name : @Raw(GetStoredProcedureBaseClass(Model)), @interfaceName
    {
		#region Constructor

		public @(name)(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

		#endregion
    }
}