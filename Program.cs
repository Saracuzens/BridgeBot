using System;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.WebSocket;
using Discord.Commands;

namespace MyBot
{
	public class Program
	{

        private DiscordSocketClient _client;

        private CommandService _commands;
        private IServiceProvider _services;

		public static void Main(string[] args)
			=> new Program().MainAsync().GetAwaiter().GetResult();

        // Main Task

		public async Task MainAsync()
		{
            _client = new DiscordSocketClient();
            _commands = new CommandService();

            _client.Log += Log;
            // _client.MessageReceived += MessageReceived;

            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .BuildServiceProvider();

            await InstallCommandsAsync();

            Console.WriteLine(Directory.GetCurrentDirectory());

            string token = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(),"Config\\token"));
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
		}

        // Other tasks

        public async Task InstallCommandsAsync()
        {
            // Hook the MessageReceived Event into our Command Handler
            _client.MessageReceived += HandleCommandAsync;
            // Discover all of the commands in this assembly and load them.
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly());
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            // Don't process the command if it was a System Message
            var message = messageParam as SocketUserMessage;
            if (message == null) return;
            // Create a number to track where the prefix ends and the command begins
            int argPos = 0;
            // Determine if the message is a command, based on if it starts with '!' or a mention prefix
            if (!(message.HasCharPrefix('!', ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos))) return;
            // Create a Command Context
            var context = new SocketCommandContext(_client, message);
            // Execute the command. (result does not indicate a return value, 
            // rather an object stating if the command executed successfully)
            var result = await _commands.ExecuteAsync(context, argPos, _services);
            if (!result.IsSuccess)
                await context.Channel.SendMessageAsync(result.ErrorReason);
        }


/*
        private async Task MessageReceived(SocketMessage message)
        {
            if (message.Content == "moral is a pedo" || message.Content == "im gay")
            {
                await message.Channel.SendMessageAsync("We know.");
            }
        }
*/
		private Task Log(LogMessage msg)
		{
			Console.WriteLine(msg.ToString());
			return Task.CompletedTask;
		}
	}
}
