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
    
    public partial class EpisodeSource
    {
        public int Id { get; set; }
        public Nullable<int> ProductEpisodeId { get; set; }
        public Nullable<int> SupplierId { get; set; }
        public string Link { get; set; }
        public Nullable<bool> IsIframe { get; set; }
        public string VideoId { get; set; }
        public Nullable<System.DateTime> CreateOn { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdateOn { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public string LinkTemp { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public string SubLink { get; set; }
    
        public virtual Episode Episode { get; set; }
    }
}
