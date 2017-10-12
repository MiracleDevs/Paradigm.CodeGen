@{

	var model = Model as Paradigm.CodeGen.Output.Razor.SummaryModel;
	var entities = model.GenerationItems.Where(x => x.Configuration.Name == "DomainEntity").Select(x => x.Model.Definition).ToList();

	var mappers = model.GenerationItems.Where(x => x.Configuration.Name == "DatabaseReaderMapper").Select(x => x.Model.Definition).ToList();
	var storedProcedures = model.GenerationItems.Where(x => x.Configuration.Name == "StoredProcedure").Select(x => x.Model.Definition).ToList();
	var databaseAccessors = model.GenerationItems.Where(x => x.Configuration.Name == "DatabaseAccess").Select(x => x.Model.Definition).ToList();
	var readRepositories = model.GenerationItems.Where(x => x.Configuration.Name == "ReadRepository").Select(x => x.Model.Definition).ToList();
	var editRepositories = model.GenerationItems.Where(x => x.Configuration.Name == "EditRepository").Select(x => x.Model.Definition).ToList();

}//////////////////////////////////////////////////////////////////////////////////
//  OutputSummary.txt
//
//  Generated with the Paradigm.CodeGen tool.
//  Do not modify this file in any way.
//
//  Copyright (c) 2016 miracledevs. All rights reserved.
//////////////////////////////////////////////////////////////////////////////////

//////////////////////////////////////////////////////////////////////////////////
// Mappings
//////////////////////////////////////////////////////////////////////////////////
@foreach(var entity in entities)
{
<text>@(Raw(entity.Name)).RegisterMapping(mapper);
</text>
}

//////////////////////////////////////////////////////////////////////////////////
// Dependency Injection - Mappers
//////////////////////////////////////////////////////////////////////////////////
@foreach(var entity in mappers)
{
<text>builder.Register<@(Raw($"I{entity.Name}DatabaseReaderMapper")), @(Raw($"{entity.Name}DatabaseReaderMapper"))>();
builder.Register<@(Raw($"IDatabaseReaderMapper<{entity.Name}>")), @(Raw($"{entity.Name}DatabaseReaderMapper"))>();
</text>
}

//////////////////////////////////////////////////////////////////////////////////
// Dependency Injection - Stored Procedures
//////////////////////////////////////////////////////////////////////////////////
@foreach(var entity in storedProcedures)
{
<text>builder.Register<@(Raw($"I{entity.Name}StoredProcedure")), @(Raw($"{entity.Name}StoredProcedure"))>();
</text>
}

//////////////////////////////////////////////////////////////////////////////////
// Dependency Injection - Database Accessors
//////////////////////////////////////////////////////////////////////////////////
@foreach(var entity in databaseAccessors)
{
<text>builder.Register<@(Raw($"I{entity.Name}DatabaseAccess")), @(Raw($"{entity.Name}DatabaseAccess"))>();
builder.Register<@(Raw($"IDatabaseAccess<{entity.Name}>")), @(Raw($"{entity.Name}DatabaseAccess"))>();
</text>
}

//////////////////////////////////////////////////////////////////////////////////
// Dependency Injection - Read Repositories
//////////////////////////////////////////////////////////////////////////////////
@foreach(var entity in readRepositories)
{
<text>builder.Register<@(Raw($"I{entity.Name}Repository")), @(Raw($"{entity.Name}Repository"))>();
</text>
}

//////////////////////////////////////////////////////////////////////////////////
// Dependency Injection - Edit Repositories
//////////////////////////////////////////////////////////////////////////////////
@foreach(var entity in editRepositories)
{
<text>builder.Register<@(Raw($"I{entity.Name}Repository")), @(Raw($"{entity.Name}Repository"))>();
</text>
}