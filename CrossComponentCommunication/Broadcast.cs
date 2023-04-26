namespace CrossComponentCommunication
{
    public abstract class Broadcast<T>
    {
        virtual public T Message { get; private set; }
    }
}
