//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            this.Images = new HashSet<Image>();
            this.OrderDetails = new HashSet<OrderDetail>();
            this.WishItems = new HashSet<WishItem>();
        }

        public string GetPreviewImage
        {
            get
            {
                // Get first image
                if (Images.Count > 0)
                {
                    return Images.FirstOrDefault().ImageURL;
                }
                return "https://via.placeholder.com/150";

            }
            set {; }
        }


        public string ShowSellPrice
        {
            get
            {
                return this.Sell_price.ToString("C3", CultureInfo.CreateSpecificCulture("vi-VN"));
            }
            set
            {
                ;
            }
        }

        public string ShowOriginalPrice
        {
            get
            {
                return this.Original_price.ToString("C3", CultureInfo.CreateSpecificCulture("vi-VN"));
            }
            set
            {
                ;
            }
        }


        public string ShowQuantityColor
        {
            get
            {
                if (Quantity > 0)
                {
                    return "Green";
                }
                return "Red";
            }
        }

        public string ShowQuantity
        {
            get
            {
                if (Quantity > 0)
                {
                    return Quantity + " items";
                }

                return "Out of stock";

            }
        }

        public string ShowUploadedDate
        {
            get
            {
                // Get first date and convert into "2 days ago", "1 week ago", "1 month ago", "1 year ago", ...
                TimeSpan timeSpan = DateTime.Now - UploadedDate;
                if (timeSpan.TotalDays < 1)
                {
                    return "Today";
                }
                else if (timeSpan.TotalDays < 2)
                {
                    return "Yesterday";
                }
                else if (timeSpan.TotalDays < 7)
                {
                    return timeSpan.Days + " days ago";
                }
                else if (timeSpan.TotalDays < 14)
                {
                    return "1 week ago";
                }
                else if (timeSpan.TotalDays < 30)
                {
                    return timeSpan.Days / 7 + " weeks ago";
                }
                else if (timeSpan.TotalDays < 60)
                {
                    return "1 month ago";
                }
                else if (timeSpan.TotalDays < 365)
                {
                    return timeSpan.Days / 30 + " months ago";
                }
                else if (timeSpan.TotalDays < 730)
                {
                    return "1 year ago";
                }
                else
                {
                    return timeSpan.Days / 365 + " years ago";
                }
            }
            set {; }
        }

        public string OwnerName
        {
            get
            {
                return this.User.Name;
            }

            set
            {
                ;
            }
        }

        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public string Info_des { get; set; }
        public string Status_des { get; set; }
        public double Original_price { get; set; }  
        public double Sell_price { get; set; }
        public System.DateTime UploadedDate { get; set; }
        public string ProductName { get; set; }
        public Nullable<int> CatID { get; set; }
        public Nullable<int> UserID { get; set; }
        public int View_count { get; set; }
    
        public virtual Category Category { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Image> Images { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WishItem> WishItems { get; set; }
    }
}
