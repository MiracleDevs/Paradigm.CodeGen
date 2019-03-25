@include "../shared.cs"
@{
	var entityName = Model.Definition.Name;
	var name = $"{entityName}Repository";
	var interfaceName = $"I{name}";
	var databaseAccess = $"I{entityName}DatabaseAccess";

}//////////////////////////////////////////////////////////////////////////////////
//  @(name + ".cs")
//
//  Generated with the Paradigm.CodeGen tool.
//  Do not modify this file in any way.
//
//  Copyright (c) 2016 miracledevs. All rights reserved.
//////////////////////////////////////////////////////////////////////////////////

using System;
using Microsoft.Extensions.DependencyInjection;
using Paradigm.Services.Repositories.ORM;
using Paradigm.Services.Repositories.UOW;
using @Model.Configuration["DatabaseAccessNamespace"];
using @Model.Configuration["DomainEntitiesNamespace"];
using @Model.Configuration["DomainRepositoriesNamespace"];

namespace @Model.Configuration["Namespace"]
{
    /// <summary>
    /// Represents a @Raw(GetReadableString(Model.Definition.Name)) repository that can read but not modify the entities.
    /// </summary>
	public partial class @Raw(name) : ReadRepositoryBase<@Raw(entityName), @Raw(GetPrimaryKeyForRepositories(Model)), @databaseAccess>, @Raw(interfaceName)
	{
		#region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="@Raw(name)"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
	    public @(name)(IServiceProvider serviceProvider) : base(serviceProvider, serviceProvider.GetRequiredService<@databaseAccess>(), serviceProvider.GetRequiredService<IUnitOfWork>())
	    {
	    }

		#endregion
	}
}