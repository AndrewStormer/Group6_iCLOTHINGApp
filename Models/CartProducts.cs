//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Group6_iCLOTHINGApp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CartProducts
    {
        public int cartID { get; set; }
        public int productID { get; set; }
        public int cartProductID { get; set; }
        public int productQuantity { get; set; }
    
        public virtual Products Products { get; set; }
        public virtual ShoppingCarts ShoppingCarts { get; set; }
    }
}
