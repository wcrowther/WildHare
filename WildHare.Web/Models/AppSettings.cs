
using WildHare.Web.Interfaces;

namespace WildHare.Web.Models
{
    public class AppSettings : IAppSettings
    {
        public string WildHareXmlDocumentationPath { get; set; }

        public string SourceFolderRootPath { get; set; }

        public string MESourceFolderRootPath { get; set; }

        public string WwwFolderRootPath { get; set; }

        public string CssWriteToFolderPath { get; set; }

        public string CssSummaryByFileName_Filename { get; set; }

        public string CssListOfStylesheets_Filename { get; set; }

        public string CssListOfClasses_Filename { get; set; }

        public bool CodeGenOverwrite { get; set; }

        public bool RemainOpenAfterCodeGen { get; set; }
    }
}
