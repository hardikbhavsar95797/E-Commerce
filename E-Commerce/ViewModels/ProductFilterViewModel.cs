using E_Commerce.Models;

namespace E_Commerce.ViewModels
{
    public class ProductFilterViewModel
    {
        public string SearchQuery { get; set; }
        public int? CategoryId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? MinRating { get; set; }

        public List<Product> Products { get; set; }
        public List<Category> Categories { get; set; }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
