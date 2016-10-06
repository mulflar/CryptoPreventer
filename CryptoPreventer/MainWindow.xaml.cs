using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace CryptoPreventer
{
    public partial class MainWindow : Window
    {
        RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        ObservableCollection<string> rutas = new ObservableCollection<string>();
        Persistencia datos;
        public ObservableCollection<string> Rutas
        {
            get { return rutas; }
            set { rutas = value; }
        }
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            datos = new Persistencia();

            System.Windows.Forms.NotifyIcon ni = new System.Windows.Forms.NotifyIcon();
            ni.Icon = new System.Drawing.Icon("favicon.ico");
            ni.Visible = true;
            ni.DoubleClick +=
                delegate (object sender, EventArgs args)
                {
                    this.Show();
                    this.WindowState = WindowState.Normal;
                };
            if (datos.lista.Count > 0)
            {
                Rutas = datos.lista;
                Iniciar();
            }
            if (rkApp.GetValue("CriptoPreventer") == null)
                checkBox.IsChecked = false;
            else
                checkBox.IsChecked = true;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            CommonFileDialogResult result = dialog.ShowDialog();
            if (result.ToString() == "Ok")
            {
                Rutas.Add(dialog.FileName.ToString());
            }
        }
        private void iniciar_Click(object sender, RoutedEventArgs e)
        {
            datos.Store(rutas);
            Iniciar();
        }
        private void Iniciar()
        {
            foreach (var item in Rutas)
            {
                new Watcher(item, "*.encrypted");
                new Watcher(item, "*.cryptolocker");
            }
            this.Hide();
        }

        private void checkBox_Click(object sender, RoutedEventArgs e)
        {
            string BaseDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            if (checkBox.IsChecked==true)
            {
                rkApp.SetValue("CriptoPreventer", BaseDir);
            }
            else
            {
                rkApp.DeleteValue("CriptoPreventer", false);
            }
        }
    }
}
