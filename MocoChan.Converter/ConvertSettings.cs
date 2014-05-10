using MocoChan.Converter.PluginInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace MocoChan.Converter
{
	public class ConvertSettings : IEnumerable
	{
		public double ScaleFactor = 1.0f;
		public bool RecalculateNormals = false;

		public IImporter Importer;
		public IExporter Exporter;

		public IEnumerator GetEnumerator()
		{
			throw new NotImplementedException();
		}
	}
}
