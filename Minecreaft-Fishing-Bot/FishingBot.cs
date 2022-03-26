using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Minecreaft_Fishing_Bot
{
    public class FishingBot
    {
        private readonly LogManager LogManager;
        public FishingBot(LogManager logManager)
        {
            LogManager = logManager;
        }

        public async Task RunBot(CancellationToken token)
        {
            while (true)
            {
                if (token.IsCancellationRequested)
                {
                    LogManager.AppendToLog("Bot is being stopped.");
                    break;
                }
                await Task.Delay(100, token);
            }
            return;
        }
    }
}
