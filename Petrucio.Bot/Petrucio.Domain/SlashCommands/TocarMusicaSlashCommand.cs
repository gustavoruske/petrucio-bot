using Discord.Interactions;

namespace Petrucio.Domain.SlashCommands
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
