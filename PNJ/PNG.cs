// See: https://platform.openai.com/docs/guides/chat/introduction
// See: https://platform.openai.com/docs/api-reference/chat

using Newtonsoft.Json;

public class PNG
{
    private const string key = "key";
    private const string url = "https://api.openai.com/v1/chat/completions";
    private List<dynamic> messages = null;

    public PNG()
    {
    }

    public void Initialize()
    {
        // Initialise the chat by describing the assistant,
        // and providing the assistants first question to the user
        messages = new List<dynamic>
        {
            new {role = "system",
                content = "You are ChatGPT, a large language " +
                                            "model trained by OpenAI. " +
                                            "Answer as concisely as possible.  " +
                                            "Make a joke every few lines just to spice things up."},
            new {role = "assistant",
                content = "How can I help you?"}
        };
    }

    public async Task<string> Ask(string msg)
    {
        // Capture the users messages and add to
        // messages list for submitting to the chat API
        messages.Add(new { role = "user", content = msg });

        // Create the request for the API sending the
        // latest collection of chat messages
        var request = new
        {
            messages,
            model = "gpt-3.5-turbo",
            max_tokens = 300,
        };

        // Send the request and capture the response
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {key}");
        var requestJson = JsonConvert.SerializeObject(request);
        var requestContent = new StringContent(requestJson, System.Text.Encoding.UTF8, "application/json");
        var httpResponseMessage = await httpClient.PostAsync(url, requestContent);
        var jsonString = await httpResponseMessage.Content.ReadAsStringAsync();
        var responseObject = JsonConvert.DeserializeAnonymousType(jsonString, new
        {
            choices = new[] { new { message = new { role = string.Empty, content = string.Empty } } },
            error = new { message = string.Empty }
        });


        if (!string.IsNullOrEmpty(responseObject?.error?.message))  // Check for errors
        {
            return responseObject?.error.message;
        }
        else  // Add the message object to the message collection
        {
            var messageObject = responseObject?.choices[0].message;
            messages.Add(messageObject);
            return messageObject.content;
        }
    }
}