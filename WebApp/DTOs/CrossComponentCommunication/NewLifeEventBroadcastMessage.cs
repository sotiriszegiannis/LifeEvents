using CrossComponentCommunication;

namespace WebApp
{
    public class NewLifeEventBroadcastMessage:IBroadcastMessage
    {
        public int LifeEventId { get; set; }
        public NewLifeEventBroadcastMessage(int lifeEventId)
        {
            LifeEventId = lifeEventId;
        }
    }
}
