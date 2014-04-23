using Moodler.Converter.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moodler.Converter
{
	public class ConverterSettings : IEnumerable
	{
		/// <summary>
		/// Default Log Provider to use
		/// </summary>
		public ILogProvider LogProvider = new VoidLogProvider();

		public IEnumerator GetEnumerator()
		{
			throw new NotImplementedException();
		}
	}
}
