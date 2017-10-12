[![Build Status](https://travis-ci.org/MiracleDevs/Paradigm.CodeGen.svg?branch=master)](https://travis-ci.org/MiracleDevs/Paradigm.CodeGen)

# Paradigm.CodeGen
Code generation / scaffolding tool making extensive use of razor as templating language. Codegen is fully configurable and pluginable, and can produce output in any language. Currenly is being used by the [Paradigm.ORM](https://github.com/MiracleDevs/Paradigm.ORM.git) to generate dbfirst classes, and to generate typescript services and model for the Paradigm.AngularJS and Paradigm.Angular wrappers.


Input Types
---

The codegen tool comes with two input methods out of the box:
- .NET CORE Assemblies
- JSON object model

Working with .NET is ideal, because Codegen can open and extract types (codegen intermediate model) directly from the assemblies.
But if you are using another language (Java, Object-c, typescript, javascript, etc) you
can still take adventage of the tool by providing a json file with object definitions.

If you want to populate the json file yourself, and you are developing in .NET, you can download the
following package that will give you the json file structure.

| Library | Nuget | Install
|-|-|-|
| Paradigm.CodeGen.Input.Json.Models | [![NuGet](https://img.shields.io/nuget/v/Nuget.Core.svg)](https://www.nuget.org/packages/Paradigm.CodeGen.Input.Json.Models/) | `Install-Package Paradigm.CodeGen.Input.Json.Models` |


Self Contained Deploy (SCD)
---

Bellow you can find portable versions for all major OSs.
If you are planning to use codegen in several projects, we recommend to add the SCD folder to your PATH.

| OS | Zip File |
|-|-|
| Windows x86 | [Download](https://raw.githubusercontent.com/MiracleDevs/Paradigm.CodeGen/master/dist/codegen.win-x86.zip) |
| Windows x64 | [Download](https://raw.githubusercontent.com/MiracleDevs/Paradigm.CodeGen/master/dist/codegen.win-x64.zip) |
| Linux x64   | [Download](https://raw.githubusercontent.com/MiracleDevs/Paradigm.CodeGen/master/dist/codegen.linux-x64.zip) |
| OSX x64     | [Download](https://raw.githubusercontent.com/MiracleDevs/Paradigm.CodeGen/master/dist/codegen.osx-x64.zip) |

Change log
---

Version `2.0.1`
- Updated Paradigm.Core to version `2.0.1`.

Version `2.0.0`
- Updated .net core from version 1 to version 2.

Version `1.0.0`
- Uploaded first version of the Paradigm CodeGen.
