@include "../shared.cs"
@{	
	var entityName = Raw(Model.Definition.Name);
	var name = $"I{entityName}StoredProcedure";	
	var parameters = $"{entityName}Parameters";

}//////////////////////////////////////////////////////////////////////////////////
//  @(name + ".cs")
//
//  Generated with the MiracleDevs.CodeGenerator tool.
//  Do not modify this file in any way.
//
//  Copyright (c) 2016 miracledevs. All rights reserved.
//////////////////////////////////////////////////////////////////////////////////

using MiracleDevs.ORM.Data.StoredProcedures;
using @Model.Configuration["DomainNamespace"];

namespace @Model.Configuration["Namespace"]
{
	public interface @name : @Raw(GetStoredProcedureInterface(Model))
    {
    }
}