namespace Playground
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            var info=TimeZoneInfo.FindSystemTimeZoneById("Europe/Athens");
            InitializeComponent();
        }
    }

    public class Parent
    {
        public IChild AChild { get; set; }
        public void Save()
        {
            AChild.Save();
        }
    }
    public interface IChild
    {
        bool Save();
    }
    public class Child:IChild
    {
        public bool Save()
        {
            Console.WriteLine("saved");
            return true;
        }
    }
}