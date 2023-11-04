using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Caching.Memory;

namespace Petrucio.Domain.Services
{
    public class UserVoiceStateUpdatedService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly DiscordSocketClient _client;

        public UserVoiceStateUpdatedService(IMemoryCache memoryCache, DiscordSocketClient client)
        {
            _memoryCache = memoryCache;
            _client = client;
        }

        public async Task Execute(SocketUser user, SocketVoiceState from, SocketVoiceState to)
        {
            await BotConnectedVoiceChannelAsync(user, from, to);
        }

        private async Task BotConnectedVoiceChannelAsync(SocketUser user, SocketVoiceState from, SocketVoiceState to)
        {
            if (user.IsBot && user.Id == _client.CurrentUser.Id)
            {
                if (to.VoiceChannel == null)
                {
                    _memoryCache.Remove("BotVoiceChannelId");
                    _memoryCache.Remove("BotInkovedChannel");
                    return;
                }

                _memoryCache.Set("BotVoiceChannelId", to.VoiceChannel.Id);
                var channel = await _client.GetChannelAsync((ulong)_memoryCache.Get("BotInkovedChannel")) as IMessageChannel;

                await channel!.SendMessageAsync("Conectado e operante!");
            }
        }
    }
}
