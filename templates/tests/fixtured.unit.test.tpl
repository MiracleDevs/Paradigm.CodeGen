
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

	public class @(name)Fixture : IDisposable
	{
		public @(name)Fixture()
		{
			// ... initialize shared data in the test (ie: database connection)...
		}

		public void Dispose()
		{
			// ... clean up test data from shared resource ...
		}

	}


	public class @name : IClassFixture<@(name)Fixture>
    {

		@(name)Fixture fixture;

        #region Constructor

        public @(name)(@(name)Fixture fixture)
        {
			this.fixture = fixture;
        }

        #endregion

		#region Public Methods
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

		#endregion
    }
}