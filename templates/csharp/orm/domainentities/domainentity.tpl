@include "../shared.cs"
@{
	var name = Model.Definition.Name;
	var interfaceName = $"I{name.ToString().Replace("View", "")}";
	var tableName = $"I{name}Table";
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
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Paradigm.ORM.Data.Attributes;
using Paradigm.Core.Mapping.Interfaces;
using Paradigm.Core.Mapping;
using Paradigm.Services.Domain;
using Microsoft.Extensions.DependencyInjection;
using @Model.Configuration["InterfacesNamespace"];
using @Model.Configuration["TablesNamespace"];

namespace @Model.Configuration["Namespace"]
{
    /// <summary>
    /// Represents a @Raw(GetReadableString(Model.Definition.Name)) domain entity.
    /// </summary>
	[DataContract]
	[TableType(typeof(@tableName))]
	public partial class @Raw(name) : @Raw(GetInheritance(Model))
    {
@if (navigationProperties.Any())
{
	<text>@Raw(GetPrivateFieldList(Model, navigationProperties, "\t\t"))</text>
}
        #region Private Properties

        private IServiceProvider ServiceProvider { get; }

        #endregion
@if (properties.Any())
{
	<text>@Raw(GetPublicPropertyList(Model, properties, "\t\t"))</text>
}
		#region Constructor

        /// @Raw("<summary>")
        /// Initializes a new instance of the @Raw("<see cref=\"" + name + "\"/>") class.
        /// @Raw("</summary>")
        /// @Raw("<param name=\"serviceProvider\">")A reference to the service provider.@Raw("</param>")
		public @(name)(IServiceProvider serviceProvider)
		{
            this.ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            @Raw(GetConstructorList(Model, navigationProperties, "\t\t\t"))
			this.Initialize();
		}

		#endregion
@if (ImplementsDomainInterface(Model))
{
<text>
		#region Public Methods

        /// @Raw("<summary>")
        /// Creates a new @Raw("<see cref=\"" + name + "\"/>") from a domain interface.
        /// @Raw("</summary>")
        /// @Raw("<param name=\"serviceProvider\">")A reference to the service provider.@Raw("</param>")
        /// @Raw("<param name=\"model\">")The model.@Raw("</param>")
        /// @Raw("<returns>")A new entity.@Raw("</returns>")
		public static  @Raw(name) FromInterface(IServiceProvider serviceProvider, @Raw(interfaceName) model)
    	{
    		return new @(name)(serviceProvider).MapFrom(model);
    	}

        /// @Raw("<summary>")
        /// Registers the mapping between this entity and its domain interface.
        /// @Raw("</summary>")
        /// @Raw("<param name=\"mapper\">")The mapper.@Raw("</param>")
        /// @Raw("<param name=\"serviceProvider\">")The service provider.@Raw("</param>")
		public static void RegisterMapping(IMapper mapper, IServiceProvider serviceProvider)
    	{
    		if (mapper.MapExists(typeof(@Raw(interfaceName)), typeof(@Raw(name))))
    			return;

    		var configuration = mapper.Register<@Raw(interfaceName), @Raw(name)>()
                                      .ConstructUsing(x => serviceProvider.GetRequiredService<@Raw(name)>());
			@Raw(GetIgnorePropertyList(Model, navigationProperties, "\t\t\t"))
			AfterRegisterMapping(configuration);
    	}

        /// @Raw("<summary>")
        /// Maps the entity from its domain interface.
        /// @Raw("</summary>")
        /// @Raw("<param name=\"model\">")The model.@Raw("</param>")
        /// @Raw("<returns>")The mapped component.@Raw("</returns>")
		public override @Raw(name) MapFrom(@Raw(interfaceName) model)
    	{
			return this.MapFrom(Mapper.Container, model);
		}

        /// @Raw("<summary>")
        /// Maps the entity from its domain interface.
        /// @Raw("</summary>")
        /// @Raw("<param name=\"mapper\">")The mapper.@Raw("</param>")
        /// @Raw("<param name=\"model\">")The model.@Raw("</param>")
        /// @Raw("<returns>")The mapped component.@Raw("</returns>")
    	public @Raw(name) MapFrom(IMapper mapper, @Raw(interfaceName) model)
    	{
    		this.BeforeMap(model);

			mapper.Map<@Raw(interfaceName), @Raw(name)>(model, this);
			@Raw(GetMapPropertyList(Model, navigationProperties, "\t\t\t"))
			this.AfterMap(model);

    		return this;
    	}

        /// @Raw("<summary>")
        /// Validates this instance.
        /// @Raw("</summary>")
		public override void Validate()
		{
    		@Raw(GetIgnoreProperties(Model.Configuration["IgnoreProperties"], properties))
    		@Raw(GetPropertyValidations(Model, navigationProperties, "\t\t\t"))
		}

        /// @Raw("<summary>")
        /// Determines whether this instance is new.
        /// @Raw("</summary>")
        /// @Raw("<returns>")
        ///   @Raw("<c>true</c>") if this instance is new; otherwise, @Raw("<c>false</c>").
        /// @Raw("</returns>")
		public override bool IsNew()
		{
			return @Raw(GetIsNewCondition(Model.Definition));
		}
</text>
    if (navigationProperties.Any())
    {
		<text>@Raw(GetCrudMethods(Model, navigationProperties, "\t\t"))</text>
    }
<text>
		#endregion
</text>
}

@if (navigationProperties.Any())
{
		<text>@Raw(GetPrivateMethods(Model, navigationProperties, "\t\t"))</text>
}
		#region Partial Methods

        /// @Raw("<summary>")
        /// Initializes the entity right after the instantiation.
        /// @Raw("</summary>")
		partial void Initialize();
@if (ImplementsDomainInterface(Model))
{
<text>
        /// @Raw("<summary>")
        /// Executes domain logic after the mapping is registered.
        /// @Raw("</summary>")
        /// @Raw("<param name=\"configuration\">")The configuration.@Raw("</param>")
		static partial void AfterRegisterMapping(IMemberConfiguration<@Raw(interfaceName), @Raw(name)> configuration);

        /// @Raw("<summary>")
        /// Executes domain logic right before the entity is mapped from a domain interface.
        /// @Raw("</summary>")
        /// @Raw("<param name=\"model\">")The model.@Raw("</param>")
    	partial void BeforeMap(@Raw(interfaceName) model);

        /// @Raw("<summary>")
        /// Executes domain logic right after the entity has been mapped from a domain interface.
        /// @Raw("</summary>")
        /// @Raw("<param name=\"model\">")The model.@Raw("</param>")
    	partial void AfterMap(@Raw(interfaceName) model);
</text>
}

		#endregion
    }
}
