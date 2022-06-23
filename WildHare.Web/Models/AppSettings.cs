
using WildHare.Web.Interfaces;

namespace WildHare.Web.Models
{
    public class AppSettings : IAppSettings
    {
        public string WildHareXmlDocumentationPath { get; set; }    

        public string ClassTagListSourceFolderRootPath { get; set; }

        public string ClassTagListWriteToFilePath { get; set; }

        public bool ClassTagList_Overwrite { get; set; }
    }
}
