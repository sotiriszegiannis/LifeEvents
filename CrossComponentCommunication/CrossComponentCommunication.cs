using System.Linq;

namespace CrossComponentCommunication
{
    public class CrossComponentCommunication<T>
    {
        List<(BroadcastTypeEnum broadcastType,Action<T> listener)> Listeners { get; set; } = new List<(BroadcastTypeEnum broadcastType, Action<T> listener)> ();
        public void Broadcast(BroadcastTypeEnum broadcastType,T message)
        {
            try
            {
                Listeners
                        .Where(p => p.broadcastType == broadcastType)
                        .ToList()
                        .ForEach(p =>
                        {
                            p.listener(message);
                        });
            }
            catch (Exception ex) {
                throw ex;
            }            
        }
        public void ListenerTo(BroadcastTypeEnum broadcastType,Action<T> listener) => Listeners.Add((broadcastType,listener));

    }
}
