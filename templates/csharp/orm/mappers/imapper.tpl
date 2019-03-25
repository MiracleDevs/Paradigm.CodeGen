@include "../shared.cs"
@{
	var name = $"I{Model.Definition.Name}DatabaseReaderMapper";
	var entityName = Model.Definition.Name;
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
    /// <summary>
    /// Provides an interface for an object that takes a @Raw(GetReadableString(Model.Definition.Name)) from the database and maps it to a <see cref="@Raw(Model.Definition.Name)"/> class.
    /// </summary>
	public interface @Raw(name) : IDatabaseReaderMapper<@Raw(entityName)>
    {
    }
}