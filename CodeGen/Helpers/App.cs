
namespace CodeGen.Models
{
    public class App 
    {
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

		public Adapter Adapter { get; set; }

		public Copy Copy { get; set; }
    }

	public class Adapter
	{
		public string OutputFolder { get; set; }

		public string Namespace { get; set; }

		public string Namespace1 { get; set; }

		public string Namespace2 { get; set; }

		public string MapName1 { get; set; }

		public string MapName2 { get; set; }
	}

	public class Copy
    {
        public string ModelSuffix { get; set; }

        public bool Overwrite { get; set; }
    }
}

