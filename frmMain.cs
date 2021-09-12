using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Drawing.Text;
using System.Net;
using System.Net.Cache;
using System.Reflection;


namespace blocnotas
{
    public partial class frmMain : Form
    {
        //Variables generales
        bool DocumentoModificado = false;
        bool DocumentoGuardado = false;

        string Filename = "";
 
        public frmMain()
        {
            InitializeComponent();

            if (ynotes.General.oFilename != "") 
            {

                txtMain.Text = ynotes.General.oTexto;
                Filename = ynotes.General.oFilename;

                this.Text = this.Tag + " - [" + Filename + "]";

                DocumentoGuardado = true;
            }

        }

        //Hace comprobaciones antes de salir del programa
        private void frmMain_Unload(object sender, CancelEventArgs e)
        {
            if (DocumentoModificado == true)
            {
                switch (MessageBox.Show("El documento ha sido modificado.\n\n¿Desea guardar los cambios antes de salir?", "Cambios realizados", MessageBoxButtons.YesNoCancel))
                {
                    case DialogResult.Yes:
                        mnuGuardar.PerformClick();

                        if (DocumentoGuardado == false)
                        {
                            e.Cancel = true;
                        }

                        break;
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                    case DialogResult.No:
                        break;
                }


            }
        }

        //Menu Archivo
        private void mnuNuevo_Click(object sender, EventArgs e)
        {

            if (DocumentoModificado == true)
            {
                switch (MessageBox.Show("El documento ha sido modificado.\n\n¿Desea guardar los cambios antes de crear uno nuevo?", "Cambios realizados", MessageBoxButtons.YesNoCancel))
                {
                    case DialogResult.Yes:
                        mnuGuardar.PerformClick();

                        if (DocumentoGuardado == true)
                        {
                            txtMain.Clear();
                            DocumentoModificado = false;

                            Filename = "";
                        }
                        
                        break;

                    case DialogResult.Cancel:
                        break;

                    case DialogResult.No:
                        txtMain.Clear();
                        DocumentoModificado = false;
                        Filename = "";
                        break;
                }

            }
            else
            {
                txtMain.Clear();
                DocumentoModificado = false;
                Filename = "";

                this.Text = this.Tag + " - [Documento no guardado]";

            }
        }

        private void mnuVentanaNueva_Click(object sender, EventArgs e)
        {
            frmMain frmMain = new frmMain();
            frmMain.Show();
        }

        private void mnuAbrir_Click(object sender, EventArgs e)
        {
            //Falta implementar comprobación de documento modificado,
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {


                StreamReader reader = new StreamReader(openFileDialog.FileName);
                txtMain.Text = reader.ReadToEnd();
                reader.Close();

                Filename = openFileDialog.FileName;


                this.Text = this.Tag + " - [" + Filename + "]";


                DocumentoModificado = false;
            }
        }

