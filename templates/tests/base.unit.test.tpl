﻿
@{
	var structDefinition = Model.Definition as MiracleDevs.CodeGenerator.Input.Models.Definitions.StructDefinition;
	var name = Raw($"{Model.Definition.Name}Tests");
}//////////////////////////////////////////////////////////////////////////////////
//  @(name + ".cs")
//
//  Generated with the MiracleDevs.CodeGenerator tool.
//
//  Copyright (c) 2016 miracledevs. All rights reserved.
//////////////////////////////////////////////////////////////////////////////////

using System;
using Xunit;
using @(Model.Definition.FullName);

namespace UnitTests.@(Model.Definition.Name)
{
	public class @name
    {
		@foreach(var method in structDefinition.Methods) 
		{
<text>
		[Fact]
		public void @(Raw(method.Name))Test()
		{
			throw new NotImplementedException();
		}
</text>
		}

    }
}