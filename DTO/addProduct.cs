using System.ComponentModel.DataAnnotations;

namespace shop.DTO
{
    public class AddProduct
    {
        [Required]
        public string Product_name {get; set;}
    }
}