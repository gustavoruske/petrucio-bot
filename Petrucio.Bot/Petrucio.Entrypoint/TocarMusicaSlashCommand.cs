using Discord.Interactions;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petrucio.Entrypoint
{
    public class TocarMusicaSlashCommand : InteractionModuleBase
    {
        public TocarMusicaSlashCommand()
        {
        }

        [SlashCommand("tocar-musica", "Vai tocar uma musica aleatoria")]
        public async Task TocarMusicaAsync()
        {
            var voiceChannel = Context.Client.CurrentUser;

            await RespondAsync("...Uma musica foi tocada!");
        }
    }
}
