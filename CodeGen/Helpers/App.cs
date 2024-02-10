
namespace CodeGen.Models
{
    public class App 
    {
        public string SourceRoot { get; set; }

        public string WwwRoot { get; set; }

        public string TempPath { get; set; }

        public string WriteToRoot { get; set; }

        public string CssSummaryByFileName { get; set; }

        public string CssStylesheetsFileName { get; set; }

        public string CssClassesFilename { get; set; }

        public bool Overwrite { get; set; }

        public bool ConsoleRemainOpen { get; set; }

        public string LineStart { get; set; }   

        public int ColumnWidth { get; set; }  
    }
}
