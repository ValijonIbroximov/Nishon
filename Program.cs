using System;
using System.Windows.Forms;

namespace FaceDBApp   // 🔹 bu yerda MultiFaceRec emas
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // FrmPrincipal formangizni ishga tushiramiz
            Application.Run(new FrmPrincipal());
        }
    }
}
