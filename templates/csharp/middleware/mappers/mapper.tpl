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
    /// <summary>
    /// Maps a @Raw(GetReadableString(Model.Definition.Name)) from the database to a <see cref="@Raw(Model.Definition.Name)"/> class.
    /// </summary>
	public class @Raw(name) : DatabaseReaderMapper<@Raw(entityName)>, @Raw(interfaceName)
    {
		#region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="@Raw(name)"/> class.
        /// </summary>
        /// <param name="connector">A database connector.</param>
		public @(Raw(name))(IDatabaseConnector connector) : base(connector)
        {
        }

		#endregion

		#region Protected Methods
        #pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

        /// <summary>
        /// Maps one row to an object.
        /// </summary>
        /// <param name="reader">An open database reader cursor.</param>
        /// <returns>A new instance.</returns>
		protected override object MapRow(IDatabaseReader reader)
		{
			var entity = new @(entityName)();
			@Raw(MapProperties(Model, "entity", "reader", false, true))
			return entity;
		}

        /// <summary>
        /// Maps one row to an object.
        /// </summary>
        /// <param name="reader">An open database reader cursor.</param>
        /// <returns>A new instance.</returns>
		protected override async System.Threading.Tasks.Task<object> MapRowAsync(IDatabaseReader reader)
		{
			var entity = new @(entityName)();
			@Raw(MapProperties(Model, "entity", "reader", true, true))
			return entity;
		}

        #pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
		#endregion
    }
}