using System.ComponentModel.DataAnnotations;

namespace shop.Models
{
    public class Customer
    {
        [Key]
        public int id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string username { get; set; }
        public string passwordhash { get; set; }
    }
}