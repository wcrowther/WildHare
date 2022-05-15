
namespace WildHare.Web.Models
{
    public class AppSettings 
    {
        public string WildHareXmlDocumentationPath { get; set; }

        public string SeedPacketAnalyticsRootPath { get; set; }

        public string AnalyticsWriteToFile { get; set; }

        // FUNCTIONS ================================================================================================

        public string AnalyticsWriteToPath(string contentRootPath) => $"{contentRootPath}{AnalyticsWriteToFile}";
    }
}
