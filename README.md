[![Build Status](https://travis-ci.org/MiracleDevs/Paradigm.CodeGen.svg?branch=master)](https://travis-ci.org/MiracleDevs/Paradigm.CodeGen)

# Paradigm.CodeGen
Code generation / scaffolding tool similar to T4s, but utilizing Razor as template language. The codegen is fully configurable and pluginable, and can produce output in any language. Currenly is being used by the Paradigm.ORM to generate dbfirst classes, and to generate Typescript services and model for the Paradigm.AngularJS and Paradigm.Angular wrappers.


JSON input
---

The codegen tool comes with two input methods out of the box:
- .NET CORE Assemblies
- JSON object model
You can provide your own plugins if with custom sources, for example databases, xml, or other datasource origins.
If you have your code already in place and can not create a new plugin for CodeGen, you can export the object model in json format, and the CodeGen will take it from there. We provide a nuget package with the json model structure that CodeGen expects, so you can download the nuget and fill the objects with your content, and then send it to CodeGen.

| Library | Nuget | Install
|-|-|-|
| Paradigm.CodeGen.Input.Json.Models | [![NuGet](https://img.shields.io/nuget/v/Nuget.Core.svg)](https://www.nuget.org/packages/Paradigm.CodeGen.Input.Json.Models/) | `Install-Package Paradigm.CodeGen.Input.Json.Models` |


Change log
---

Version `2.0.1`
- Updated Paradigm.Core to version `2.0.1`.

Version `2.0.0`
- Updated .net core from version 1 to version 2.

Version `1.0.0`
- Uploaded first version of the Paradigm CodeGen.
