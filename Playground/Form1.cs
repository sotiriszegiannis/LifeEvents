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
}