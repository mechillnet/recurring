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
    
    public partial class Topic
    {
        public int Id { get; set; }
        public Nullable<bool> IncludeInTopMenu { get; set; }
        public Nullable<bool> Password { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Script { get; set; }
        public string Css { get; set; }
        public bool Published { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }
        public Nullable<int> Order { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
    }
}
