using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelConverter.Model
{
	public class ConverterSettings
	{
		/// <summary>
		/// if empty exported Files are saved in same Folder as imported one
		/// if not empty all exported file are saved in this folder (ignores subfolder strucure) 
		/// </summary>
		public string outputDir;

		/// <summary>
		/// file extension of format to be exported to
		/// </summary>
		public string exportType;
	}
}
