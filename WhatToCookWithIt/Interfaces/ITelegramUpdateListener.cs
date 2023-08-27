using Telegram.Bot.Requests;
using Telegram.Bot.Types;

namespace WhatToCookWithIt.Interfaces
{
    public interface ITelegramUpdateListener
    {
        Task GetUpdate(Update update);
    }
}
