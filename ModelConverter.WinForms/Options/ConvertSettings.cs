using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using ModelConverter.Model;

namespace ModelConverter.WinForms.Options
{
	public class ConvertSettings
	{
		public Converter converter;
		public ConverterSettings converterSettings;

		public ConvertSettings(Converter converter)
		{
			this.converter = converter;
			this.converterSettings = this.converter.settings;

			ExtensionConverter.converter = this.converter;
		}

		[EditorAttribute(typeof(DirectoryEditor),typeof(System.Drawing.Design.UITypeEditor))]
		public string outputDirectory
		{
			get { return converterSettings.outputDir; }
			set
			{
				if (!Directory.Exists(value))
				{
					throw new DirectoryNotFoundException("Export Directory not found");
				}

				converterSettings.outputDir = value;
			}
		}

		[TypeConverter(typeof(ExtensionConverter))]
		public string exportType
		{
			get { return converterSettings.exportType; }
			set
			{
				if (!converter.extensions.ContainsKey(value))
				{
					throw new KeyNotFoundException("Found no Plugin for this Extension");
				}

				converterSettings.exportType = value;
			}
		}
	}
}