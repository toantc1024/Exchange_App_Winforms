using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange_App.Repositories.Interfaces
{
    public interface IWishItemRepository
    {
        bool CheckWishItem(int ProductID, int UserID);
    }
}
