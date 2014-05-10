using MocoChan.Converter.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MocoChan.Converter.PluginInterface
{
	public interface IImporter : IPlugin
	{
		Dictionary<String, String> Formats { get; }
		Type ImporterSettingsType { get; }
		Asset Read(Stream input);
	}
}
