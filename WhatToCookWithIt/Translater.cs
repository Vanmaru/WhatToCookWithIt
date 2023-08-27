using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace WhatToCookWithIt
{
    public static class Translater
    {
        public static async Task<string> ToRussianAsync(string input)
        {
            using (var client = new HttpClient())
            {
                var request = CreateTranslationRequest(input, "ru", "en");

                try
                {
                    using (var response = await client.SendAsync(request))
                    {
                        response.EnsureSuccessStatusCode();
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        string translatedText = GetTranslatedText(jsonResponse);
                        return translatedText;
                    }
                }
                catch (HttpRequestException ex)
                {
                    return $"Error: {ex.Message}";
                }
            }
        }

        public static async Task<string> ToEnglishAsync(string input)
        {
            using (var client = new HttpClient())
            {
                var request = CreateTranslationRequest(input, "en", "ru");

                try
                {
                    using (var response = await client.SendAsync(request))
                    {
                        response.EnsureSuccessStatusCode();
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        string translatedText = GetTranslatedText(jsonResponse);
                        return translatedText;
                    }
                }
                catch (HttpRequestException ex)
                {
                    return $"Error: {ex.Message}";
                }
            }
        }
        private static HttpRequestMessage CreateTranslationRequest(string input, string targetLanguage, string sourceLanguage)
        {
            return new HttpRequestMessage
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
                    { "target", targetLanguage },
                    { "source", sourceLanguage },
                }),
            };
        }
        private static string GetTranslatedText(string jsonResponse)
        {
            var translationResponse = JsonConvert.DeserializeObject<TranslationResponse>(jsonResponse);
            if (translationResponse?.Data?.Translations != null && translationResponse.Data.Translations.Count > 0)
            {
                return translationResponse.Data.Translations[0].TranslatedTextField;
            }
            else
            {
                return "Error: No translations found in the response.";
            }
        }
        private class TranslationResponse
        {
            [JsonProperty("data")]
            public TranslationData Data { get; set; }
        }

        private class TranslationData
        {
            [JsonProperty("translations")]
            public List<TranslatedText> Translations { get; set; }
        }

        private class TranslatedText
        {
            [JsonProperty("translatedText")]
            public string TranslatedTextField { get; set; }
        }
    }
}