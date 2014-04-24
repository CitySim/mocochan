using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using MocoChan.Converter;

namespace MocoChan.WinForms.Options
{
	public class ConverterOptions
	{
		public MocoConverter Converter;
		public MocoSettings ConverterSettings;

		public ConverterOptions(MocoConverter converter)
		{
			this.Converter = converter;
			this.ConverterSettings = converter.Settings;

			ExtensionConverter.converter = this.Converter;
		}

		public void LoadSettings()
		{
			// try reading settings
			//try
			//{
			//	OutputDirectory = Properties.Settings.Default.lastOutputDir;
			//}
			//catch { }
			//try
			//{
			//	ExportType = Properties.Settings.Default.lastExportType;
			//}
			//catch { }
		}

		//[EditorAttribute(typeof(DirectoryEditor), typeof(System.Drawing.Design.UITypeEditor))]
		//public string OutputDirectory
		//{
		//	get { return ConverterSettings.outputDir; }
		//	set
		//	{
		//		if (!Directory.Exists(value))
		//		{
		//			throw new DirectoryNotFoundException("Export Directory not found");
		//		}

		//		ConverterSettings.outputDir = value;
		//	}
		//}

		//[TypeConverter(typeof(ExtensionConverter))]
		//public string ExportType
		//{
		//	get
		//	{
		//		if (string.IsNullOrEmpty(ConverterSettings.exportType))
		//		{
		//			return ConverterSettings.exportType;
		//		}
		//		else
		//		{
		//			return ConverterSettings.exportType + " - " + Converter.extensions[ConverterSettings.exportType].Name;
		//		}
		//	}
		//	set
		//	{
		//		if (!Converter.extensions.ContainsKey(value))
		//		{
		//			throw new KeyNotFoundException("Found no Plugin for this Extension");
		//		}

		//		ConverterSettings.exportType = value;
		//	}
		//}

		//[DefaultValue(1.0f)]
		//public float ScaleFactor
		//{
		//	get { return ConverterSettings.scaleFactor; }
		//	set
		//	{
		//		ConverterSettings.scaleFactor = value;
		//	}
		//}

		//[DefaultValue(false)]
		//public bool RecalculateNormals
		//{
		//	get { return ConverterSettings.recalculateNormals; }
		//	set
		//	{
		//		ConverterSettings.recalculateNormals = value;
		//	}
		//}
	}
}