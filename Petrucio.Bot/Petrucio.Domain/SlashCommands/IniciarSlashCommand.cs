using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Caching.Memory;

namespace Petrucio.Domain.SlashCommands
{
    public class IniciarSlashCommand : InteractionModuleBase
    {
        private readonly IMemoryCache _memoryCache;

        public IniciarSlashCommand(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [SlashCommand("iniciar", "O bot será inicializado", runMode: RunMode.Async)]
        public async Task IniciarAsync()
        {
            var user = (SocketGuildUser)Context.User;

            if (user is not null)
                Console.WriteLine($"Comando executado por: {user.GlobalName}, ele esta na sala {user.VoiceChannel.Id}");

            _memoryCache.Set("BotInkovedChannel", Context.Channel.Id);

            await user!.VoiceChannel.ConnectAsync();
            await RespondAsync("Iniciado!");
        }
    }
}