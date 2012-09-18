using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
			this.converterSettings = converter.settings;

			ExtensionConverter.converter = this.converter;
		}

		public void loadSettings()
		{
			// try reading settings
			try
			{
				outputDirectory = Properties.Settings.Default.lastOutputDir;
			}
			catch { }
			try
			{
				exportType = Properties.Settings.Default.lastExportType;
			}
			catch { }
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
			get
			{
				if (string.IsNullOrEmpty(converterSettings.exportType))
				{
					return converterSettings.exportType;
				}
				else
				{
					return converterSettings.exportType + " - " + converter.extensions[converterSettings.exportType].Name;
				}
			}
			set
			{
				if (!converter.extensions.ContainsKey(value))
				{
					throw new KeyNotFoundException("Found no Plugin for this Extension");
				}

				converterSettings.exportType = value;
			}
		}

		[DefaultValue(1.0f)]
		public float scaleFactor
		{
			get { return converterSettings.scaleFactor; }
			set
			{
				converterSettings.scaleFactor = value;
			}
		}
	}
}