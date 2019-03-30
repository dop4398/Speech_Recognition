using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;

namespace Listener_Bot.Core.Commands
{
    class Hello : ModuleBase<SocketCommandContext>
    {
        [Command("hello there")]
        [Alias("Hello there!")]
        [Summary("Hello command")]

        public async Task HelloThere()
        {
            await Context.Channel.SendMessageAsync("General Kenobi! You are a bold one...");
        }
    }
}
