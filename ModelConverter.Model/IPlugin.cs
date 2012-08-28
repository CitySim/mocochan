using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelConverter.Model
{
    public interface IPlugin
    {
		string Name { get; }
		string Creator { get; }
		string About { get; }
		Version PluginVersion { get; }

		IPluginHost host { get; set; }

		Dictionary<string, string> fileExtensions { get; }
		
        bool canRead { get; }
        bool canWrite { get; }

        BaseModel Read(string filePath);
        void Write(string filePath, BaseModel model);
    }
}
