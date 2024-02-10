namespace Me2.Models
{
    public class AppSettings
    {
        public bool CodeGenOverwrite { get; set; }

		public string CodeGenTempPath { get; set; }

		public string CssListOfClasses_Filename { get; set; }

		public string CssListOfStylesheets_Filename { get; set; }

		public string CssSummaryByFileName_Filename { get; set; }

		public string CssWriteToFolderPath { get; set; }

		public string MESourceFolderRootPath { get; set; }

		public bool RemainOpenAfterCodeGen { get; set; }

		public string SourceFolderRootPath { get; set; }

		public string WwwFolderRootPath { get; set; }
    }
}