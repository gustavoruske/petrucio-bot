using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Petrucio.Domain.SlashCommands;

namespace Petrucio.Domain.Services
{
    public class EntrypointService
    {
        private readonly DiscordSocketClient _client;
        private readonly UserVoiceStateUpdatedService _userVoiceStateUpdatedService;
        private readonly IMemoryCache _memoryCache;

        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;

        public EntrypointService(IServiceProvider serviceProvider, IConfiguration configuration, UserVoiceStateUpdatedService userVoiceStateUpdatedService, IMemoryCache memoryCache, DiscordSocketClient client)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
            _userVoiceStateUpdatedService = userVoiceStateUpdatedService;
            _memoryCache = memoryCache;
            _client = client;
        }

        public async Task Init()
        {
            Console.WriteLine($"variavel -> {_configuration.GetValue<string>("teste")}");
            Console.WriteLine($"variavel 2 -> {_configuration.GetValue<string>("http:exemplo")}");

            _client.Log += Log;

            _client.Ready += ClientReady;
            _client.UserVoiceStateUpdated += _userVoiceStateUpdatedService.Execute;

            await Task.Delay(Timeout.Infinite);
        }

        private async Task ClientReady()
        {
            //Limpa os commandos que ja existem
            List<ApplicationCommandProperties> applicationCommandProperties = new List<ApplicationCommandProperties>();
            await _client.BulkOverwriteGlobalApplicationCommandsAsync(applicationCommandProperties.ToArray());

            //Registra os novos comandos
            var _interactionService = new InteractionService(_client.Rest);
            await _interactionService.AddModulesAsync(typeof(IniciarSlashCommand).Assembly, _serviceProvider);
            await _interactionService.RegisterCommandsToGuildAsync(394844735778586635);

            _client.InteractionCreated += async interaction =>
            {
                var scope = _serviceProvider.CreateScope();
                var ctx = new SocketInteractionContext(_client, interaction);
                await _interactionService.ExecuteCommandAsync(ctx, scope.ServiceProvider);
            };
        }

        private static Task Log(LogMessage message)
        {
            switch (message.Severity)
            {
                case LogSeverity.Critical:
                case LogSeverity.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogSeverity.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogSeverity.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogSeverity.Verbose:
                case LogSeverity.Debug:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
            }
            Console.WriteLine($"{DateTime.Now,-19} [{message.Severity,8}] {message.Source}: {message.Message} {message.Exception}");
            Console.ResetColor();

            return Task.CompletedTask;
        }
    }
}
