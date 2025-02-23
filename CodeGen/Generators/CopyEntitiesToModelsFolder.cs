using CodeGen.Models;
using System.Collections.Generic;
using System.IO;
using WildHare.Extensions;
using static System.Environment;

namespace CodeGen.Generators
{
	public class CopyEntitiesToModelsFolder(App app)
	{
		public string Init()
        {
            var allFiles = app.EntitiesSourceFolder
                           .GetAllFiles("*.cs");

            var target = new DirectoryInfo(app.ModelsTargetFolder);
            target.Create();

            if (!Directory.Exists(app.ModelsTargetFolder))
            {
                Directory.CreateDirectory(app.ModelsTargetFolder);
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
				string entityName = file.Name.RemoveEnd(".cs");
				string modelName  = entityName.AddEnd(app.Copy.ModelSuffix);

				if (line.TrimStart().StartsWith("namespace"))
                {
                    lines2.Add(line.Replace("WildHare.Web.Entities", "CodeGen.Models"));
                }
                else if (line.TrimStart().StartsWith("public class"))
                {
                    lines2.Add(line.Replace(entityName, entityName.AddEnd(app.Copy.ModelSuffix)));
                }
                else
                    lines2.Add(line.Replace($"<{entityName}>", $"<{modelName}>"));
            }

            string filePath = $"{target.FullName}\\{file.Name.RemoveEnd(".cs").AddEnd(app.Copy.ModelSuffix + ".cs")}";

            var result = lines2.AsString(NewLine)
                               .WriteToFile(filePath, false);
        }
    }
}