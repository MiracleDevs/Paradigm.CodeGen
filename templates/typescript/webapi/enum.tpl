@include "shared.cs"
@{
    var name = Model.Definition.Name;
}//////////////////////////////////////////////////////////////////////////////////
//  generated with the Paradigm.CodeGen tool.
//  do not modify this file in any way.
//
//  copyright (c) 2016 miracledevs. All rights reserved.
//////////////////////////////////////////////////////////////////////////////////

export enum @Raw(name)
{
    @foreach(var value in (Model.Definition as Paradigm.CodeGen.Input.Models.Definitions.EnumDefinition).Values)
    {
<text>    @Raw(value.Name) = @Raw(value.Value),
</text>
    }
}
