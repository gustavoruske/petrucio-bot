using Discord.Interactions;
using Discord.WebSocket;

namespace Petrucio.Domain.SlashCommands
{
    public class IniciarSlashCommand : InteractionModuleBase
    {
        [SlashCommand("iniciar", "O bot será inicializado", runMode: RunMode.Async)]
        public async Task IniciarAsync()
        {
            var user = (SocketGuildUser)Context.User;

            if (user is not null)
                Console.WriteLine($"Comando executado por: {user.GlobalName}, ele esta na sala {user.VoiceChannel.Id}");

            await user!.VoiceChannel.ConnectAsync();

            await RespondAsync("Iniciado!");
        }
    }
}