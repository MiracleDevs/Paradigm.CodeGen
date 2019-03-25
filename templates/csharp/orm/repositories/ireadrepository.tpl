@include "../shared.cs"
@{
	var entityName = Model.Definition.Name;
	var name = $"I{entityName}Repository";

}//////////////////////////////////////////////////////////////////////////////////
//  @(name + ".cs")
//
//  Generated with the Paradigm.CodeGen tool.
//  Do not modify this file in any way.
//
//  Copyright (c) 2016 miracledevs. All rights reserved.
//////////////////////////////////////////////////////////////////////////////////

using System;
using Paradigm.Services.Repositories;
using @Model.Configuration["DomainNamespace"];

namespace @Model.Configuration["Namespace"]
{
    /// <summary>
    /// Provides an interface for @Raw(GetReadableString(Model.Definition.Name)) repositories that can only read but not modify the entities.
    /// </summary>
	public partial interface @Raw(name) : IReadRepository<@Raw(entityName), @Raw(GetPrimaryKeyForRepositories(Model))>
	{
	}
}