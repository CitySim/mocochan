using MocoChan.Converter.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MocoChan.Converter.PluginInterface
{
	public interface IExporter : IPlugin
	{
		Dictionary<String, String> Formats { get; }
		Type ExporterSettingsType { get; }
		void Write(Asset asset, Stream output);
	}
}
