using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SekiroModManager
{
    class ExceptionHandler
    {
        public static void ShowError(Exception e,string d)
        {
            MessageBox.Show(d + "\n" + e.Message + "\n" + e.StackTrace, "错误"
                ,MessageBoxButtons.OK,MessageBoxIcon.Error);
            Console.WriteLine(e.Message + "\n" + e.StackTrace);
        }
    }
}
