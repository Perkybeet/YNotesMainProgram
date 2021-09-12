using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using ynotes;



namespace blocnotas
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)


        {
            //MessageBox.Show(args.Length.ToString());

            if (args.Length != 0)
            {

                if (File.Exists(args[0]) == true)
                {

                    StreamReader reader = new StreamReader(args[0]);
                    General.oTexto = reader.ReadToEnd();

                    reader.Close();

                    General.oFilename = args[0];

                }
                else
                {
                    MessageBox.Show("   Los argumentos especificados no son válidos.\n\n   >>> " + args[0],"Error",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    return;
                    
                }
            }

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }
    }
}
