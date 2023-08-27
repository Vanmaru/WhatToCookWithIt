using Telegram.Bot;

namespace WhatToCookWithIt.Entities
{
    public class Bot
    {
        private static TelegramBotClient Client { get; set; }
        public static TelegramBotClient GetTelegramBot()
        {
            if (Client != null)
            {
                return Client;
            }
            Client = new TelegramBotClient("5940543077:AAGGMDgeErvU78WWKtqfgCphqyS2NLk99xA");
            return Client;
        }
    }
}