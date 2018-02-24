using System;
using System.Threading;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace Test
{
   


    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.DoEvents();
            Application.Run(new MainForm());
        }
    }
}

