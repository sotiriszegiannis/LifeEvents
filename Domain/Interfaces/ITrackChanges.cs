using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    /// <summary>
    /// This interface is responsible for tracking db changes on a record. A db table that implementes this interface, is being tracked by the system on every change that 
    /// the db record is experiencing. Every time it is saved, the ChangeCounter is being increased by one. This way we now that it has been changed.
    /// Is being updated through the code. Originally it was being updated by a sql trigger.
    /// </summary>
    public interface ITrackChanges
    {
        long? ChangeCounter { get; set; }
    }
}
