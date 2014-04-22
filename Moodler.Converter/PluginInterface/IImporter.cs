﻿using Moodler.Converter.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Moodler.Converter.PluginInterface
{
	public interface IImporter
	{
		Model Read(Stream input);
	}
}
