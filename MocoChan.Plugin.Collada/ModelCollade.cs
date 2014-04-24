using MocoChan.Converter.PluginInterface;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace MocoChan.Plugin.Collada
{
	public class ModelCollade : IPlugin
	{
		public string Name { get { return "COLLADA"; } }
		public string Creator { get { return "Sven Tatter"; } }
		public string About { get { return "Support for COLLADA Exchange Format"; } }
		public Version PluginVersion { get { return Assembly.GetExecutingAssembly().GetName().Version; } }

		public IPluginHost Host { get; set; }

		public Dictionary<string, string> fileExtensions
		{
			get
			{
				Dictionary<string, string> extensionsDic = new Dictionary<string, string>();
				extensionsDic.Add("dae", "COLLADA");
				return extensionsDic;
			}
		}

		public bool canRead { get { return false; } }
		public bool canWrite { get { return false; } }

		public IImporter Importer { get { throw new NotImplementedException(); } }
		public IExporter Exporter { get { return new ColladaWriter(); } }
	}
}
