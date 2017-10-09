@include "../shared.cs"
@{
	var name = Raw($"{Model.Definition.Name}DatabaseReaderMapper");
	var interfaceName = Raw($"I{Model.Definition.Name}DatabaseReaderMapper");
	var entityName = @Raw(Model.Definition.Name);
}//////////////////////////////////////////////////////////////////////////////////
//  @(name + ".cs")
//
//  Generated with the MiracleDevs.CodeGenerator tool.
//  Do not modify this file in any way.
//
//  Copyright (c) 2016 miracledevs. All rights reserved.
//////////////////////////////////////////////////////////////////////////////////

using System;
using MiracleDevs.ORM.Data.Database;
using MiracleDevs.ORM.Data.Mappers.Generic;
using @Model.Configuration["DomainNamespace"];

namespace @Model.Configuration["Namespace"]
{
	public class @name : DatabaseReaderMapper<@entityName>, @interfaceName
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