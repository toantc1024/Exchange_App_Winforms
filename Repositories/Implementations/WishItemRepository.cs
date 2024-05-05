using Exchange_App.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange_App.Repositories.Implementations
{
    internal class WishItemRepository :BaseRepository, IWishItemRepository
    {
        public WishItemRepository()
        {
            
        }

        public bool CheckWishItem(int ProductID, int UserID)
        {
            var wish = DbContext.WishItems.Where(x => (x.ProductID == ProductID && x.UserID == UserID)).ToList();
            return wish.Count > 0;


        }
    }
}
