using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MocoChan.Converter;
using MocoChan.Converter.Logging;

namespace MocoChan
{
	class ConsoleLogProvider: ILogProvider
	{
		int LevelLength = 0;

		public ConsoleLogProvider()
		{
			foreach (String name in Enum.GetNames(typeof(LogLevel)))
			{
				LevelLength = Math.Max(LevelLength, name.Length);
			}
		}

		public void Log(LogLevel lvl, string Message)
		{
			Console.WriteLine("{0} │ {1}", lvl.ToString().PadRight(LevelLength), Message);
		}
	}
}
