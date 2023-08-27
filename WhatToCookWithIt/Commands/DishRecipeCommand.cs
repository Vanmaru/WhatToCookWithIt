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
        public string Name => "/dish";

        public async Task Execute(Update update)
        {
            long chatId = update.Message.Chat.Id;
            string messageText = update.Message.Text;
            string[] commandParts = messageText.Split(' ');

            if (commandParts.Length > 1)
            {
                string dishId = commandParts[1];
                string apiUrl = $"https://www.themealdb.com/api/json/v1/1/lookup.php?i={dishId}";

                using (HttpClient httpClient = new HttpClient())
                {
                    HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                    string jsonContent = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ApiResult>(jsonContent);

                    if (result != null && result.Meals != null && result.Meals.Any())
                    {
                        var meal = result.Meals[0];
                        string mealName = await Translater.ToRussianAsync(meal.StrMeal);
                        string instrustions = await Translater.ToRussianAsync(meal.StrInstructions);
                        string recipeMessage = $"Рецепт для блюда '{mealName}':\n\n{instrustions}";

                        // Отправка фото и рецепта
                        await SendDishRecipeWithPhoto(chatId, meal, recipeMessage);
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
        private async Task SendDishRecipeWithPhoto(long chatId, Meal meal, string recipeMessage)
        {
            using (var httpClient = new HttpClient())
            {
                var imageStream = await httpClient.GetStreamAsync(meal.StrMealThumb);

                // Создание InputFileStream из потока с изображением
                var inputFileStream = new InputFileStream(imageStream);

                // Отправка изображения и рецепта
                await Client.SendPhotoAsync(chatId, inputFileStream, caption: recipeMessage);
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

            [JsonProperty("strMealThumb")]
            public string StrMealThumb { get; set; }

            [JsonProperty("strInstructions")]
            public string StrInstructions { get; set; }
        }
    }
}
