using System.Collections.Generic;
using System.IO;
using ModelConverter.Model;

namespace ModelConverter
{
    public class Config
	{
		public bool writeInfo = true;
		public bool writeHelp;

		public List<string> InputFiles = new List<string>();

		public double scaleFactor = 1f;

        public string PluginDirectory = Path.GetFullPath(".");

		// get copied to Converter Settings:
		public string outputDir = string.Empty;
		public string exportType = string.Empty;
	}
}
