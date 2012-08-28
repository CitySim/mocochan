using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelConverter.Model
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
