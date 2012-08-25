using System.Collections.Generic;
using System.IO;
using ModelConverter.Model;

namespace ModelConverter
{
    public class Config
	{
		public bool writeInfo = true;
		public bool writePluginInfo = true;

		public List<string> InputFiles = new List<string>();
		public string Output = string.Empty;

		public double scaleFactor = 1f;

        public string PluginDirectory = Path.GetFullPath(".");
		public List<IPlugin> Plugins = new List<IPlugin>();
	}
}
