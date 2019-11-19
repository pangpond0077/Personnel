using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Personnel.Models
{
    public class CheckRowInOut
    {
        public int ID { get; set; }
        public string Site { get; set; }
        public int LastRow { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
