@include "./shared.cs"
@{
	var name = GetModelName(Model, Model.Definition, false);
	var properties = GetProperties(Model.Definition);
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
using System.Runtime.Serialization;

namespace @Model.Configuration["Namespace"]
{
    /// <summary>
    /// Provides an interface for the @Raw(GetReadableString(name)) proxy model.
    /// </summary>
    [DataContract]
	public partial class @Raw(name)
    {
        #region Properties
		@foreach(var property in properties)
		{
<text>
        /// @Raw("<summary>")
        /// Gets or sets the @Raw(GetReadableString(property.Name)).
        /// @Raw("</summary>")
        [DataMember(Name = "@(ToCamelCase(@property.Name))")]
		public @Raw(GetModelName(Model, property.Type, false)) @property.Name { get; set; }
</text>
		}

		#endregion
    }
}