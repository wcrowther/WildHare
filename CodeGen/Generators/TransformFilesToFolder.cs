using CodeGen.Models;
using System.Collections.Generic;
using System.IO;
using WildHare.Extensions;
using static System.Environment;

namespace CodeGen.Generators
{
	public class TransformFilesToFolder(App app)
	{
		TransformFilesSettings settings = app.TransformFilesSettings;
		
		int filesCopied = 0;
		int filesNotCopied = 0;

		public string Init()
        {
            var allFiles = settings
							.FolderFrom
							.GetAllFiles("*.cs");

            var target = new DirectoryInfo(settings.FolderTo);
            target.Create();

            foreach (var file in allFiles)
            {
                CopyToFolder(file, target);
            }

			string message = $"Target folder: {settings.FolderTo}. ";
			message += filesCopied    > 0 ? $"{filesCopied} files copied. " : "";
			message	+= filesNotCopied > 0 ? $"{filesNotCopied} files not copied. Check if they already exist." : "";

			return message.TrimEnd(); 
        }

        private bool CopyToFolder(FileInfo file, DirectoryInfo target)
        {
            var linesFrom = file.ReadFile().ToLineArray();
            var linesTo = new List<string>();

			string entityName = file.Name.RemoveEnd(".cs");
			string modelName  = entityName.AddEnd(settings.ModelSuffix);

			foreach (string line in linesFrom)
            {
				// Example logic for line by line transformations
				
				if (line.TrimStart().StartsWith("namespace"))
				{
					linesTo.Add(line.Replace(settings.NamespaceFrom, settings.NamespaceTo));
				}
				else if (line.TrimStart().StartsWith("public class"))
				{
					linesTo.Add(line.Replace(entityName, entityName.AddEnd(settings.ModelSuffix)));
				}
				else
				{
					linesTo.Add(line.Replace($"<{entityName}>", $"<{modelName}>"));
				}
            }

			string fileName = file.Name
								.RemoveEnd(".cs")
								.AddEnd(settings.ModelSuffix + ".cs");

			string filePath = $"{target.FullName}\\{fileName}";

            var result = linesTo.AsString(NewLine)
                               .WriteToFile(filePath, settings.Overwrite);

			if (result)
				filesCopied++;
			else
				filesNotCopied++;

			return result; 
        }
    }
}