namespace Helper  
{
    public class ListItem<T,K>
    {
        public T Key { get; set; }
        public string Value { get; set; }
        public K ExtraInfo { get; set; }
    }
}
