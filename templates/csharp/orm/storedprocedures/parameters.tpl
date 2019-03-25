@include "../shared.cs"
@{
	var entityName = Model.Definition.Name;
	var name = $"{entityName}Parameters";
	var properties = (Model.Definition as Paradigm.CodeGen.Input.Models.Definitions.StructDefinition).Properties;

}//////////////////////////////////////////////////////////////////////////////////
//  @(name + ".cs")
//
//  Generated with the Paradigm.CodeGen tool.
//  Do not modify this file in any way.
//
//  Copyright (c) 2016 miracledevs. All rights reserved.
//////////////////////////////////////////////////////////////////////////////////

using System;
using System.Runtime.Serialization;
using Paradigm.ORM.Data.Attributes;

namespace @Model.Configuration["Namespace"]
{
    /// <summary>
    /// Defines an object that groups all the parameters required to call the '@Raw(Model.Definition.Name)' procedure.
    /// </summary>
	[DataContract]@Raw(GetAttributes(Model.Definition.Attributes, "\t"))
	public class @Raw(name)
    {
        #region Properties
		@foreach(var property in properties)
		{
<text>
        /// @Raw("<summary>")
        /// Gets or sets the @Raw(GetReadableString(property.Name)).
        /// @Raw("</summary>")
		[DataMember]@Raw(GetAttributes(property.Attributes, "\t\t"))
		public @Raw(GetModelName(Model, property.Type, true)) @property.Name { get; set; }
</text>
		}

		#endregion
    }
}