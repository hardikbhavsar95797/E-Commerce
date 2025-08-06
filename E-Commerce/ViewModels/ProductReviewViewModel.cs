using E_Commerce.Models;
using System.ComponentModel.DataAnnotations;

public class ProductReviewViewModel
{
    public long ProductId { get; set; }
    public long UserId { get; set; }

    [Range(1, 5)]
    public int Rating { get; set; }

    [Required(ErrorMessage = "ReviewText is required")]
    [MaxLength(1000)]
    public string ReviewText { get; set; }

    public List<ProductReview>? ExistingReviews { get; set; }
    public double AverageRating { get; set; }
}
