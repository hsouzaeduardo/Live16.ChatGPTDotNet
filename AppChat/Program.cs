using System.Text;
using Newtonsoft.Json;


while (true)
{


    HttpClient client = new HttpClient();

    client.DefaultRequestHeaders.Add("authorization", "Bearer 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");

    Console.WriteLine("Digite sua pergunta");

    string texto = Console.ReadLine();

    var content = new StringContent("{\"model\": \"text-davinci-003\", \"prompt\": \"" + texto + "\",\"temperature\": 1,\"max_tokens\": 100}",
        Encoding.UTF8, "application/json");

    HttpResponseMessage response = await client.PostAsync("https://api.openai.com/v1/completions", content);

    string responseString = await response.Content.ReadAsStringAsync();

    try
    {
        var dyData = JsonConvert.DeserializeObject<dynamic>(responseString);

        string guess = GuessCommand(dyData!.choices[0].text);
        Console.ForegroundColor = ConsoleColor.Green;
        //Console.WriteLine($"---> Minha pergunta no prompt de comando é: {guess}");
        Console.ResetColor();

    }
    catch (Exception ex)
    {
        Console.WriteLine($"---> Não foi possível desserializar o JSON: {ex.Message}");
    }

    //Console.WriteLine(responseString);
}

static string GuessCommand(string raw)
{
    Console.WriteLine("---> Texto retornado da API GPT-3:");
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine(raw);

    var lastIndex = raw.LastIndexOf('\n');

    string guess = raw.Substring(lastIndex + 1);

    Console.ResetColor();

    TextCopy.ClipboardService.SetText(guess);

    return guess;
}