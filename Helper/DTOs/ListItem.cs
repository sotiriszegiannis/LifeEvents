namespace Helper  
{
    public class ListItem<T,K> where K:class
    {
        public ListItem() { }
        public ListItem(T key,string value,K extraInfo=null)
        {
            Key = key;
            Text = value;
            ExtraInfo = extraInfo;
        }
        public T Key { get; set; }
        public string Text { get; set; }
        public K ExtraInfo { get; set; }
    }
}
