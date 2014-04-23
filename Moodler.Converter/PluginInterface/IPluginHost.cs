using Moodler.Converter.Data;
using Moodler.Converter.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Moodler.Converter.PluginInterface
{
	public interface IPluginHost
	{
		ILogProvider LogProvider { get; set; }

		void loadPlugins (string loadDir);

		Model Convert(String input, String output);
		Model Convert(String input, String output, ConvertSettings settings);
		Model Convert(Stream input, Stream output, ConvertSettings settings);
	}
}
