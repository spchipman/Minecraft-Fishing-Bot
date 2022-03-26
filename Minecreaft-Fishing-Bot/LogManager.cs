using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minecreaft_Fishing_Bot
{
    public class LogManager
    {
        public event EventHandler<string> LogUpdatedEvent;

        private readonly StringBuilder _log;
        public string CurrentLog
        {
            get { return _log.ToString(); }
        }

        public LogManager()
        {
            _log = new StringBuilder();
        }

        public void AppendToLog(string text)
        {
            _log.Append(text);
            LogUpdatedEvent.Invoke(this, text);
        }
    }
}
