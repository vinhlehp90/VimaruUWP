using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VimaruUWP.Models
{
    public class NamHoc
    {
        public string Ten { get; set; }
        public string TB { get; set; }
        public List<MonHoc> DSMonHoc { get; set; }
        public NamHoc()
        {
            DSMonHoc = new List<MonHoc>();
        }
    }
}
