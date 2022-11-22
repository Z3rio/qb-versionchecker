using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static VersionChecker.Models.ResourceModel;

namespace VersionChecker.Models
{
    public class Lists
    {
        public List<Resource>? invalid { get; set; }
        public List<Resource>? outdated { get; set; }
        public List<Resource>? upToDate { get; set; }
    }
}
