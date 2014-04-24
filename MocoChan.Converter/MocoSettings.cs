using MocoChan.Converter.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MocoChan.Converter
{
	public class MocoSettings : IEnumerable
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