        private void mnuGuardar_Click(object sender, EventArgs e)
        {


            try
            {
                DocumentoGuardado = false;

                if (Filename != "")
                {

                    //Especifica la ruta y nombre de archivo la clase de streamwriter
                    StreamWriter Fichero = new StreamWriter(Filename);

                    //Escribe el texto
                    Fichero.Write(txtMain.Text);

                    //ciera la clase stream writer
                    Fichero.Close();

                    //Comentamos que el documento fue guardado
                    DocumentoGuardado = true;
                }

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {

                    //Especifica la ruta y nombre de archivo la clase de streamwriter
                    StreamWriter Fichero = new StreamWriter(saveFileDialog.FileName);

                    //Escribe el texto
                    Fichero.Write(txtMain.Text);

                    //ciera la clase stream writer
                    Fichero.Close();

                    Filename = saveFileDialog.FileName;

                    this.Text = this.Tag + " - [" + Filename + "]";

                    //Comentamos que el documento fue guardado
                    DocumentoGuardado = true;
                }
                else { DocumentoGuardado = false; }
            }
            catch (Exception a)
            {
                MessageBox.Show("El documento no se ha podido guardar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DocumentoGuardado = false;

            }

        }

        private void mnuGuardarComo_Click(object sender, EventArgs e)
        {


            try
            {

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {

                    //Pass the filepath and filename to the StreamWriter Constructor
                    StreamWriter Fichero = new StreamWriter(saveFileDialog.FileName);
                    //Write a line of text
                    Fichero.Write(txtMain.Text);
                    //Write a second line of text
                    Fichero.Close();

                    Filename = saveFileDialog.FileName;

                    this.Text = this.Tag + " - [" + Filename + "]";
                }
            }
            catch (Exception)
            {
               MessageBox.Show ("Ha ocurrido un error al guardar el documento. ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void mnuConfigurarPagina_Click(object sender, EventArgs e)
        {
            pageSetup.PrinterSettings = new PrinterSettings();
            pageSetup.PageSettings = new PageSettings();

            if (pageSetup.ShowDialog() == DialogResult.OK)
            {
                object[] results = new object[]{

                pageSetup.PageSettings.Margins,
                pageSetup.PageSettings.PaperSize,
                pageSetup.PageSettings.Landscape,
                pageSetup.PrinterSettings.PrinterName,
                pageSetup.PrinterSettings.PrintRange};

                for (int i = 0; i < results.Length; i++)
                {
                    txtMain.Text += results[i].ToString() + "\n";
                }

            }




        }
        //Realiza una busqueda en Google con el texto selecionado
        private void mnuBuscarInternet_Click(object sender, EventArgs e)
        {

            if (txtMain.SelectedText.Trim() != "")
            {

                //Asigna a la variable target la cadena de la búsqueda
                string target = "www.google.com/search?q=" + txtMain.SelectedText;

                //Reemplaza los espacios por signo '+', para que Google entienda la búsqueda
                target = target.Replace(" ", "+");

                //Inicia el comando de DOS, CMD.EXE y a su vez este inicia el programa Start.exe que se
                //encarga de abrir el programa asociado al protocolo http
                Process.Start(new ProcessStartInfo("cmd", $"/c start {target}") { CreateNoWindow = true });


            }
        }

        //Activa o desactiva el ajuste de linea
        private void mnuAjusteLinea_Click(object sender, EventArgs e)
        {
            if (mnuAjusteLinea.Checked == true)
            {
                txtMain.WordWrap = false;
                mnuAjusteLinea.Checked = false;
            }
            else
            {
                txtMain.WordWrap = true;
                mnuAjusteLinea.Checked = true;
            }
        }

        //Cambia la tipografía del texto
        private void mnuFuente_Click(object sender, EventArgs e)
        {

            fontDialog.Font = txtMain.Font;
            fontDialog.Color = txtMain.ForeColor;

            if (fontDialog.ShowDialog() == DialogResult.OK) {

                txtMain.Font = fontDialog.Font;
                txtMain.ForeColor = fontDialog.Color;

            }
        }

        //Muestra u oculta la barra de estado
        private void mnuBarraEstado_Click(object sender, EventArgs e)
        {

            if (mnuBarraEstado.Checked == true)
            {
                mnuBarraEstado.Checked = false;
                statusMain.Visible = false;
            }
            else
            {
                mnuBarraEstado.Checked = true;
                statusMain.Visible = true;
            }
        }

        private void mnuSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void mnuDeshacer_Click(object sender, EventArgs e)
        {

            txtMain.Undo();

        }

        private void mnuCortar_Click(object sender, EventArgs e)
        {

            txtMain.Cut();

        }

        private void mnuCopiar_Click(object sender, EventArgs e)
        {

            txtMain.Copy();

        }

        private void mnuPegar_Click(object sender, EventArgs e)
        {

            txtMain.Paste();

        }

        public static string GetAssemblyFileVersion()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fileVersion = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fileVersion.FileVersion;
        }

        private void mnuCheckUpdates_Click(object sender, EventArgs e)
        {
            //Declara la clase WebClient para descarga de datos
            WebClient chkUpdates = new WebClient();


            //Declara la clase que define el uso que se le dará a la caché de datos
            RequestCachePolicy iCache = new RequestCachePolicy(RequestCacheLevel.Reload);

            //Declara variables
            byte[] updData;// Almacen temporal de la informacion
            string lData = "";// Concatenacion de los datos en updData
            string MainVersion = "";// Version principal
            string SubVersion = "";// Sub version
            string CompilationVersion = "";// Ensamblado
            string[] FileList;//lista de archivos descargar

            //Aplica la politica de uso de la cache de datos a la clase que va a manejar la descarga
            chkUpdates.CachePolicy = iCache;

            //Establece la dirección del archivo a leer y lo lee

            //Primero descarga el número de versión
            ///////////////////////////////////////////


            try
            {
                updData = chkUpdates.DownloadData("https://raw.githubusercontent.com/Perkybeet/updates/main/ynotes.upd");
            }
            catch (Exception)
            {

                MessageBox.Show("No se ha podido conectar con el servidor. Porfavor, revise su conexión a internet y vuelva a intentarlo mas tarde.", "Error de conexión:", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //Recorre la longitud de la matriz updData leyendo todos los valores y los une en la variable lData
            //Los datos devueltos por updData son los códigos en ASCII de los caracteres, por lo tanto antes de
            //unir los convierte a String
            for (int i = 0; i < updData.Length; i++)
            {
                lData += char.ConvertFromUtf32(updData[i]);

            }



            //Separa la información descargada según nuestras preferencias
            MainVersion = lData.Substring(0, 2);
            SubVersion = lData.Substring(3, 2);
            CompilationVersion = lData.Substring(6, 4);




            //Empieza la comprobación de ambas versiones
            string version = GetAssemblyFileVersion();
            string[] versionActual = version.Split(".");
            bool update = false;

            if (Int32.Parse(MainVersion) == Int32.Parse(versionActual[0]))
            {
                if (Int32.Parse(SubVersion) == Int32.Parse(versionActual[1]))
                {
                    if (Int32.Parse(CompilationVersion) == Int32.Parse(versionActual[2]))
                    {
                        update = false;
                    }
                    else //Compilacion distinta
                    {
                        if (Int32.Parse(CompilationVersion) > Int32.Parse(versionActual[2])) { update = true; } else { update = false; }
                    }

                }
                else//Subversion distinta
                {
                    if (Int32.Parse(SubVersion) > Int32.Parse(versionActual[1])) { update = true; } else { update = false; }
                }
            }
            else//Version distinta
            {
                if (Int32.Parse(MainVersion) > Int32.Parse(versionActual[0])) { update = true; } else { update = false; }
            }

            if (update == false)
            {
                MessageBox.Show("Está usando la última versión disponible.\n\nVersión instalada: " + versionActual[0] + "." + versionActual[1] + "." + versionActual[2] + "\n\nVersion en el servidor: " + MainVersion + "." + SubVersion + "." + CompilationVersion, "Actualizado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                if (MessageBox.Show("Hay disponible una nueva versión.\n\n¿Quiere descargarla?\n\nVersión instalada: " + versionActual[0] + "." + versionActual[1] + "." + versionActual[2] + "\n\nVersión en el servidor: " + MainVersion + "." + SubVersion + "." + CompilationVersion, "Nueva versión disponible", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.No)
                {

                    return;
                }
                else
                {


                    //Segundo descarga la lista de archivos
                    ///////////////////////////////////////////

                    //reiniciamos el valor de ambas variables
                    lData = "";
                    Array.Clear(updData, 0, updData.Length);
                    

                    try
                    {
                        updData = chkUpdates.DownloadData("https://raw.githubusercontent.com/Perkybeet/updates/main/ynotes.list");
                    }
                    catch (Exception)
                    {

                        MessageBox.Show("No se podido descargar la lista de archivos, revise su conexión a internet o vuelva a intentarlo mas tarde.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;

                    }


                    //Recorre la longitud de la matriz updData leyendo todos los valores y los une en la variable lData
                    //Los datos devueltos por updData son los códigos en ASCII de los caracteres, por lo tanto antes de
                    //unir los convierte a String
                    for (int i = 0; i < updData.Length; i++)
                    {
                        lData += char.ConvertFromUtf32(updData[i]);

                    }



                    //Separa la información descargada según nuestras preferencias
                    FileList = lData.Split("\n");

                    //Corrige la ruta si es necesario añadiendo \ al final
                    string CopyPath = Application.StartupPath; //Almacen para la ruta

                    if (CopyPath.EndsWith(char.ConvertFromUtf32(92)) == false)
                    {
                        //MessageBox.Show(CopyPath.EndsWith(char.ConvertFromUtf32(92)).ToString() + "\n\nOriginal: " + CopyPath + "\n\nModificado: " + CopyPath + char.ConvertFromUtf32(92).ToString());
                        CopyPath += char.ConvertFromUtf32(92);

                    }

                    //MessageBox.Show(FileList.Length.ToString());

                    pbData.Visible = true;
                    Estado("Descargando...");
                    pbData.Minimum = 0;
                    pbData.Maximum = FileList.Length;


                    for (int i = 0; i < FileList.Length; i++)
                    {


                        try
                        {
                            // AÑADIR CODIGO ELIMINACION PREVIAS De EXISTENTES
                            //
                            // OJO FALTA CODIGO
                            //////////////////////////////////////////////////

                            pbData.Value = i + 1;
                            Application.DoEvents();
                            Estado("Descargando (" + FileList[i].Trim() + ")...");
                            chkUpdates.DownloadFile("https://raw.githubusercontent.com/Perkybeet/updates/main/" + FileList[i].Trim(), FileList[i].Trim());
                            System.Threading.Thread.Sleep(100);
                            //MessageBox.Show("Descargado:\n\n" + "https://raw.githubusercontent.com/Perkybeet/updates/main/" + FileList[i].Trim());
                        }
                        catch (Exception error)
                        {
                            MessageBox.Show(error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }



                    }//Aqui finaliza la descarga de archivos

                    pbData.Visible = false;
                    Estado("");


                    try
                    {
                        //MessageBox.Show("'" + CopyPath + FileList[FileList.Length - 1].Trim() + "'");
                        Process.Start(new ProcessStartInfo(CopyPath + FileList[FileList.Length - 1].Trim()) { CreateNoWindow = false });
                        Application.Exit();
                    }
                    catch (Exception error)
                    {

                        MessageBox.Show("No se ha podido llevar a cabo la actualización.\n\n" + error.Message);
                        return;

                    }

                }
            }
        }

        void Estado(string texto){
            if (texto.Trim() == "")
            {
                statusTexto.Text = "©2021 Programado por Yago López Prado";
            }
            else
            {
                statusTexto.Text = texto;
            }

            Application.DoEvents();
        }

        private void mnuAcercaDe_Click(object sender, EventArgs e)
            {

                frmAcercaDe frmAcercaDe = new frmAcercaDe();
                frmAcercaDe.ShowDialog();
            
            }

        private void btAcercade_Click(object sender, EventArgs e)
        {

            mnuAcercaDe.PerformClick();

        }

        private void txtMain_TextChanged(object sender, EventArgs e)
        {
            DocumentoModificado = true;

            if (Filename == "") { this.Text = this.Tag + " - [Documento no guardado]"; }
            
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            // Carga nombres de fuentes
            InstalledFontCollection installedFonts = new InstalledFontCollection();
            foreach (FontFamily fontFamily in installedFonts.Families)
            {
                cbFtuente.Items.Add(fontFamily.Name);
            }

            // Select the first font.
            cbFtuente.SelectedIndex = 0;
        }

        private void menuMain_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void icoNuevaVentana_Click(object sender, EventArgs e)
        {
            mnuVentanaNueva.PerformClick();
        }

        private void icoNuevoArchivo_Click(object sender, EventArgs e)
        {

            mnuNuevo.PerformClick();

        }

        private void icoGuardar_Click(object sender, EventArgs e)
        {
            mnuGuardar.PerformClick();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(openFileDialog.FileName);
        }
    }
}
