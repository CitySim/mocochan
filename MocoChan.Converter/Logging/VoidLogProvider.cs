using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MocoChan.Converter.Logging
{
	/// <summary>
	/// a log dummy provider doing nothing with log messages
	/// </summary>
	internal class VoidLogProvider : ILogProvider
	{
		public void Log(LogLevel lvl, string Message)
		{
		}
	}
}
