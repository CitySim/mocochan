using System.Collections.Generic;
using System.IO;
using MocoChan.Converter;

namespace MocoChan
{
    public class Config
	{
		public bool WriteInfo = true;
		public bool WriteHelp;

		public List<string> InputFiles = new List<string>();
		
		public double ScaleFactor = 1f;
		public bool Normals = false;

        public string PluginDirectory = Path.GetFullPath(".");

		// get copied to Converter Settings:
		public string OutputDir = string.Empty;
		public string Exporter = string.Empty;
	}
}
