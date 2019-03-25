@include "./shared.cs"
@{
	var interfaceName = GetServiceName(Model.Definition, isInterface: true);
	var name = GetServiceName(Model.Definition, isInterface: false);
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
    /// Implements the @Raw(GetReadableString(GetServiceName(Model.Definition, isInterface: false))) interface.
    /// </summary>
    public class @Raw(name): HttpService, @Raw(interfaceName)
    {
        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="@Raw(name)"/> class.
        /// </summary>
        /// <param name="httpClient">The http client.</param>
        public @(Raw(name))(@Raw(Model.Configuration["HttpClient"]) httpClient): base(httpClient)
        {
        }

        #endregion

        #region Public Method

        @foreach(var method in (Model.Definition as Paradigm.CodeGen.Input.Models.Definitions.StructDefinition).Methods)
        {
<text>        /// @Raw("<summary>")
        /// Calls the method @Raw(GetReadableString(method.Name)).
        /// @Raw("</summary>")
        public @Raw(MethodFirm(Model, method, false))
        {
            @Raw(MethodBody(Model, method, GetServiceName(Model.Definition, isInterface: false)))
        }

</text>
        }
        #endregion
    }
}