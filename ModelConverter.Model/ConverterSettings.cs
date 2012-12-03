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

		/// <summary>
		/// factor for scaling models.
		/// 0.5 => 50% half size
		/// 1.0 => 100% original size
		/// 2.0 => 200% double size
		/// </summary>
		public float scaleFactor = 1.0f;

		/// <summary>
		/// Recalcutes Normals of Objects using a very simple Method.
		/// </summary>
		public bool recalculateNormals = false; 
	}
}
