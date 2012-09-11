using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using ModelConverter.Model;

namespace ModelConverter.WinForms.Options
{
	public class ExtensionConverter : StringConverter
	{
		public static Converter converter;

		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		//StandardValuesCollection

		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			return new StandardValuesCollection(converter.extensions.Keys);
		}
	}
}
