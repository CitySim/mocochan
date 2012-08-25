using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelConverter.Model
{
    public interface IPlugin
    {
        string Name { get; }
        Dictionary<string, string> fileExtensions { get; }
        Version PluginVersion { get; }
        string Creator { get; }
        string About { get; }

        bool canRead { get; }
        bool canWrite { get; }

        BaseModel Read(string filePath, out List<LogMessage> Log);
        void Write(string filePath, BaseModel model, out List<LogMessage> Log);
    }
}
