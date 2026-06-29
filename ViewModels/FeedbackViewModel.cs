namespace CogStayMVC.ViewModels;

public class FeedbackViewModel
{
    public int RatingValue { get; set; }
    public string FeedbackCategory { get; set; } = "comfort";
    public string FeedbackComments { get; set; } = string.Empty;
}
