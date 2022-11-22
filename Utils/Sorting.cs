using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static VersionChecker.Models.ResourceModel;
using static VersionChecker.Models.Lists;
using VersionChecker.Models;

namespace VersionChecker.Utils
{
    internal class Sorting
    {
        public static List<Resource> SortResources(List<Resource> resources)
        {
            Lists lists = new Lists () {
                invalid = new List<Resource> { },
                outdated = new List<Resource> { },
                upToDate = new List<Resource> { },
            };
            
            foreach (Resource resource in resources)
            {
                if (resource.versions != null && resource.versions.current != null && resource.versions.local != null)
                {
                    if (resource.versions.current == resource.versions.local)
                    {
                        lists.upToDate.Add(resource);
                    }
                    else
                    {
                        lists.outdated.Add(resource);
                    }
                }
                else
                {
                    lists.invalid.Add(resource);
                }
            }

            return lists.upToDate.Concat(lists.invalid).Concat(lists.outdated).ToList();
        }
    }
}
