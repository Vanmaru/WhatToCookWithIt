using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace WhatToCookWithIt
{
    internal class MessageHandler
    {
        private readonly TelegramBotClient _botClient;

        public MessageHandler(TelegramBotClient botClient)
        {
            _botClient = botClient;
        }

        public async Task HandleMessageAsync(Update update)
        {
            if (update.Message != null && update.Message.Type == MessageType.Text)
            {
                string messageText = update.Message.Text;
                long chatId = update.Message.Chat.Id;

                if (messageText == "Поиск рецепта по ингредиенту")
                {
                    var inlineKeyboard = new InlineKeyboardMarkup(new[]
                    {
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("Поиск рецепта по ингредиенту")
                    }
                });

                    await _botClient.SendTextMessageAsync(chatId, "Выберите действие:", replyMarkup: inlineKeyboard);
                }
                else if (update.Message.ReplyToMessage != null && update.Message.ReplyToMessage.Text == "Введите ингредиент:")
                {
                    string ingredient = update.Message.Text;

                    // Обработка поиска рецепта по ингредиенту
                    ingredient = await Translater.ToEnglishAsync(ingredient);
                    string recipe = await TheMealDB.ByIngredientAsync(ingredient);

                    // Отправка результата пользователю
                    await _botClient.SendTextMessageAsync(chatId, recipe);
                }
                else
                {
                    await _botClient.SendTextMessageAsync(chatId, $"You said: {messageText}");
                }
            }
            else if (update.CallbackQuery != null)
            {
                // Обработка нажатия inline-кнопки
                string callbackData = update.CallbackQuery.Data;
                long chatId = update.CallbackQuery.Message.Chat.Id;

                if (callbackData == "Поиск рецепта по ингредиенту")
                {
                    await _botClient.SendTextMessageAsync(chatId, "Введите ингредиент:");
                }
                // Добавьте код для обработки выбора блюда
                else if (callbackData.StartsWith("dish_"))
                {
                    int dishId = int.Parse(callbackData.Substring(5));
                    string dishInfo = await TheMealDB.GetDishByIdAsync(dishId);
                    await _botClient.SendTextMessageAsync(chatId, dishInfo);
                }
            }
        }
    }
}