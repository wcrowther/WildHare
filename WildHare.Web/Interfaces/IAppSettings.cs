namespace WildHare.Web.Interfaces
{
    public interface IAppSettings
    {
        public string WildHareXmlDocumentationPath { get; set; }

        public string SourceFolderRootPath { get; set; }

        public string WwwFolderRootPath { get; set; }

        public string CodeGenTempPath { get; set; }

        public string CssWriteToFolderPath { get; set; }

        public string CssSummaryByFileName_Filename { get; set; }

        public string CssListOfStylesheets_Filename { get; set; }

        public string CssListOfClasses_Filename { get; set; }

        public bool CodeGenOverwrite { get; set; }
    }
}