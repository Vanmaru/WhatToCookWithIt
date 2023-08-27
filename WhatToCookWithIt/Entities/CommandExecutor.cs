using Telegram.Bot.Types;
using WhatToCookWithIt.Commands;
using WhatToCookWithIt.Interfaces;

namespace WhatToCookWithIt.Entities
{
    public class CommandExecutor : ITelegramUpdateListener
    {
        private List<ICommand> commands;

        public CommandExecutor()
        {
            commands = new List<ICommand>();
            {
                new StartCommand();
            }
        }
        public async Task GetUpdate(Update update)
        {
            Message msg = update.Message;
            if (msg.Text == null) //такое бывает, во избежании ошибок делаем проверку
                return;

            foreach (var command in commands)
            {
                if (command.Name == msg.Text)
                {
                    await command.Execute(update);
                }
            }
        }
    }
}
