using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Net;
using Petrucio.Entrypoint;
using Newtonsoft.Json;
using Discord.Interactions;
using System;

class Start
{
    // Program entry point
    static Task Main(string[] args)
    {
        // Call the Program constructor, followed by the 
        // MainAsync method and wait until it finishes (which should be never).
        return new Start().MainAsync();
    }

    private readonly DiscordSocketClient _client;

    // Keep the CommandService and DI container around for use with commands.
    // These two types require you install the Discord.Net.Commands package.
    private readonly CommandService _commands;
    private readonly IServiceProvider _services;

    private Start()
    {
        _client = new DiscordSocketClient(new DiscordSocketConfig
        {
            LogLevel = LogSeverity.Info,
            GatewayIntents = GatewayIntents.All
        });

        _commands = new CommandService(new CommandServiceConfig
        {
            LogLevel = LogSeverity.Info,
            CaseSensitiveCommands = false,
        });

        _client.Log += Log;
        _commands.Log += Log;

        _services = ConfigureServices();
    }

    private static IServiceProvider ConfigureServices()
    {
        var map = new ServiceCollection();
            // and other dependencies that your commands might need.
            //.AddSingleton<SomeModule>();

        return map.BuildServiceProvider();
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

    private async Task MainAsync()
    {
        // Centralize the logic for commands into a separate method.
        await InitCommands();

        // Login and connect.
        await _client.LoginAsync(TokenType.Bot,
            Environment.GetEnvironmentVariable("discord_bot_token")
            );
        await _client.StartAsync();

        _client.Ready += Client_Ready;
        //_client.SlashCommandExecuted += SlashCommandHandler;

        await Task.Delay(Timeout.Infinite);
    }

    private async Task Client_Ready()
    {
        var _interactionService = new InteractionService(_client);
        await _interactionService.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        //await _interactionService.RegisterCommandsToGuildAsync(_guildId);

        _client.InteractionCreated += async interaction =>
        {
            var scope = _services.CreateScope();
            var ctx = new SocketInteractionContext(_client, interaction);
            await _interactionService.ExecuteCommandAsync(ctx, scope.ServiceProvider);
        };
    }

    private async Task InitCommands()
    {
        await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        _client.MessageReceived += HandleCommandAsync;
    }

    private async Task HandleCommandAsync(SocketMessage arg)
    {
        // Bail out if it's a System Message.
        var msg = arg as SocketUserMessage;
        if (msg == null) return;

        // We don't want the bot to respond to itself or other bots.
        if (msg.Author.Id == _client.CurrentUser.Id || msg.Author.IsBot) return;

        int pos = 0;
        if (msg.HasCharPrefix('!', ref pos) /* || msg.HasMentionPrefix(_client.CurrentUser, ref pos) */)
        {
            // Create a Command Context.
            var context = new SocketCommandContext(_client, msg);

            // Execute the command. (result does not indicate a return value, 
            // rather an object stating if the command executed successfully).
            var result = await _commands.ExecuteAsync(context, pos, _services);

            await msg.Channel.SendMessageAsync("Opa!");
        }
    }
}