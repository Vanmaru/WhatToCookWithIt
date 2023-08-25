namespace WhatToCookWithIt
{
    public static class TheMealDB
    {
        public static async Task<string> ByIngredientAsync(string ingredient)
        {
            string apiUrl = $"https://www.themealdb.com/api/json/v1/1/filter.php?i={ingredient}";

            using (HttpClient client = new())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    response.EnsureSuccessStatusCode();

                    return await response.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException ex)
                {
                    return ($"HTTP Request Error: {ex.Message}");
                }
            }
        }
        public static async Task<string> GetDishByIdAsync(int id)
        {
            string apiUrl = $"www.themealdb.com/api/json/v1/1/lookup.php?i={id}";

            using (HttpClient client = new())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    response.EnsureSuccessStatusCode();

                    return await response.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException ex)
                {
                    return ($"HTTP Request Error: {ex.Message}");
                }
            }
        }
    }
}