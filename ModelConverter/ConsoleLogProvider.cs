using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModelConverter.Model;

namespace ModelConverter
{
	class ConsoleLogProvider: ILogProvider
	{
		public void Log(LogLevel lvl, string Message)
		{
			// TODO: extend function
			Console.WriteLine(Message);
		}
	}
}
