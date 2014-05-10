using MocoChan.Converter.Data;
using MocoChan.Converter.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MocoChan.Converter.PluginInterface
{
	public interface IPluginHost
	{
		ILogProvider LogProvider { get; set; }

		void LoadPlugins(string loadDir);

		Asset Convert(String input, String output);
		Asset Convert(String input, String output, ConvertSettings settings);
		Asset Convert(Stream input, Stream output, ConvertSettings settings);
	}
}
