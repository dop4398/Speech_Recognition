using System;
using Microsoft.Speech.Recognition;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;
using Discord.Audio;
using Discord.Commands;

// Where I left off: https://www.youtube.com/watch?v=QGYCgAFFvL0

namespace Listener_Bot
{
    class Program
    {
        private DiscordSocketClient _client;
        private CommandService _commands;

        static void Main(string[] args)
        => new Program().MainAsync().GetAwaiter().GetResult();


        private async Task MainAsync()
        {
            // Configure the client
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Debug
            });

            // Configure the command service
            _commands = new CommandService(new CommandServiceConfig
            {
                CaseSensitiveCommands = true,
                DefaultRunMode = RunMode.Async,
                LogLevel = LogSeverity.Debug
            });

            // Configure the methods
            _client.MessageReceived += Client_MessageReceived;
            _client.MessageReceived += Client_MessageHeard;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), services: null);

            _client.Ready += Client_Ready;
            _client.Log += Client_Log;

            string token = "NTYxMzAzNjMyODQ3NTY4OTM3.XJ6RcQ.LGys9F0l9itKnl4LuvgSO8e4Jcc";
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();
            await Task.Delay(-1);
        }


        private async Task Client_Log(LogMessage message)
        {
            Console.WriteLine($"[{DateTime.Now} at {message.Source}] {message.Message}");
        }


        private async Task Client_Ready()
        {
            await _client.SetGameAsync("Listening");
        }


        private async Task Client_MessageReceived(SocketMessage messageParam)
        {
            var message = messageParam as SocketUserMessage;         

            if(message == null)
            {
                return;
            }

            int argPos = 0;
            if (!(message.HasStringPrefix("!a ", ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos)))
            {
                return;
            }

            var context = new SocketCommandContext(_client, message);

            var result = await _commands.ExecuteAsync(context, argPos, services: null);
            if(!result.IsSuccess)
            {
                Console.WriteLine($"[{DateTime.Now} at Commands] Something went wrong with executing a command. Text: {context.Message.Content} | Error: {result.ErrorReason}");
            }
        }

        private async Task Client_MessageHeard(SocketMessage messageParam)
        {

        }
    }
}