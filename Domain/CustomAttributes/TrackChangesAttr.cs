using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class TrackChangesAttr : Attribute
    {
        /// <summary>
        /// The auto update is being done by the base repository.If false must be done manually through the code. 
        /// </summary>
        public bool AutoUpdate { get; set; } = true;
        public TrackChangesAttr() { }
        public TrackChangesAttr(bool autoUpdate)
        {
            this.AutoUpdate = autoUpdate;
        }
    }
}
