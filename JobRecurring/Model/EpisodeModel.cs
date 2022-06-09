using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobRecurring.Model
{
   public class EpisodeModel
    {
        public int? EpisodeNumber { get; set; }
        public string Link { get; set; }
        public string Name { get; set; }
        public int? SupplierId { get; set; }
        public string VideoId { get; set; }
    }
}
