using SFML.Graphics;

namespace Play
{
    class Program
    {
        [System.STAThread​Attribute]
        static void Main()
        {
            MainForm.GetPlayMainForm().ShowDialog();
        }
    }
}