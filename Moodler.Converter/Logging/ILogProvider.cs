using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moodler.Converter.Logging
{
	public enum LogLevel
	{
		Info,
		Warning,
		Error,
		Fatal
	}

	public interface ILogProvider
	{
		void Log(LogLevel lvl, string Message);
	}
}
