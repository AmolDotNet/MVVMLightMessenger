//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MVVMLightMessengerDemo.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProductSize
    {
        public ProductSize()
        {
            this.OrderItems = new HashSet<OrderItem>();
        }
    
        public int Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal PremiumPrice { get; set; }
        public decimal ToppingPrice { get; set; }
        public string IsGultenFree { get; set; }
    
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
