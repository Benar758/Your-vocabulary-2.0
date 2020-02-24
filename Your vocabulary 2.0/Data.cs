using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows;

namespace Your_vocabulary_2._0
{
    /// <summary>
    /// Класс для работы с данными
    /// </summary>
    static class Data
    {
        /// <summary>
        /// Сохраняет данные
        /// </summary>
        public static void Save()
        {
            string json = JsonConvert.SerializeObject(MainWindow.Dictionary);

            File.WriteAllText("data.json", json);
        }

        /// <summary>
        /// Загружает данные
        /// </summary>
        public static void Load()
        {
            if (!File.Exists("data.json")) File.Create("data.json").Dispose();
            string json = File.ReadAllText("data.json");

            if (!string.IsNullOrEmpty(json))
            MainWindow.Dictionary = JsonConvert.DeserializeObject<Dictionary>(json);
        }
    }
}
