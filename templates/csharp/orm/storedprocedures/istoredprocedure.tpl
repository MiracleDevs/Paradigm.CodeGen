@include "../shared.cs"
@{
	var entityName = Model.Definition.Name;
	var name = $"I{entityName}StoredProcedure";
	var parameters = $"{entityName}Parameters";

}//////////////////////////////////////////////////////////////////////////////////
//  @(name + ".cs")
//
//  Generated with the Paradigm.CodeGen tool.
//  Do not modify this file in any way.
//
//  Copyright (c) 2016 miracledevs. All rights reserved.
//////////////////////////////////////////////////////////////////////////////////

using Paradigm.ORM.Data.StoredProcedures;
using @Model.Configuration["DomainNamespace"];

namespace @Model.Configuration["Namespace"]
{
	public interface @Raw(name) : @Raw(GetStoredProcedureInterface(Model))
    {
    }
}