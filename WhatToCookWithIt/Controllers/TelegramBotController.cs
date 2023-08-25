using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace WhatToCookWithIt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TelegramBotController : ControllerBase
    {
        private readonly TelegramBotClient _botClient;
        private readonly MessageHandler _messageHandler;

        public TelegramBotController()
        {
            _botClient = new TelegramBotClient("5940543077:AAGGMDgeErvU78WWKtqfgCphqyS2NLk99xA");
            _messageHandler = new MessageHandler(_botClient);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] Update update)
        {
            await _messageHandler.HandleMessageAsync(update);

            return Ok();
        }
    }
}
