using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MocoChan.Converter.Logging
{
	public class LogMessage
	{
		public LogLevel Level;
		public DateTime Time = DateTime.Now;
		public string Message;

		public static int LevelMaxLength
		{
			get
			{
				int levelLength = 0;
				foreach (String name in Enum.GetNames(typeof(LogLevel)))
				{
					levelLength = Math.Max(levelLength, name.Length);
				}
				return levelLength;
			}
		}

		public LogMessage(string message, LogLevel level)
		{
			this.Message = message;
			this.Level = level;
		}

		public override string ToString()
		{
			return String.Format("{0} {1} | {2}", Time.ToString(), Level.ToString().PadRight(LevelMaxLength), Message);
		}
	}
}