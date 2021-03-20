using System.ComponentModel.DataAnnotations;

namespace shop.DTO
{
    public class AddCustomer
    {
        [Required]
        public string Customer_firstname { get; set; }
        [Required]
        public string Customer_lastname { get; set; }
        [Required]
        public string Customer_username { get; set; }
        [Required]
        public string Customer_password { get; set; }
    }
}