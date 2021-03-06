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
    
    public partial class Episode
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Episode()
        {
            this.EpisodeSources = new HashSet<EpisodeSource>();
        }
    
        public int Id { get; set; }
        public int EpisodeNumber { get; set; }
        public Nullable<bool> Status { get; set; }
        public int ProductId { get; set; }
        public Nullable<int> ViewNumber { get; set; }
        public Nullable<System.DateTime> CreateOn { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateOn { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public Nullable<int> Type { get; set; }
        public string Keyword { get; set; }
        public string FullLink { get; set; }
    
        public virtual Movie Movie { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EpisodeSource> EpisodeSources { get; set; }
    }
}
