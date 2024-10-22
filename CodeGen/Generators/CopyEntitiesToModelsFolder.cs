using AngleSharp.Common;
using CodeGen.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Xml;
using WildHare.Extensions;
using WildHare.Extensions.ForTemplating;
using WildHare.Web;
using static System.Environment;

namespace CodeGen.Generators
{
    public class CopyEntitiesToModelsFolder
    {
        private App _app;

        public CopyEntitiesToModelsFolder(App app)
        {
            _app            = app;
        }

        public string Init()
        {
            string writeToFilePath = $@"{_app.WriteToRoot}{_app.PartialsSummaryFileName}";

            var allFiles = _app.EntitiesSourceFolder
                           .GetAllFiles("*.cs");

            var target = new DirectoryInfo(_app.ModelsTargetFolder);
            target.Create();

            if (!Directory.Exists(_app.ModelsTargetFolder))
            {
                Directory.CreateDirectory(_app.ModelsTargetFolder);
            }

            foreach (var file in allFiles)
            {
                CopyToModelsFolder(file, target);
            }

            string result = "";

            return result;
        }

        private void CopyToModelsFolder(FileInfo file, DirectoryInfo target)
        {
            var lines = file.ReadFile().ToLineArray();
            var lines2 = new List<string>();

            foreach (string line in lines)
            {
                if (line.TrimStart().StartsWith("namespace"))
                {
                    lines2.Add(line.Replace("WildHare.Web.Entities", "CodeGen.Models"));
                }
                else if (line.TrimStart().StartsWith("public class"))
                {
                    string entityName = file.Name.RemoveEnd(".cs");
                    lines2.Add(line.Replace(entityName, entityName.AddEnd(_app.Copy.ModelSuffix)));
                }
                else
                    lines2.Add(line);
            }

            string filePath = $"{target.FullName}\\{file.Name.RemoveEnd(".cs").AddEnd(_app.Copy.ModelSuffix + ".cs")}";

            var result = lines2.AsString(NewLine)
                               .WriteToFile(filePath, false);
        }
    }
}