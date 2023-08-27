using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;
using WhatToCookWithIt.Entities;

namespace WhatToCookWithIt.Controllers
{
    [ApiController]
    [Route("/")]
    public class TelegramBotController : ControllerBase
    {
        private readonly TelegramBotClient _botClient = Bot.GetTelegramBot();
        private readonly UpdateDistributor<CommandExecutor> updateDistributor = new UpdateDistributor<CommandExecutor>();

        [HttpPost]
        public async Task Post(Update update)
        {
            if (update.Message == null)
                return;
            await updateDistributor.GetUpdate(update);
        }

        //[HttpPost]
        //public async Task<IActionResult> Post([FromBody] Update update)
        //{
        //    await _messageHandler.HandleMessageAsync(update);

        //    return Ok();
        //}
        [HttpGet]
        public string Get()
        {
            return "bot is working";
        }
    }
}
