namespace WildHare.Web.Interfaces
{
    public interface IAppSettings
    {
        string WildHareXmlDocumentationPath { get; set; }

        string ClassTagListSourceFolderRootPath { get; set; }

        string ClassTagListWriteToFilePath { get; set; }

        bool ClassTagList_Overwrite { get; set; }
    }
}