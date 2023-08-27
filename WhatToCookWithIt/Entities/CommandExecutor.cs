using Telegram.Bot.Types;
using WhatToCookWithIt.Commands;
using WhatToCookWithIt.Interfaces;

namespace WhatToCookWithIt.Entities
{
    public class CommandExecutor : ITelegramUpdateListener
    {
        private List<ICommand> commands;
        private IListener? listener = null;

        public CommandExecutor()
        {
            commands = new List<ICommand>()
            {
                new StartCommand(),
                new DishListCommand(),
                new RegisterCommand(this)
            };
        }
        public async Task GetUpdate(Update update)
        {
            if (listener == null)
            {
                await ExecuteCommand(update);
            }
            else
            {
                await listener.GetUpdate(update);
            }
        }
        private async Task ExecuteCommand(Update update)
        {
            Message msg = update.Message;
            string commandText = msg.Text.Split(' ').First(); //we need only command name, without arguments

            foreach (var command in commands)
            {
                if (command.Name == commandText)
                {
                    await command.Execute(update);
                }
            }
        }
        public void StartListen(IListener newListener)
        {
            listener = newListener;
        }
        public void StopListen()
        {
            listener = null;
        }
    }
}
