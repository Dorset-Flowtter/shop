using shop.Data;
using System.Linq;

namespace shop.utils
{   
    public class utils
    {
        public static bool CustomerExist(Context _context, int id)
        {
            return _context.Customer.Any(x => x.id == id);
        }
        public static bool ProductExist(Context _context, int id)
        {
            return _context.Product.Any(x => x.id == id);
        }
        public static bool OrderExist(Context _context, int id)
        {
            return _context.Order.Any(x => x.id == id);
        }
    }
}