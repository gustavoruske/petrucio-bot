using Discord.WebSocket;
using Discord;
using Microsoft.Extensions.DependencyInjection;
using Petrucio.Domain.Services;
using Petrucio.Domain.SlashCommands;

namespace Petrucio.Domain.Extensions
{
    public static class DependecyInjectionExtension
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<EntrypointService>();
            services.AddScoped<UserVoiceStateUpdatedService>();
            services.ConfigClient();

            return services;
        }

        private static IServiceCollection ConfigClient(this IServiceCollection services)
        {
            var client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Info,
                GatewayIntents = GatewayIntents.All
            });

            // Login and connect.
            client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("discord_bot_token")).GetAwaiter().GetResult();
            client.StartAsync().GetAwaiter().GetResult();

            services.AddSingleton(client);

            return services;
        }
    }
}
