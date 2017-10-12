@include "../shared.cs"
@{
	var name = Raw(Model.Definition.Name);
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
using @Model.Configuration["InterfacesNamespace"];
using @Model.Configuration["TablesNamespace"];

namespace @Model.Configuration["Namespace"]
{
	[DataContract]
	[TableType(typeof(@tableName))]
	public partial class @name : @Raw(GetInheritance(Model))
    {
@if (navigationProperties.Any())
{
	<text>@Raw(GetPrivateFieldList(Model, navigationProperties, "\t\t"))</text>
}
@if (properties.Any())
{
	<text>@Raw(GetPublicPropertyList(Model, properties, "\t\t"))</text>
}
		#region Constructor

		public @(name)()
		{	@Raw(GetConstructorList(Model, navigationProperties, "\t\t\t"))
			this.Initialize();
		}

		#endregion
@if (ImplementsDomainInterface(Model))
{
<text>
		#region Public Methods

		public static  @name FromInterface(@interfaceName model)
    	{
    		return new @(name)().MapFrom(model);
    	}
    
		public static void RegisterMapping(IMapper mapper)
    	{
    		if (mapper.MapExists(typeof(@interfaceName), typeof(@name)))
    			return;
    
    		var configuration = mapper.Register<@interfaceName, @name>();
			@Raw(GetIgnorePropertyList(Model, navigationProperties, "\t\t\t"))   		
			AfterRegisterMapping(configuration);
    	}

		public override @name MapFrom(@interfaceName model)
    	{
			return this.MapFrom(Mapper.Container, model);
		}
    
    	public @name MapFrom(IMapper mapper, @interfaceName model)
    	{
    		this.BeforeMap(model);
    		
			mapper.Map<@interfaceName, @name>(model, this); 
			@Raw(GetMapPropertyList(Model, navigationProperties, "\t\t\t"))     	
			this.AfterMap(model);
			
    		return this;
    	}

		public override void Validate()
		{			
    		@Raw(GetIgnoreProperties(Model.Configuration["IgnoreProperties"], properties))
    		@Raw(GetPropertyValidations(Model, navigationProperties, "\t\t\t"))    
		}

		public override bool IsNew()
		{
			return @Raw(GetIsNewCondition(Model.Definition));
		}
@if (navigationProperties.Any())
{
		<text>@Raw(GetCrudMethods(Model, navigationProperties, "\t\t"))</text>
}

		#endregion
</text>
}

@if (navigationProperties.Any())
{
		<text>@Raw(GetPrivateMethods(Model, navigationProperties, "\t\t"))</text>
}
		#region Partial Methods
	
		partial void Initialize();
@if (ImplementsDomainInterface(Model))
{
<text>		
		static partial void AfterRegisterMapping(IMemberConfiguration<@interfaceName, @name> configuration);

    	partial void BeforeMap(@interfaceName model);
    
    	partial void AfterMap(@interfaceName model);
</text>
}  
		
		#endregion
    }
}
