namespace CryptoPreventer
{
    using System.Diagnostics;
    using System.IO;
    using System.Security.Permissions;
    using System.Windows;
    class Watcher
    {
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public Watcher(string path, string type)
        {

            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.IncludeSubdirectories = true;
            watcher.Path = path;

            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            watcher.Filter = type;

            watcher.Created += new FileSystemEventHandler(Shutdown);

            watcher.Changed += new FileSystemEventHandler(Shutdown);

            watcher.Renamed += new RenamedEventHandler(Shutdown);
            // Begin watching.
            watcher.EnableRaisingEvents = true;
        }
        private void Shutdown(object source, FileSystemEventArgs e)
        {
            Process.Start("shutdown", "/s /t 0");
            Application.Current.Shutdown();
        }
    }
}
