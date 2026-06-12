using System.Collections.Generic;
using System.Linq;
using FoodyExpress.Models;

namespace FoodyExpress.Helpers
{
    public class CartItem
    {
        public FoodyExpress.Models.MenuItem? MenuItem { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal => (MenuItem?.Price ?? 0) * Quantity;
    }

    public static class CartHelper
    {
        public static List<CartItem> CurrentCart { get; private set; } = new List<CartItem>();

        public static void AddItem(FoodyExpress.Models.MenuItem item, int quantity = 1)
        {
            var existing = CurrentCart.FirstOrDefault(c => c.MenuItem?.MenuItemID == item.MenuItemID);
            if (existing != null)
            {
                existing.Quantity += quantity;
            }
            else
            {
                CurrentCart.Add(new CartItem { MenuItem = item, Quantity = quantity });
            }
        }

        public static void RemoveItem(int menuItemId)
        {
            var item = CurrentCart.FirstOrDefault(c => c.MenuItem?.MenuItemID == menuItemId);
            if (item != null)
            {
                CurrentCart.Remove(item);
            }
        }

        public static void ClearCart()
        {
            CurrentCart.Clear();
        }

        public static decimal GetTotal()
        {
            return CurrentCart.Sum(c => c.Subtotal);
        }
    }
}
