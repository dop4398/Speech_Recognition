using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.Audio;
using Discord.Commands;

namespace Listener_Bot.Core.Commands
{
    public class JoinCall : ModuleBase<SocketCommandContext>
    {
        [Command("join", RunMode = RunMode.Async)]
        [Alias("join call")]
        [Summary("Adds the bot to the user's call")]
        public async Task JoinCmd()
        {
            //Context.Channel.JoinAudio();
        }
    }
}
