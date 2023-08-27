using Telegram.Bot.Types;
using WhatToCookWithIt.Entities;

namespace WhatToCookWithIt.Interfaces
{
    public interface IListener
    {
        public async Task GetUpdate(Update update) { }

        public CommandExecutor Executor { get; }
    }
}
