using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MocoChan.Converter.PluginInterface
{
	public interface IPlugin
	{
		string Name { get; }
		string Creator { get; }
		string About { get; }
		Version PluginVersion { get; }

		IPluginHost Host { get; set; }


		IImporter Importer { get; }
		IExporter Exporter { get; }
	}
}
