
namespace staj_forum_backend.DTOs.Chat;

public class CVAnalysisRequestDto
{
    public string CvText { get; set; } = string.Empty;
}

public class CVAnalysisResponseDto
{
    public string Analysis { get; set; } = string.Empty;
}
