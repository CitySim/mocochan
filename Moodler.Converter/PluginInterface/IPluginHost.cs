using Moodler.Converter.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moodler.Converter.PluginInterface
{
	public interface IPluginHost
	{
		ILogProvider logProvider { get; set; }
	}
}
