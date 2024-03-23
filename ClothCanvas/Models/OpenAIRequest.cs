namespace ClothCanvas.Models;

//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
public class OpenAIRequest
{
    public string model { get; set; } = "dall-e-3";
    public string prompt { get; set; }
    public int n { get; set; } = 1;
    public string size { get; set; } = "1024x1024";
}
