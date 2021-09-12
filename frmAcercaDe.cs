using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;

namespace blocnotas
{
    public partial class frmAcercaDe : Form
    {
        public static string GetAssemblyFileVersion()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fileVersion = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fileVersion.FileVersion;
        }

        public frmAcercaDe()
        {
            InitializeComponent();
        }

        private void lblLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("cmd", $"/c start https://raw.githubusercontent.com/Perkybeet/YNotes/main/LICENSE%3A%20Ms.SS") { CreateNoWindow = true });
        }

        private void frmAcercaDe_Load(object sender, EventArgs e)
        {
            string version = GetAssemblyFileVersion();
            string[] versionActual = version.Split(".");

            label1.Text = Application.ProductName.ToString() + "\n\n" + "Versión " + versionActual [0] + "." + versionActual[1] + "." + versionActual[2] + " ©2021 Yago López Prado" + "\n\n" + "Más información acerca de la licencia de este producto en";


        }
    }
}
