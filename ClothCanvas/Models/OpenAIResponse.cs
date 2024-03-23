namespace ClothCanvas.Models;

public class OpenAIResponse
{
    public long created { get; set; }
    public OpenAIData[] data { get; set; }
}


public class OpenAIData
{
    public string revised_prompt { get; set; }
    public string url { get; set; }
}