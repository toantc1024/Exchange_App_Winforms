using Exchange_App.Model;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace Exchange_App.ViewModel
{


    public class UserManagerViewModel :BaseViewModel
    {
        private ExchangeBeeEntities dbContext;
        private ObservableCollection<User> _users;
        public ExchangeBeeEntities DbContext { get => dbContext; set => dbContext=value; }
        private User _currentUser;

        public UserManagerViewModel(User currentUser)
        {
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();
            sqlBuilder.DataSource = "(localdb)\\mssqllocaldb";
            sqlBuilder.InitialCatalog = "ExchangeBee";
            sqlBuilder.PersistSecurityInfo = true;
            sqlBuilder.IntegratedSecurity = true;
            sqlBuilder.MultipleActiveResultSets = true;
            sqlBuilder.TrustServerCertificate = true;
            sqlBuilder.UserID = "tctoan";
            sqlBuilder.Password  = "12356!";

            using (SqlConnection connection = new SqlConnection(sqlBuilder.ToString()))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM FUNC_getAllUsers()", connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        Console.WriteLine(String.Format("{0}, {1}",
                        reader["tPatCulIntPatIDPk"], reader["tPatSFirstname"]));// etc
                    }
                }
                finally
                {
                    // Always call Close when done reading.
                    reader.Close();
                }
            }


            CurrentUser=currentUser;


        }

        public User CurrentUser { get => _currentUser; set {
                _currentUser=value; OnPropertyChanged();
            } }

        public ObservableCollection<User> Users { get => _users; set {

                _users=value; OnPropertyChanged();
            } }
    }
}
