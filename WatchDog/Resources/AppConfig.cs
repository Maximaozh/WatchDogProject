using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Text.Json;

namespace WatchDog
{
    class AppConfig
    {
        public string BOT_TOKEN { get; set; }
        public string CHAT_ID { get; set; }
               
        public string CONNECTION_STRING { get; set; }
        public string FILE_SAVE_NAME { get; set; }

        public void Load(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    var json = File.ReadAllText(filePath);
                    var config = JsonSerializer.Deserialize<AppConfig>(json);
                    BOT_TOKEN = config.BOT_TOKEN;
                    CHAT_ID = config.CHAT_ID;
                    CONNECTION_STRING = config.CONNECTION_STRING;
                    FILE_SAVE_NAME = config.FILE_SAVE_NAME;
                }
                else
                {
                    Console.WriteLine($"Файл не найден: {filePath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке: {ex.Message}");
            }
        }

        public void Save(string filePath)
        {
            try
            {
                var json = JsonSerializer.Serialize<AppConfig>(this);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении: {ex.Message}");
            }
        }
    }
}
