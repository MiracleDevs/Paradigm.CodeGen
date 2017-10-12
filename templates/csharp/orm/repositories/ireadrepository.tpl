@include "../shared.cs"
@{	
	var entityName = @Raw(Model.Definition.Name);
	var name = Raw($"I{entityName}Repository");

}//////////////////////////////////////////////////////////////////////////////////
//  @(name + ".cs")
//
//  Generated with the Paradigm.CodeGen tool.
//  Do not modify this file in any way.
//
//  Copyright (c) 2016 miracledevs. All rights reserved.
//////////////////////////////////////////////////////////////////////////////////

using Paradigm.Services.Repositories;
using @Model.Configuration["DomainNamespace"];

namespace @Model.Configuration["Namespace"]
{
	public partial interface @name : IReadRepository<@entityName, @Raw(GetPrimaryKeyForRepositories(Model))>
	{	
	}
}