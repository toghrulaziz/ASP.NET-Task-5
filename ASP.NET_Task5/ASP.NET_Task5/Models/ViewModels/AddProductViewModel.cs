using System.ComponentModel.DataAnnotations;

namespace ASP.NET_Task5.Models.ViewModels
{
    public class AddProductViewModel
    {
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "CategoryId is required")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter a valid price")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Image is required")]
        public IFormFile ImageUrl { get; set; }

        [Required(ErrorMessage = "TagIds are required")]
        public List<int> TagIds { get; set; }
    }
}
