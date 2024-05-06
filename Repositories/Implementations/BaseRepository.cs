using Exchange_App.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange_App.Repositories.Implementations
{
    public class BaseRepository  
    {



        private StuffExchangeAppEntities dbContext = DataProvider.Ins.DB;

        public BaseRepository()
        {
            //dbContext.Database.Connection.ConnectionString = "Data Source=DESKTOP-1VJ6V2G;Initial Catalog=ExchangeBee;Integrated Security=True";
        }
        public StuffExchangeAppEntities DbContext { get => dbContext; set => dbContext=value; }
    }
}
