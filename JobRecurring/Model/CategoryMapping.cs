//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JobRecurring.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class CategoryMapping
    {
        public int Id { get; set; }
        public Nullable<int> ProductId { get; set; }
        public Nullable<int> CategoryId { get; set; }
    
        public virtual Category Category { get; set; }
        public virtual Movie Movie { get; set; }
    }
}
