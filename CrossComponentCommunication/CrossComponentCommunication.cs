using System.Linq;

namespace CrossComponentCommunication
{
    public class CrossComponentCommunication
    {
        List<(BroadcastTypeEnum broadcastType,Action<IBroadcastMessage> listener)> Listeners { get; set; } = new List<(BroadcastTypeEnum broadcastType, Action<IBroadcastMessage> listener)> ();
        public void Broadcast(BroadcastTypeEnum broadcastType, IBroadcastMessage message)
        {
            try
            {
                Listeners
                        .Where(p => p.broadcastType == broadcastType)
                        .ToList()
                        .ForEach(async p =>
                        {
                            p.listener(message);
                        });
            }
            catch (Exception ex) {
                throw ex;
            }            
        }
        public void ListenTo(BroadcastTypeEnum broadcastType,Action<IBroadcastMessage> listener) => Listeners.Add((broadcastType,listener));

    }
}
