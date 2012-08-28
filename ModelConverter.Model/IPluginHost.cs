using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelConverter.Model
{
	public interface IPluginHost
	{
		ILogProvider logProvider { get; set; }
	}
}
