using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Optical;

namespace Optical.Administer
{
    public interface IManager
    {
        public void Save();

        public void Edit(IOptical oldIOptical, IOptical newIOptical);

        public void Add(IOptical optical1, IOptical optical2);

        public void Remove(IOptical optical);

    }
    public sealed class Manager 
    {
        public void Add(IOptical optical1, IOptical optical2)
        {
             
        }

        public void Edit(IOptical oldIOptical, IOptical newIOptical)
        {
            
        }

        public void Remove(IOptical optical)
        {
             
        }

        public void Save()
        {
           
        }
    }
}
