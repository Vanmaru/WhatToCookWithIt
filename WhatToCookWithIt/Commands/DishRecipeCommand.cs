using Newtonsoft.Json;
using Telegram.Bot.Types;
using Telegram.Bot;
using WhatToCookWithIt.Entities;
using WhatToCookWithIt.Interfaces;

namespace WhatToCookWithIt.Commands
{
    public class DishRecipeCommand : ICommand
    {
        public TelegramBotClient Client => Bot.GetTelegramBot();
        public string Name => "/DishRecipe";
        public async Task Execute(Update update)
        {
            long chatId = update.Message.Chat.Id;
            string messageText = update.Message.Text;
            string[] commandParts = messageText.Split(' ');

            if (commandParts.Length > 1)
            {
                string dishId = commandParts[1];
                string apiUrl = $"https://www.themealdb.com/api/json/v1/1/lookup.php?i={dishId}";

                // Отправляем GET-запрос и получаем JSON-ответ
                using (HttpClient httpClient = new HttpClient())
                {
                    HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                    string jsonContent = await response.Content.ReadAsStringAsync();

                    // Распарсим JSON-ответ
                    var result = JsonConvert.DeserializeObject<ApiResult>(jsonContent);

                    if (result != null && result.Meals != null && result.Meals.Any())
                    {
                        var meal = result.Meals[0];
                        // Формируем сообщение с рецептом
                        string recipeMessage = $"Рецепт для блюда '{meal.StrMeal}':\n\n{meal.StrInstructions}";

                        await Client.SendTextMessageAsync(chatId, recipeMessage);
                    }
                    else
                    {
                        await Client.SendTextMessageAsync(chatId, $"Не удалось найти рецепт для блюда с ID '{dishId}'.");
                    }
                }
            }
            else
            {
                await Client.SendTextMessageAsync(chatId, "Пожалуйста, укажите ID блюда.");
            }
        }

        private class ApiResult
        {
            [JsonProperty("meals")]
            public List<Meal> Meals { get; set; }
        }
        private class Meal
        {
            [JsonProperty("strMeal")]
            public string StrMeal { get; set; }

            [JsonProperty("strInstructions")]
            public string StrInstructions { get; set; }
        }
    }
}
