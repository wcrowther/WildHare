#:project CodeGen/CodeGen.csproj

using CodeGen.Generators;
using CodeGen.Models;

Console.WriteLine(">>>> Loaded CodeGen project.");

var adaptersSettings = new AdaptersSettings
{
    ProjectRoot 			= @"C:\Git\WildHare\CodeGen\Examples\Adapters\",
	OutputFolder            = @"CodeGen\Examples\Adapters\",
	AdapterNamespace        = @"CodeGen.Adapters",
	MapNamespace1           = @"CodeGen.Entities",
	MapNamespace2           = @"CodeGen.Models",
	MapName1                = @"entity",
	MapName2                = @"model",
	AdapterListOutputFile   = @"CodeGenAdapters_RunAdaptersList.cs",
	AdapterSuffix			= @"Model"
};

var gen = CodeGenAdaptersList.Generate(adaptersSettings);

Console.WriteLine(gen);
