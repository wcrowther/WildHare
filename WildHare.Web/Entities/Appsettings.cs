
namespace Me2.Models
{
    public class AppSettings
    {
        public string WildHareXmlDocumentationPath { get; set; }

		public string SourceFolderRootPath { get; set; }

		public string MESourceFolderRootPath { get; set; }

		public string WwwFolderRootPath { get; set; }

		public string CssWriteToFolderPath { get; set; }

		public string CssSummaryByFileName_Filename { get; set; }

		public string CssListOfStylesheets_Filename { get; set; }

		public string CssListOfClasses_Filename { get; set; }

		public string CodeGenOverwrite { get; set; }

		public string RemainOpenAfterCodeGen { get; set; }
    }
}
