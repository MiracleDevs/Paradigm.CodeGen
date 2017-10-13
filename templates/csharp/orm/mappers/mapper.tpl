@include "../shared.cs"
@{
	var name = $"{Model.Definition.Name}DatabaseReaderMapper";
	var interfaceName = $"I{Model.Definition.Name}DatabaseReaderMapper";
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
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Mappers.Generic;
using @Model.Configuration["DomainNamespace"];

namespace @Model.Configuration["Namespace"]
{
	public class @Raw(name) : DatabaseReaderMapper<@Raw(entityName)>, @Raw(interfaceName)
    {
		#region Protected Methods

		protected override object MapRow(IDatabaseReader reader)
		{
			var entity = new @(entityName)();
			@Raw(MapProperties(Model, "entity", "reader", false, true))
			return entity;
		}

		protected override async System.Threading.Tasks.Task<object> MapRowAsync(IDatabaseReader reader)
		{
			var entity = new @(entityName)();
			@Raw(MapProperties(Model, "entity", "reader", true, true))
			return entity;
		}

		#endregion
    }
}