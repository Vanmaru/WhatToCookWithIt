using System.Net.Http.Headers;

namespace WhatToCookWithIt
{
    public static class Translater
    {
        public static async Task<string> ToEnglishAsync(string input)
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://google-translate1.p.rapidapi.com/language/translate/v2"),
                    Headers =
                    {
                        { "X-RapidAPI-Key", "2de1826290msh097c9f46fb99712p119414jsnd5e4694fa7ce" },
                        { "X-RapidAPI-Host", "google-translate1.p.rapidapi.com" },
                    },
                    Content = new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                        { "q", input },
                        { "target", "en" },
                    }),
                };

                try
                {
                    using (var response = await client.SendAsync(request))
                    {
                        response.EnsureSuccessStatusCode();
                        return await response.Content.ReadAsStringAsync();
                    }
                }
                catch (HttpRequestException ex)
                {
                    // Обработка исключения, например, запись в лог или возврат сообщения об ошибке.
                    return $"Error: {ex.Message}";
                }
            }
        }
    }
}
