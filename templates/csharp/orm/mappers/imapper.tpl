@include "../shared.cs"
@{
	var name = Raw($"I{Model.Definition.Name}DatabaseReaderMapper");
	var entityName = @Raw(Model.Definition.Name);
}//////////////////////////////////////////////////////////////////////////////////
//  @(name + ".cs")
//
//  Generated with the Paradigm.CodeGen tool.
//  Do not modify this file in any way.
//
//  Copyright (c) 2016 miracledevs. All rights reserved.
//////////////////////////////////////////////////////////////////////////////////

using System;
using Paradigm.ORM.Data.Mappers.Generic;
using @Model.Configuration["DomainNamespace"];

namespace @Model.Configuration["Namespace"]
{
	public interface @name : IDatabaseReaderMapper<@entityName>
    {
    }
}