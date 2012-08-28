using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelConverter.Model
{
    public class LogMessage
    {
        public string Message;
        public LogLevel Level;

        public LogMessage(string Message, LogLevel Level)
        {
            this.Message = Message;
            this.Level = Level;
        }


        public override string ToString()
        {
            switch (Level)
            {
                case LogLevel.Info:
                    return "INFO   : " + Message;
                case LogLevel.Warning:
                    return "WARNING: " + Message;
                case LogLevel.Error:
                    return "ERROR  : " + Message;
            }
            return "UNKOWN : " + Message;
        }
    }
}