using CodeGen.Models;
using System.Collections.Generic;
using System.IO;
using WildHare.Extensions;
using static System.Environment;

namespace CodeGen.Generators
{
	public class TransformFilesToFolder(AppSettings app)
	{
		int filesCopied = 0;

		public string Init()
        {
            var allFiles = app.EntitiesSourceFolder
							  .GetAllFiles("*.cs");

            var target = new DirectoryInfo(app.ModelsTargetFolder);
            target.Create();

            foreach (var file in allFiles)
            {
                CopyToModelFolder(file, target);
            }

            return $"{filesCopied} files copied to: {app.ModelsTargetFolder}";
        }

        private bool CopyToModelFolder(FileInfo file, DirectoryInfo target)
        {
            var lines = file.ReadFile().ToLineArray();
            var lines2 = new List<string>();

			string entityName = file.Name.RemoveEnd(".cs");
			string modelName = entityName.AddEnd(app.TransformFiles.ModelSuffix);

			foreach (string line in lines)
            {
				// Example logic for line by line transformations
				
				if (line.TrimStart().StartsWith("namespace"))
				{
					lines2.Add(line.Replace(app.TransformFiles.NamespaceFrom, app.TransformFiles.NamespaceTo));
				}
				else if (line.TrimStart().StartsWith("public class"))
				{
					lines2.Add(line.Replace(entityName, entityName.AddEnd(app.TransformFiles.ModelSuffix)));
				}
				else
				{
					lines2.Add(line.Replace($"<{entityName}>", $"<{modelName}>"));
				}
            }

			string fileName = file.Name.RemoveEnd(".cs").AddEnd(app.TransformFiles.ModelSuffix + ".cs");
			string filePath = $"{target.FullName}\\{fileName}";

            var result = lines2.AsString(NewLine)
                               .WriteToFile(filePath, false);

			if (result) filesCopied++;

			return result; 
        }
    }
}