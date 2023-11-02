using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petrucio.Entrypoint
{
    public class RoletaRussaSlashCommand : InteractionModuleBase
    {
        [SlashCommand("roleta", "teste", runMode: Discord.Interactions.RunMode.Async)]
        public async Task ExecuteAsync()
        {
            var user = (SocketGuildUser)Context.User;

            if (user is not null)
                Console.WriteLine($"Comando executado por: {user.GlobalName}, ele esta na sala {user.VoiceChannel.Id}");

            await user!.VoiceChannel.ConnectAsync();

            await RespondAsync("Iniciado!");
        }
    }
}
