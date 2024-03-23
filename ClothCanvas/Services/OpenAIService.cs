using System.Text;
using System.Text.Json;
using ClothCanvas.Models;

namespace ClothCanvas.Services;

public class OpenAIService : IOpenAIService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public OpenAIService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<OpenAIResponse> GenerateImage(OpenAIRequest request)
    {
        // var response = new OpenAIResponse()
        // {
        //     created = 1631819535,
        //     data =
        //     [
        //         new OpenAIData
        //         {
        //             url =
        //                 "https://oaidalleapiprodscus.blob.core.windows.net/private/org-93lQCx6pKOE9mSuTjcCFTMch/user-lh1veBF0qEhU4Zls0ZhS1p1z/img-WlJlgCqGuyr7b8Pj9qjPoqGK.png?st=2024-03-13T15:29:46Z&se=2024-03-13T17:29:46Z&sp=r&sv=2021-08-06&sr=b&rscd=inline&rsct=image/png&skoid=6aaadede-4fb3-4698-a8f6-684d7786b067&sktid=a48cca56-e6da-484e-a814-9c849652bcb3&skt=2024-03-13T15:40:29Z&ske=2024-03-14T15:40:29Z&sks=b&skv=2021-08-06&sig=9euSbAa/7wDQ1j9v/5irW04AoQqaA0+F+0nX4QvY5UE="
        //         }
        //     ]
        // };
        //
        // return response;


        var apiKey = _configuration["OpenAI:ApiKey"];
        var url = _configuration["OpenAI:Url"];
        
        var json = JsonSerializer.Serialize(request);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
        
        var response = await _httpClient.PostAsync(url, data);
        
        if (response.IsSuccessStatusCode)
        {
            var responseStream = await response.Content.ReadAsStreamAsync();
            var openAIResponse = await JsonSerializer.DeserializeAsync<OpenAIResponse>(responseStream);
            return openAIResponse;
        }
        else
        {
            throw new Exception("Failed to generate image");
        }
    }
}