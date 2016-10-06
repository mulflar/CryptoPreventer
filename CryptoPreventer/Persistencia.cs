namespace CryptoPreventer
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.ObjectModel;
    using System.IO;

    class Persistencia
    {
        string ruta;
        public ObservableCollection<string> lista;


        public Persistencia()
        {
            ruta = AppDomain.CurrentDomain.BaseDirectory + "datos.json";
            lista = new ObservableCollection<string>();
            if (File.Exists(ruta))
            {
                lista = JsonConvert.DeserializeObject<ObservableCollection<string>>(File.ReadAllText(ruta));
            }
        }

        public void Store(ObservableCollection<string> lista)
        {
            using (StreamWriter file = File.CreateText(@ruta))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, lista);
            }
        }
    }
}
