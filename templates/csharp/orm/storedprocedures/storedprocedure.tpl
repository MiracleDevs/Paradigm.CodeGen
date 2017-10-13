@include "../shared.cs"
@{
	var entityName = Model.Definition.Name;
	var name = $"{entityName}StoredProcedure";
	var interfaceName = $"I{entityName}StoredProcedure";
	var parameters = $"{entityName}Parameters";
	var routine = (Model.Definition as Paradigm.CodeGen.Input.Models.Definitions.StructDefinition).Attributes.FirstOrDefault(x => x.Name == "RoutineAttribute");

}//////////////////////////////////////////////////////////////////////////////////
//  @(name + ".cs")
//
//  Generated with the Paradigm.CodeGen tool.
//  Do not modify this file in any way.
//
//  Copyright (c) 2016 miracledevs. All rights reserved.
//////////////////////////////////////////////////////////////////////////////////

using System;
using System.Runtime.Serialization;
using Paradigm.ORM.Data.StoredProcedures;
using @Model.Configuration["DomainNamespace"];
using @Model.Configuration["MapperNamespace"];

namespace @Model.Configuration["Namespace"]
{
	[DataContract]
	public class @Raw(name) : @Raw(GetStoredProcedureBaseClass(Model)), @Raw(interfaceName)
    {
		#region Constructor

		public @(name)(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

		#endregion
    }
}