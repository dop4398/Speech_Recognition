using System;
using Microsoft.Speech.Recognition;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;
using Discord.Commands;

namespace Listener_Bot
{
    class Program
    {
        private DiscordSocketClient Client;
        private CommandService Commands;

        static void Main(string[] args)
        => new Program().MainAsync().GetAwaiter().GetResult();


        private async Task MainAsync()
        {
            // Configure the client
            Client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Debug
            });

            // Configure the command service
            Commands = new CommandService(new CommandServiceConfig
            {
                CaseSensitiveCommands = true,
                DefaultRunMode = RunMode.Async,
                LogLevel = LogSeverity.Debug
            });

            // Configure the methods
            Client.MessageReceived += Client_MessageReceived;
            await Commands.AddModulesAsync(Assembly.GetEntryAssembly(), null);

            Client.Ready += Client_Ready;
            Client.Log += Client_Log;

            string token = "NTYxMzAzNjMyODQ3NTY4OTM3.XJ6RcQ.LGys9F0l9itKnl4LuvgSO8e4Jcc";
            await Client.LoginAsync(TokenType.Bot, token);
            await Client.StartAsync();
            await Task.Delay(-1);
        }

        private async Task Client_Log(LogMessage message)
        {
            Console.WriteLine($"[{DateTime.Now} at {message.Source}] {message.Message}");
        }

        private async Task Client_Ready()
        {
            await Client.SetGameAsync("Listening");
        }

        private async Task Client_MessageReceived(SocketMessage arg)
        {
            // Configure the commands here
        }
    }
}