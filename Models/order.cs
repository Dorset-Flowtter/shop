using System.ComponentModel.DataAnnotations;

namespace shop.Models
{
    public class Order
    {
        [Key]
        public int id { get; set; }
        public int idproduct { get; set; }
        public int idcustomer { get; set; }
        public int quantity { get; set; }
    }
}