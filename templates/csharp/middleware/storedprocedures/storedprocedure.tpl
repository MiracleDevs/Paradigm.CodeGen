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
using Paradigm.ORM.Data.Database;
using @Model.Configuration["DomainNamespace"];
using @Model.Configuration["MapperNamespace"];

namespace @Model.Configuration["Namespace"]
{
    /// <summary>
    /// Defines a stored procedure caller that calls to '@Raw(Model.Definition.Name)' procedure.
    /// </summary>
	[DataContract]
	public partial class @Raw(name) : @Raw(GetStoredProcedureBaseClass(Model)), @Raw(interfaceName)
    {
		#region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="@Raw(name)"/> class.
        /// </summary>
        /// <param name="connector">A database connector.</param>
        public @(name)(IDatabaseConnector connector) : base(connector)
		{
		}

		#endregion
    }
}