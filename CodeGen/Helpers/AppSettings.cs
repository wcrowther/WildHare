
namespace CodeGen.Models;

public class AppSettings 
{
	public string ProjectRoot { get; set; }

	public bool ClearConsole { get; set; }

	public string SourceRoot { get; set; }

	public string WwwRoot { get; set; }
	
	public string TempPath { get; set; }
	
	public string WriteToRoot { get; set; }
	
	public string PartialsSummaryFileName { get; set; }
	
	public string CssStylesheetsFileName { get; set; }
	
	public string CssClassesFilename { get; set; }
	
	public bool Overwrite { get; set; }
	
	public bool ConsoleRemainOpen { get; set; }
	
	public string LineStart { get; set; }   
	
	public int ColumnWidth { get; set; }

	public string EntitiesSourceFolder { get; set; }

    public string ModelsTargetFolder { get; set; }

	public Adapters Adapters { get; set; }

	public Validators Validators { get; set; }

	public TransformFiles TransformFiles { get; set; }
}

public class Adapters
{
	public string OutputFolder { get; set; }

	public string AdapterNamespace { get; set; }

	public string MapNamespace1 { get; set; }

	public string MapNamespace2 { get; set; }

	public string MapName1 { get; set; }

	public string MapName2 { get; set; }

	public string AdapterListOutputFile { get; set; }
}

public class Validators
{
	public string SourceNamespace { get; set; }

	public string SourceTypeInNamespace { get; set; }

	public string SourceAssemblyName { get; set; }

	public string ExcludeClasses { get; set; }

	public string OutputFile { get; set; }
}
	

public class TransformFiles
    {
        public string ModelSuffix { get; set; }

        public bool Overwrite { get; set; }

	public string NamespaceFrom { get; set; }

	public string NamespaceTo { get; set; } 
}

