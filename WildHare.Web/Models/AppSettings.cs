﻿
using WildHare.Web.Interfaces;

namespace WildHare.Web.Models
{
    public class AppSettings : IAppSettings
    {
        public string WildHareXmlDocumentationPath { get; set; }
    }
}
