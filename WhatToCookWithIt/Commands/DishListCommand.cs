using Telegram.Bot.Types;
using Telegram.Bot;
using WhatToCookWithIt.Entities;
using WhatToCookWithIt.Interfaces;
using Newtonsoft.Json;
using System.Text;
using Telegram.Bot.Types.Enums;

namespace WhatToCookWithIt.Commands
{
    public class DishListCommand : ICommand
    {
        public TelegramBotClient Client => Bot.GetTelegramBot();
        public string Name => "/DishList";
        public async Task Execute(Update update)
        {
            long chatId = update.Message.Chat.Id;
            string messageText = update.Message.Text;

            string[] commandParts = messageText.Split(' ');

            if (commandParts.Length > 1)
            {
                string ingredient = string.Join(" ", commandParts.Skip(1));
                string apiUrl = $"https://www.themealdb.com/api/json/v1/1/filter.php?i={ingredient}";
                using (HttpClient httpClient = new HttpClient())
                {
                    HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                    string jsonContent = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ApiResult>(jsonContent);
                    if (result != null && result.Meals != null && result.Meals.Any())
                    {
                        StringBuilder messageBuilder = new StringBuilder();
                        messageBuilder.AppendLine($"Блюда с ингредиентом '{ingredient}':");

                        foreach (var meal in result.Meals)
                        {
                            string recipeLink = $"/DishRecipe {meal.IdMeal}";
                            string clickableMealName = $"<a href='{recipeLink}'>{meal.StrMeal}</a>";

                            messageBuilder.AppendLine(clickableMealName);
                        }

                        await Client.SendTextMessageAsync(chatId, messageBuilder.ToString(), parseMode: ParseMode.Html);
                    }
                    else
                    {
                        await Client.SendTextMessageAsync(chatId, $"Не удалось найти блюда с ингредиентом '{ingredient}'.");
                    }
                }
            }
            else
            {
                await Client.SendTextMessageAsync(chatId, "Пожалуйста, укажите ингредиент.");
            }
        }

        private class ApiResult
        {
            [JsonProperty("meals")]
            public List<Meal> Meals { get; set; }
        }
        private class Meal
        {
            [JsonProperty("idMeal")]
            public string IdMeal { get; set; }

            [JsonProperty("strMeal")]
            public string StrMeal { get; set; }
        }
    }
}