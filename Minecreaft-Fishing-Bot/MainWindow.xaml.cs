using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Extensions.Configuration;
using NHotkey;
using NHotkey.Wpf;

namespace Minecreaft_Fishing_Bot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IConfiguration Configuration;
        private readonly LogManager LogManager;
        private readonly FishingBot FishingBot;

        private Task BotTask { get; set; }

        private CancellationTokenSource BotCancellationTokenSource { get; set; }

        public MainWindow(IConfiguration configuration, LogManager logManager, FishingBot fishingBot)
        {
            this.Configuration = configuration;
            this.LogManager = logManager;

            InitializeComponent();

            RegisterHotkeys();
        }



        private void StartBot()
        {
            if (BotTask != null && BotTask.Status == TaskStatus.Running) return;

            StopBot(); //Attempt to stop any bot activity before starting is again.
            
            BotCancellationTokenSource = new CancellationTokenSource();
            BotTask = FishingBot.RunBot(BotCancellationTokenSource.Token);
        }

        private void StopBot()
        {
            if (BotTask != null)
            {
                BotCancellationTokenSource.Cancel();

                if (Task.WaitAny(BotTask, Task.Delay(4000)) == 1)
                {
                    //BotTask took longer that 4 seconds to stop and may be hung.
                    //Display message.
                    return;
                }
                if (BotTask.Status == TaskStatus.RanToCompletion) BotTask = null;
                BotCancellationTokenSource.Dispose();
            }
        }

        private void RegisterHotkeys()
        {
            var hotkeyConfig = Configuration.GetSection("hotkeys");
            Key startBotKey = (Key)Enum.Parse(typeof(Key), hotkeyConfig["startBotKey"]);
            Key stopBotKey = (Key)Enum.Parse(typeof(Key), hotkeyConfig["stopBotKey"]);
            ModifierKeys startBotModifierKey = (ModifierKeys)Enum.Parse(typeof(ModifierKeys), hotkeyConfig["startBotModifierKey"]);
            ModifierKeys stopBotModifierKey = (ModifierKeys)Enum.Parse(typeof(ModifierKeys), hotkeyConfig["stopBotModifierKey"]);

            HotkeyManager.Current.AddOrReplace("StartBot", startBotKey, startBotModifierKey, (object sender, HotkeyEventArgs e) => { e.Handled = true; StartBot(); });
            HotkeyManager.Current.AddOrReplace("StopBot", stopBotKey, stopBotModifierKey, (object sender, HotkeyEventArgs e) => { e.Handled = true; StopBot(); });
        }
    }
}
