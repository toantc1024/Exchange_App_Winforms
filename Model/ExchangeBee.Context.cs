﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Exchange_App.Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class ExchangeBeeEntities : DbContext
    {
        public ExchangeBeeEntities()
            : base("name=ExchangeBeeEntities")
        {
        }

        public ExchangeBeeEntities(string connStr) :base(connStr)
        {

        }

    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User_Order> User_Order { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<WishItem> WishItems { get; set; }
        public virtual DbSet<View_category> View_category { get; set; }
        public virtual DbSet<View_orderdetail> View_orderdetail { get; set; }
        public virtual DbSet<View_product_images> View_product_images { get; set; }
        public virtual DbSet<View_products> View_products { get; set; }
        public virtual DbSet<View_user_order> View_user_order { get; set; }
        public virtual DbSet<View_users> View_users { get; set; }
        public virtual DbSet<View_wishitem> View_wishitem { get; set; }
    
        [DbFunction("ExchangeBeeEntities", "FUNC_getAllOrders")]
        public virtual IQueryable<User_Order> FUNC_getAllOrders()
        {
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<User_Order>("[ExchangeBeeEntities].[FUNC_getAllOrders]()");
        }
    
        [DbFunction("ExchangeBeeEntities", "FUNC_getAllUsers")]
        public virtual IQueryable<User> FUNC_getAllUsers()
        {
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<User>("[ExchangeBeeEntities].[FUNC_getAllUsers]()");
        }
    
        [DbFunction("ExchangeBeeEntities", "FUNC_GetImagesByProductID")]
        public virtual IQueryable<Image> FUNC_GetImagesByProductID(Nullable<int> productID)
        {
            var productIDParameter = productID.HasValue ?
                new ObjectParameter("ProductID", productID) :
                new ObjectParameter("ProductID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<Image>("[ExchangeBeeEntities].[FUNC_GetImagesByProductID](@ProductID)", productIDParameter);
        }
    
        [DbFunction("ExchangeBeeEntities", "FUNC_GetOrderDetail")]
        public virtual IQueryable<OrderDetail> FUNC_GetOrderDetail(Nullable<int> orderID)
        {
            var orderIDParameter = orderID.HasValue ?
                new ObjectParameter("OrderID", orderID) :
                new ObjectParameter("OrderID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<OrderDetail>("[ExchangeBeeEntities].[FUNC_GetOrderDetail](@OrderID)", orderIDParameter);
        }
    
        [DbFunction("ExchangeBeeEntities", "FUNC_GetOrdersByUserID")]
        public virtual IQueryable<User_Order> FUNC_GetOrdersByUserID(Nullable<int> userID)
        {
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<User_Order>("[ExchangeBeeEntities].[FUNC_GetOrdersByUserID](@UserID)", userIDParameter);
        }
    
        [DbFunction("ExchangeBeeEntities", "FUNC_GetProductByCategories")]
        public virtual IQueryable<Product> FUNC_GetProductByCategories(Nullable<int> catID)
        {
            var catIDParameter = catID.HasValue ?
                new ObjectParameter("CatID", catID) :
                new ObjectParameter("CatID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<Product>("[ExchangeBeeEntities].[FUNC_GetProductByCategories](@CatID)", catIDParameter);
        }
    
        [DbFunction("ExchangeBeeEntities", "FUNC_GetProductByName")]
        public virtual IQueryable<Product> FUNC_GetProductByName(string keyword)
        {
            var keywordParameter = keyword != null ?
                new ObjectParameter("Keyword", keyword) :
                new ObjectParameter("Keyword", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<Product>("[ExchangeBeeEntities].[FUNC_GetProductByName](@Keyword)", keywordParameter);
        }
    
        [DbFunction("ExchangeBeeEntities", "FUNC_GetUserInformationById")]
        public virtual IQueryable<User> FUNC_GetUserInformationById(Nullable<int> userID)
        {
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<User>("[ExchangeBeeEntities].[FUNC_GetUserInformationById](@UserID)", userIDParameter);
        }
    
        [DbFunction("ExchangeBeeEntities", "FUNC_Login")]
        public virtual IQueryable<User> FUNC_Login(string username, string password)
        {
            var usernameParameter = username != null ?
                new ObjectParameter("Username", username) :
                new ObjectParameter("Username", typeof(string));
    
            var passwordParameter = password != null ?
                new ObjectParameter("Password", password) :
                new ObjectParameter("Password", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<User>("[ExchangeBeeEntities].[FUNC_Login](@Username, @Password)", usernameParameter, passwordParameter);
        }
    
        [DbFunction("ExchangeBeeEntities", "FUNC_SearchProductByName")]
        public virtual IQueryable<Product> FUNC_SearchProductByName(string productName)
        {
            var productNameParameter = productName != null ?
                new ObjectParameter("ProductName", productName) :
                new ObjectParameter("ProductName", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<Product>("[ExchangeBeeEntities].[FUNC_SearchProductByName](@ProductName)", productNameParameter);
        }
    
        [DbFunction("ExchangeBeeEntities", "FUNC_SearchProductByUserID")]
        public virtual IQueryable<Product> FUNC_SearchProductByUserID(Nullable<int> userID)
        {
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<Product>("[ExchangeBeeEntities].[FUNC_SearchProductByUserID](@UserID)", userIDParameter);
        }
    
        public virtual int PROC_AddCat(string catName)
        {
            var catNameParameter = catName != null ?
                new ObjectParameter("CatName", catName) :
                new ObjectParameter("CatName", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("PROC_AddCat", catNameParameter);
        }
    
        public virtual int PROC_AddImageByProductID(string imageURL, Nullable<int> productID)
        {
            var imageURLParameter = imageURL != null ?
                new ObjectParameter("ImageURL", imageURL) :
                new ObjectParameter("ImageURL", typeof(string));
    
            var productIDParameter = productID.HasValue ?
                new ObjectParameter("ProductID", productID) :
                new ObjectParameter("ProductID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("PROC_AddImageByProductID", imageURLParameter, productIDParameter);
        }
    
        public virtual int PROC_AddUserOrder(Nullable<int> userID, Nullable<int> productID, Nullable<int> quantity)
        {
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(int));
    
            var productIDParameter = productID.HasValue ?
                new ObjectParameter("ProductID", productID) :
                new ObjectParameter("ProductID", typeof(int));
    
            var quantityParameter = quantity.HasValue ?
                new ObjectParameter("Quantity", quantity) :
                new ObjectParameter("Quantity", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("PROC_AddUserOrder", userIDParameter, productIDParameter, quantityParameter);
        }
    
        public virtual int PROC_AddWishItem(Nullable<int> productID, Nullable<int> userID)
        {
            var productIDParameter = productID.HasValue ?
                new ObjectParameter("ProductID", productID) :
                new ObjectParameter("ProductID", typeof(int));
    
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("PROC_AddWishItem", productIDParameter, userIDParameter);
        }
    
        public virtual int PROC_CreateAccount(string name, string username, string password, string phone, string address, Nullable<System.DateTime> birthdate, Nullable<int> roleID)
        {
            var nameParameter = name != null ?
                new ObjectParameter("Name", name) :
                new ObjectParameter("Name", typeof(string));
    
            var usernameParameter = username != null ?
                new ObjectParameter("Username", username) :
                new ObjectParameter("Username", typeof(string));
    
            var passwordParameter = password != null ?
                new ObjectParameter("Password", password) :
                new ObjectParameter("Password", typeof(string));
    
            var phoneParameter = phone != null ?
                new ObjectParameter("Phone", phone) :
                new ObjectParameter("Phone", typeof(string));
    
            var addressParameter = address != null ?
                new ObjectParameter("Address", address) :
                new ObjectParameter("Address", typeof(string));
    
            var birthdateParameter = birthdate.HasValue ?
                new ObjectParameter("Birthdate", birthdate) :
                new ObjectParameter("Birthdate", typeof(System.DateTime));
    
            var roleIDParameter = roleID.HasValue ?
                new ObjectParameter("RoleID", roleID) :
                new ObjectParameter("RoleID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("PROC_CreateAccount", nameParameter, usernameParameter, passwordParameter, phoneParameter, addressParameter, birthdateParameter, roleIDParameter);
        }
    
        public virtual int PROC_DeleteCat(Nullable<int> catId)
        {
            var catIdParameter = catId.HasValue ?
                new ObjectParameter("CatId", catId) :
                new ObjectParameter("CatId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("PROC_DeleteCat", catIdParameter);
        }
    
        public virtual int PROC_DeleteProduct(Nullable<int> productID)
        {
            var productIDParameter = productID.HasValue ?
                new ObjectParameter("ProductID", productID) :
                new ObjectParameter("ProductID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("PROC_DeleteProduct", productIDParameter);
        }
    
        public virtual int PROC_DeleteWishItem(Nullable<int> wishItemID)
        {
            var wishItemIDParameter = wishItemID.HasValue ?
                new ObjectParameter("WishItemID", wishItemID) :
                new ObjectParameter("WishItemID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("PROC_DeleteWishItem", wishItemIDParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> PROC_EditProduct(Nullable<int> productID, Nullable<int> quantity, string info_des, string status_des, Nullable<double> original_price, Nullable<double> sell_price, string productName, Nullable<int> catID, Nullable<int> userID)
        {
            var productIDParameter = productID.HasValue ?
                new ObjectParameter("ProductID", productID) :
                new ObjectParameter("ProductID", typeof(int));
    
            var quantityParameter = quantity.HasValue ?
                new ObjectParameter("Quantity", quantity) :
                new ObjectParameter("Quantity", typeof(int));
    
            var info_desParameter = info_des != null ?
                new ObjectParameter("Info_des", info_des) :
                new ObjectParameter("Info_des", typeof(string));
    
            var status_desParameter = status_des != null ?
                new ObjectParameter("Status_des", status_des) :
                new ObjectParameter("Status_des", typeof(string));
    
            var original_priceParameter = original_price.HasValue ?
                new ObjectParameter("Original_price", original_price) :
                new ObjectParameter("Original_price", typeof(double));
    
            var sell_priceParameter = sell_price.HasValue ?
                new ObjectParameter("Sell_price", sell_price) :
                new ObjectParameter("Sell_price", typeof(double));
    
            var productNameParameter = productName != null ?
                new ObjectParameter("ProductName", productName) :
                new ObjectParameter("ProductName", typeof(string));
    
            var catIDParameter = catID.HasValue ?
                new ObjectParameter("CatID", catID) :
                new ObjectParameter("CatID", typeof(int));
    
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("PROC_EditProduct", productIDParameter, quantityParameter, info_desParameter, status_desParameter, original_priceParameter, sell_priceParameter, productNameParameter, catIDParameter, userIDParameter);
        }
    
        public virtual int PROC_RemoveImagesByProductID(Nullable<int> productID)
        {
            var productIDParameter = productID.HasValue ?
                new ObjectParameter("ProductID", productID) :
                new ObjectParameter("ProductID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("PROC_RemoveImagesByProductID", productIDParameter);
        }
    
        public virtual int PROC_SetAccountStatus(Nullable<int> userID, Nullable<bool> isActive)
        {
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(int));
    
            var isActiveParameter = isActive.HasValue ?
                new ObjectParameter("IsActive", isActive) :
                new ObjectParameter("IsActive", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("PROC_SetAccountStatus", userIDParameter, isActiveParameter);
        }
    
        public virtual int PROC_UpdateCat(Nullable<int> catId, string catName)
        {
            var catIdParameter = catId.HasValue ?
                new ObjectParameter("CatId", catId) :
                new ObjectParameter("CatId", typeof(int));
    
            var catNameParameter = catName != null ?
                new ObjectParameter("CatName", catName) :
                new ObjectParameter("CatName", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("PROC_UpdateCat", catIdParameter, catNameParameter);
        }
    
        public virtual int PROC_UpdateUserInformation(Nullable<int> userId, string name, string username, string password, string phone, string address, Nullable<System.DateTime> birthdate)
        {
            var userIdParameter = userId.HasValue ?
                new ObjectParameter("UserId", userId) :
                new ObjectParameter("UserId", typeof(int));
    
            var nameParameter = name != null ?
                new ObjectParameter("Name", name) :
                new ObjectParameter("Name", typeof(string));
    
            var usernameParameter = username != null ?
                new ObjectParameter("Username", username) :
                new ObjectParameter("Username", typeof(string));
    
            var passwordParameter = password != null ?
                new ObjectParameter("Password", password) :
                new ObjectParameter("Password", typeof(string));
    
            var phoneParameter = phone != null ?
                new ObjectParameter("Phone", phone) :
                new ObjectParameter("Phone", typeof(string));
    
            var addressParameter = address != null ?
                new ObjectParameter("Address", address) :
                new ObjectParameter("Address", typeof(string));
    
            var birthdateParameter = birthdate.HasValue ?
                new ObjectParameter("Birthdate", birthdate) :
                new ObjectParameter("Birthdate", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("PROC_UpdateUserInformation", userIdParameter, nameParameter, usernameParameter, passwordParameter, phoneParameter, addressParameter, birthdateParameter);
        }
    
        public virtual int PROC_DeleteUser(Nullable<int> userID)
        {
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("PROC_DeleteUser", userIDParameter);
        }
    
        public virtual int PROC_UpdateProduct(Nullable<int> productID, Nullable<int> quantity, string info_des, string status_des, Nullable<double> original_price, Nullable<double> sell_price, string productName, Nullable<int> catID)
        {
            var productIDParameter = productID.HasValue ?
                new ObjectParameter("ProductID", productID) :
                new ObjectParameter("ProductID", typeof(int));
    
            var quantityParameter = quantity.HasValue ?
                new ObjectParameter("Quantity", quantity) :
                new ObjectParameter("Quantity", typeof(int));
    
            var info_desParameter = info_des != null ?
                new ObjectParameter("Info_des", info_des) :
                new ObjectParameter("Info_des", typeof(string));
    
            var status_desParameter = status_des != null ?
                new ObjectParameter("Status_des", status_des) :
                new ObjectParameter("Status_des", typeof(string));
    
            var original_priceParameter = original_price.HasValue ?
                new ObjectParameter("Original_price", original_price) :
                new ObjectParameter("Original_price", typeof(double));
    
            var sell_priceParameter = sell_price.HasValue ?
                new ObjectParameter("Sell_price", sell_price) :
                new ObjectParameter("Sell_price", typeof(double));
    
            var productNameParameter = productName != null ?
                new ObjectParameter("ProductName", productName) :
                new ObjectParameter("ProductName", typeof(string));
    
            var catIDParameter = catID.HasValue ?
                new ObjectParameter("CatID", catID) :
                new ObjectParameter("CatID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("PROC_UpdateProduct", productIDParameter, quantityParameter, info_desParameter, status_desParameter, original_priceParameter, sell_priceParameter, productNameParameter, catIDParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> TEST_PROC(Nullable<int> a)
        {
            var aParameter = a.HasValue ?
                new ObjectParameter("a", a) :
                new ObjectParameter("a", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("TEST_PROC", aParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> PROC_CreateProduct(Nullable<int> quantity, string info_des, string status_des, Nullable<double> original_price, Nullable<double> sell_price, string productName, Nullable<int> catID, Nullable<int> userID)
        {
            var quantityParameter = quantity.HasValue ?
                new ObjectParameter("Quantity", quantity) :
                new ObjectParameter("Quantity", typeof(int));
    
            var info_desParameter = info_des != null ?
                new ObjectParameter("Info_des", info_des) :
                new ObjectParameter("Info_des", typeof(string));
    
            var status_desParameter = status_des != null ?
                new ObjectParameter("Status_des", status_des) :
                new ObjectParameter("Status_des", typeof(string));
    
            var original_priceParameter = original_price.HasValue ?
                new ObjectParameter("Original_price", original_price) :
                new ObjectParameter("Original_price", typeof(double));
    
            var sell_priceParameter = sell_price.HasValue ?
                new ObjectParameter("Sell_price", sell_price) :
                new ObjectParameter("Sell_price", typeof(double));
    
            var productNameParameter = productName != null ?
                new ObjectParameter("ProductName", productName) :
                new ObjectParameter("ProductName", typeof(string));
    
            var catIDParameter = catID.HasValue ?
                new ObjectParameter("CatID", catID) :
                new ObjectParameter("CatID", typeof(int));
    
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("PROC_CreateProduct", quantityParameter, info_desParameter, status_desParameter, original_priceParameter, sell_priceParameter, productNameParameter, catIDParameter, userIDParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> PROC_AddProduct(Nullable<int> quantity, string info_des, string status_des, Nullable<double> original_price, Nullable<double> sell_price, string productName, Nullable<int> catID, Nullable<int> userID)
        {
            var quantityParameter = quantity.HasValue ?
                new ObjectParameter("Quantity", quantity) :
                new ObjectParameter("Quantity", typeof(int));
    
            var info_desParameter = info_des != null ?
                new ObjectParameter("Info_des", info_des) :
                new ObjectParameter("Info_des", typeof(string));
    
            var status_desParameter = status_des != null ?
                new ObjectParameter("Status_des", status_des) :
                new ObjectParameter("Status_des", typeof(string));
    
            var original_priceParameter = original_price.HasValue ?
                new ObjectParameter("Original_price", original_price) :
                new ObjectParameter("Original_price", typeof(double));
    
            var sell_priceParameter = sell_price.HasValue ?
                new ObjectParameter("Sell_price", sell_price) :
                new ObjectParameter("Sell_price", typeof(double));
    
            var productNameParameter = productName != null ?
                new ObjectParameter("ProductName", productName) :
                new ObjectParameter("ProductName", typeof(string));
    
            var catIDParameter = catID.HasValue ?
                new ObjectParameter("CatID", catID) :
                new ObjectParameter("CatID", typeof(int));
    
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("PROC_AddProduct", quantityParameter, info_desParameter, status_desParameter, original_priceParameter, sell_priceParameter, productNameParameter, catIDParameter, userIDParameter);
        }
    }
}
