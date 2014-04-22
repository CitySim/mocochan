﻿using Moodler.Converter.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moodler.Converter
{
	public class ConverterSettings : IEnumerable
	{
		public ConverterSettings()
		{
			LogProvider = new VoidLogProvider();
		}

		/// <summary>
		/// Default Log Provider to use
		/// </summary>
		public ILogProvider LogProvider;

		public IEnumerator GetEnumerator()
		{
			throw new NotImplementedException();
		}
	}
}
