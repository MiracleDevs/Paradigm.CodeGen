﻿@include "../shared.cs"
@{
	var entityName = Model.Definition.Name;
	var name = $"{entityName}Repository";
	var interfaceName = $"I{name}";
	var databaseAccess = $"I{entityName}DatabaseAccess";

	var properties = (Model.Definition as Paradigm.CodeGen.Input.Models.Definitions.StructDefinition).Properties;
	var navigationProperties = properties.Where(x => x.Attributes.Any(a => a.Name == "NavigationAttribute")).ToList();

}//////////////////////////////////////////////////////////////////////////////////
//  @(name + ".cs")
//
//  Generated with the Paradigm.CodeGen tool.
//  Do not modify this file in any way.
//
//  Copyright (c) 2016 miracledevs. All rights reserved.
//////////////////////////////////////////////////////////////////////////////////

using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Paradigm.Services.Repositories.ORM;
using Paradigm.Services.Repositories.UOW;
using @Model.Configuration["DatabaseAccessNamespace"];
using @Model.Configuration["DomainEntitiesNamespace"];
using @Model.Configuration["DomainRepositoriesNamespace"];
using FrameworkTask = System.Threading.Tasks.Task;

namespace @Model.Configuration["Namespace"]
{
	public partial class @Raw(name) : EditRepositoryBase<@Raw(entityName), @Raw(GetPrimaryKeyForRepositories(Model)), @databaseAccess>, @Raw(interfaceName)
	{
		#region Constructor

		public @(name)(IServiceProvider serviceProvider) : base(serviceProvider, serviceProvider.GetService<@databaseAccess>(), serviceProvider.GetService<IUnitOfWork>())
	    {
	    }

		#endregion

		#region Protected Methods

		public override void Edit(@(entityName) entity)
	    {
			this.BeforeEdit(entity);
			@Raw(GetSingleEditRemoval(Model, navigationProperties, "\t\t\t"))
            base.Edit(entity);

			this.AfterEdit(entity);
	    }

	    public override void Edit(IEnumerable<@(entityName)> entities)
	    {
			var entityList = entities as IList<@(entityName)> ?? entities.ToList();
			this.BeforeEdit(entityList);
			@Raw(GetMultiEditRemoval(Model, navigationProperties, "\t\t\t"))
            base.Edit(entityList);

			this.AfterEdit(entityList);
	    }

		public override async FrameworkTask EditAsync(@(entityName) entity)
	    {
			this.BeforeEdit(entity);
			@Raw(GetSingleEditRemoval(Model, navigationProperties, "\t\t\t", "await ", "Async"))
            await base.EditAsync(entity);

			this.AfterEdit(entity);
	    }

	    public override async FrameworkTask EditAsync(IEnumerable<@(entityName)> entities)
	    {
			var entityList = entities as IList<@(entityName)> ?? entities.ToList();
			this.BeforeEdit(entityList);
			@Raw(GetMultiEditRemoval(Model, navigationProperties, "\t\t\t", "await ", "Async"))
            await base.EditAsync(entityList);

			this.AfterEdit(entityList);
	    }

		#endregion
@if (navigationProperties.Any())
{
	<text>@Raw(GetRemoveMethods(Model, navigationProperties, "\t\t"))</text>
}

		#region Partial Methods

		partial void BeforeEdit(@(entityName) entity);

		partial void BeforeEdit(IEnumerable<@(entityName)> entities);

		partial void AfterEdit(@(entityName) entity);

		partial void AfterEdit(IEnumerable<@(entityName)> entities);

		#endregion
	}
}