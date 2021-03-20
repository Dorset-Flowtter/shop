using System.ComponentModel.DataAnnotations;

namespace shop.Models
{
    public class Product
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
    }
}