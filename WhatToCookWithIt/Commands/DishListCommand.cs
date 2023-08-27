using Telegram.Bot.Types;
using Telegram.Bot;
using WhatToCookWithIt.Entities;
using WhatToCookWithIt.Interfaces;

namespace WhatToCookWithIt.Commands
{
    public class DishListCommand : ICommand
    {
        public TelegramBotClient Client => Bot.GetTelegramBot();

        public string Name => "/DishList";

        public async Task Execute(Update update)
        {
            long chatId = update.Message.Chat.Id;
            await Client.SendTextMessageAsync(chatId, update.Message.Text);
        }
    }
}