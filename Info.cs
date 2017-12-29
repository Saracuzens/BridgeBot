using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

public class InfoModule : ModuleBase<SocketCommandContext>
{
    // ~say hello -> hello
	[Command("say")]
	[Summary("Echos a message.")]
	public async Task SayAsync([Remainder] [Summary("The text to echo")] string echo)
	{
		// ReplyAsync is a method on ModuleBase
		await ReplyAsync(echo);
	}

    [Command("pedoalert")]
	[Summary("Break the glass in case of pedo")]
	public async Task PedoAlertAsync([Remainder] [Summary("The text to ignore")] string echo=null)
	{
		// ReplyAsync is a method on ModuleBase
		await Context.Channel.SendMessageAsync("https://images-cdn.9gag.com/photo/a5n3BxG_700b.jpg");
	}

    // ~sample userinfo --> foxbot#0282
	// ~sample userinfo @Khionu --> Khionu#8708
	// ~sample userinfo Khionu#8708 --> Khionu#8708
	// ~sample userinfo Khionu --> Khionu#8708
	// ~sample userinfo 96642168176807936 --> Khionu#8708
	// ~sample whois 96642168176807936 --> Khionu#8708
	[Command("userinfo")]
	[Summary("Returns info about the current user, or the user parameter, if one passed.")]
	[Alias("user", "whois")]
	public async Task UserInfoAsync([Summary("The (optional) user to get info for")] SocketUser user = null)
	{
		var userInfo = user ?? Context.Client.CurrentUser;
		await ReplyAsync($"{userInfo.Mention} : {userInfo.Username}#{userInfo.Discriminator}");
        var c = userInfo.GetOrCreateDMChannelAsync().Result;
        await c.SendMessageAsync("asdfghjklöä");
	}
}
