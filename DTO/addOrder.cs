using System.ComponentModel.DataAnnotations;

namespace shop.DTO
{
    public class AddOrder
    {
        [Required]
        public int Order_customer_id {get; set;}
        [Required]
        public int Order_product_id {get; set;}
        [Required]
        public int Order_quantity {get; set;}
    }
}