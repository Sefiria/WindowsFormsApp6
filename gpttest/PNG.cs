// See: https://platform.openai.com/docs/guides/chat/introduction
// See: https://platform.openai.com/docs/api-reference/chat

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

public class PNG
{
    private const string key = "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";

    public static string Ask(string msg)
    {
        return callOpenAI(30, msg, "text-babbage-001", 0.3, 1, 0, 0);
    }

    //
    private static string callOpenAI(int tokens, string input, string engine,
              double temperature, int topP, int frequencyPenalty, int presencePenalty)
    {

        var apiCall = "https://api.openai.com/v1/engines/" + engine + "/completions";

        try
        {

            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), apiCall))
                {
                    request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + key);
                    request.Content = new StringContent("{\n  \"prompt\": \"" + input + "\",\n  \"temperature\": " +
                                                        temperature.ToString(CultureInfo.InvariantCulture) + ",\n  \"max_tokens\": " + tokens + ",\n  \"top_p\": " + topP +
                                                        ",\n  \"frequency_penalty\": " + frequencyPenalty + ",\n  \"presence_penalty\": " + presencePenalty + "\n}");

                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                    var response = httpClient.SendAsync(request).Result;
                    var json = response.Content.ReadAsStringAsync().Result;

                    dynamic dynObj = JsonConvert.DeserializeObject(json);

                    if (dynObj != null)
                    {
                        return dynObj.choices[0].text.ToString();
                    }


                }
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur : {ex.Message}");

        }

        return "Attend, juste une seconde...";


    }

    //
}