@include "./shared.cs"
@{
	var name = GetServiceName(Model.Definition, isInterface: true);
}//////////////////////////////////////////////////////////////////////////////////
//  @(name + ".cs")
//
//  Generated with the Paradigm.CodeGen tool.
//  Do not modify this file in any way.
//
//  Copyright (c) 2016 miracledevs. All rights reserved.
//////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using @Model.Configuration["HttpServiceNamespace"];
using @(Model.Configuration["Namespace"]).Models;

namespace @Model.Configuration["Namespace"]
{
    /// <summary>
    /// Provides an interface for the @Raw(GetReadableString(GetServiceName(Model.Definition, isInterface: false))).
    /// </summary>
    public interface @Raw(name)
    {
        @foreach(var method in (Model.Definition as Paradigm.CodeGen.Input.Models.Definitions.StructDefinition).Methods)
        {
<text>        /// @Raw("<summary>")
        /// Calls the method @Raw(GetReadableString(method.Name)).
        /// @Raw("</summary>")
        @Raw(MethodFirm(Model, method, true))

</text>
        }
    }
}