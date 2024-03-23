using ClothCanvas.Models;

namespace ClothCanvas.Services;

public interface IOpenAIService
{
    public Task<OpenAIResponse> GenerateImage(OpenAIRequest request);
}