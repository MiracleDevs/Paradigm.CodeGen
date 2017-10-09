using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Paradigm.CodeGen.Input.NetAssembly.Collections.Interfaces;
using Paradigm.CodeGen.Tests.Fixtures;
using Paradigm.CodeGen.Tests.Mocks;
using NUnit.Framework;
using Paradigm.CodeGen.Input.Models.Definitions;

namespace Paradigm.CodeGen.Tests.Plugins.Input.NetAssembly.Providers
{
    [TestFixture]
    public class EnumValueDefinitionProviderTest
    {
        private ContainerFixture ContainerFixture { get; set; }

        #region Setup

        [OneTimeSetUp]
        public void Setup()
        {
            this.ContainerFixture = new ContainerFixture();
        }

        #endregion

        #region TearDown

        [OneTimeTearDown]
        public void TearDown()
        {
            this.ContainerFixture.Dispose();
        }

        #endregion

        #region Public Methods

        [TestCase(typeof(MockEnum), typeof(int))]
        [TestCase(typeof(MockEnumLong), typeof(long))]
        [TestCase(typeof(MockEnumULong), typeof(ulong))]
        [TestCase(typeof(MockEnumInt), typeof(int))]
        [TestCase(typeof(MockEnumUInt), typeof(uint))]
        [TestCase(typeof(MockEnumShort), typeof(short))]
        [TestCase(typeof(MockEnumUShort), typeof(ushort))]
        [TestCase(typeof(MockEnumByte), typeof(byte))]
        [TestCase(typeof(MockEnumSByte), typeof(sbyte))]
        public void ShouldEqualNameAndValueInGetFromSourceMethod(Type enumType, Type baseType)
        {
            var zippedEnumValueDefinition = Enum.GetValues(enumType).Cast<object>()
                                                                    .Select(x => new Dictionary<string, object>
                                                                                 {
                                                                                    { "EnumValue", x },
                                                                                    { "EnumValueDefinition", this.ContainerFixture.EnumValueProvider.GetFromSource(x) }
                                                                                 })
                                                                    .ToList();

            foreach (var enumRes in zippedEnumValueDefinition)
            {
                enumRes["EnumValue"].ToString().Should().Equals(((EnumValueDefinition)enumRes["EnumValueDefinition"]).Name);
                this.GetEnumValue(baseType, (Enum)enumRes["EnumValue"]).Should().Equals(((EnumValueDefinition)enumRes["EnumValueDefinition"]).Value);
            }

        }

        [TestCase(typeof(MockEnum), typeof(int))]
        [TestCase(typeof(MockEnumLong), typeof(long))]
        [TestCase(typeof(MockEnumULong), typeof(ulong))]
        [TestCase(typeof(MockEnumInt), typeof(int))]
        [TestCase(typeof(MockEnumUInt), typeof(uint))]
        [TestCase(typeof(MockEnumShort), typeof(short))]
        [TestCase(typeof(MockEnumUShort), typeof(ushort))]
        [TestCase(typeof(MockEnumByte), typeof(byte))]
        [TestCase(typeof(MockEnumSByte), typeof(sbyte))]
        public void ShouldEqualNameAndValueInProcessMethod(Type enumType, Type baseType)
        {
            var objectDefinitions = this.ContainerFixture.Container.Resolve<IObjectDefinitions<Type>>();

            var zippedEnumValueDefinition = Enum.GetValues(enumType).Cast<object>()
                                                                    .Select(x => new Dictionary<string, object>
                                                                                 {
                                                                                    { "EnumValue", x },
                                                                                    { "EnumValueDefinition", this.ContainerFixture.EnumValueProvider.Process(this.ContainerFixture.EnumValueProvider.GetFromSource(x), objectDefinitions, x) }
                                                                                 })
                                                                    .ToList();

            foreach (var enumRes in zippedEnumValueDefinition)
            {
                enumRes["EnumValue"].ToString().Should().Equals(((EnumValueDefinition)enumRes["EnumValueDefinition"]).Name);
                this.GetEnumValue(baseType, (Enum)enumRes["EnumValue"]).Should().Equals(((EnumValueDefinition)enumRes["EnumValueDefinition"]).Value);
            }

        }

        private string GetEnumValue(Type type, Enum enumeration)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.SByte:
                    return Convert.ToSByte(enumeration).ToString();

                case TypeCode.Byte:
                    return Convert.ToByte(enumeration).ToString();

                case TypeCode.Int16:
                    return Convert.ToInt16(enumeration).ToString();

                case TypeCode.Int32:
                    return Convert.ToInt32(enumeration).ToString();

                case TypeCode.Int64:
                    return Convert.ToInt64(enumeration).ToString();

                case TypeCode.UInt16:
                    return Convert.ToUInt16(enumeration).ToString();

                case TypeCode.UInt32:
                    return Convert.ToUInt32(enumeration).ToString();

                case TypeCode.UInt64:
                    return Convert.ToUInt64(enumeration).ToString();

                default:
                    throw new ArgumentException();
            }
        }

        #endregion

    }
}