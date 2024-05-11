using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoomboxController_Content
{
    internal class Configs
    {
        public ConfigEntry<bool> requstbattery;
        public ConfigEntry<bool> pocketitem;
        public ConfigEntry<bool> radiuscheck;
        public ConfigEntry<string> languages;
        public ConfigEntry<bool> visual;
        public ConfigEntry<string> body;
        public ConfigEntry<string> otherelem;

        public static Lang lang = new Lang();

        public Configs()
        {
            var customFile = new ConfigFile(@"BoomboxController\boombox_controller.cfg", true);
            requstbattery = customFile.Bind("General.Toggles", "RequestBattery", false, "Enable/disable boombox battery (true = Enable; false = Disable)");
            //pocketitem = customFile.Bind("General.Toggles", "PocketItem", true, "Enable/disable music in your pocket. (true = Enable; false = Disable)");
            //radiuscheck = customFile.Bind("General.Toggles", "RadiusUse", true, "Enable/disable the command radius of the boombox. (true = Radius greater than 25; false = Standard radius 25)");
            //languages = customFile.Bind("General", "Languages", "en", "EN/RU");
            //visual = customFile.Bind("Visual", "Visual", false, "Enable/Disable Visual Elements of Boombox");
            //body = customFile.Bind("Visual", "Body", "#FFFFFF", "Color body Boombox");
            //otherelem = customFile.Bind("Visual", "Other", "#000000", "Color Other Elements Boombox");
        }

        public Lang.Languages GetLanguages()
        {
            switch (languages.Value.ToLower())
            {
                case "ru": return Lang.Languages.RU;
                case "en": return Lang.Languages.EN;
                default: return Lang.Languages.RU;
            }
        }

        public Lang GetLang()
        {
            return lang;
        }
    }

    internal class Lang
    {
        public Dictionary<Localized, string> Localized_Keys = new Dictionary<Localized, string>();

        public enum Localized
        {
            main_1,
            main_2,
            main_3,
            main_4,
            main_5,
            main_6,
            main_7,
            main_8,
            main_9,
            main_10,
            main_11,
            main_12,
            main_13,
            main_14,
        }

        public enum Languages
        {
            RU,
            EN
        }

        public void SwitchLang(Languages languages)
        {
            Localized_Keys.Clear();
            switch(languages)
            {
                case Languages.RU: GetConfigRU(); break;
                case Languages.EN: GetConfigEN(); break;
            }
        }

        public void GetConfigRU()
        {
            Localized_Keys.Add(Localized.main_1, "Пожалуйста, подождите, загружаются дополнительные библиотеки, чтобы модификация заработала.");
            Localized_Keys.Add(Localized.main_2, $"Взять BoomBox[{Plugin.Version}] : [E]\n@2 - @3\n@1 громкость\nСейчас играет: @4\nДоступных треков: @5");
            Localized_Keys.Add(Localized.main_3, "Все дополнительные библиотеки загружены, теперь вы можете использовать команды для бумбокса.");
            Localized_Keys.Add(Localized.main_4, "Подождите, трек еще загружается!");
            Localized_Keys.Add(Localized.main_5, "Команды:\n/bplay - Проиграть музыку\n/btime - Изменить позицию песни\n/bvolume - Изменить громкость трека");
            Localized_Keys.Add(Localized.main_6, "Введите правильный URL-адрес!");
            Localized_Keys.Add(Localized.main_7, "Пожалуйста подождите...");
            Localized_Keys.Add(Localized.main_8, "Трек был загружен в бумбокс");
            Localized_Keys.Add(Localized.main_9, "@1 изменил громкость трека @2");
            Localized_Keys.Add(Localized.main_10, "Введите правильную громкость трека (пример: 0, 10, 20, 30...)!");
            Localized_Keys.Add(Localized.main_11, "Ссылка недействительная!");
            Localized_Keys.Add(Localized.main_12, "Позиция трека изменена на @1!");
            Localized_Keys.Add(Localized.main_13, "Загрузка трека отменена!");
            Localized_Keys.Add(Localized.main_14, "Текущий трек был переключен на: @1!");
        }

        public void GetConfigEN()
        {
            Localized_Keys.Add(Localized.main_1, "Please wait, additional libraries are being loaded for the modification to work.");
            Localized_Keys.Add(Localized.main_2, $"Pickup BoomBox[{Plugin.Version}] : [E]\n@2 - @3\n@1 volume\nNow playing: @4\nAvailable tracks: @5");
            Localized_Keys.Add(Localized.main_3, "All libraries have loaded, now you can use the boombox commands.");
            Localized_Keys.Add(Localized.main_4, "Another track is being uploaded to the boombox!");
            Localized_Keys.Add(Localized.main_5, "Commands:\n/bplay - Play music\n/btime - Change the position of the song\n/bvolume - Change Boombox volume");
            Localized_Keys.Add(Localized.main_6, "Enter the correct URL!");
            Localized_Keys.Add(Localized.main_7, "Please wait...");
            Localized_Keys.Add(Localized.main_8, "The track was uploaded to the boombox");
            Localized_Keys.Add(Localized.main_9, "@1 changed the volume @2 of the boombox.");
            Localized_Keys.Add(Localized.main_10, "Enter the correct Volume (example: 0, 10, 20, 30...)!");
            Localized_Keys.Add(Localized.main_11, "Link is invalid!");
            Localized_Keys.Add(Localized.main_12, "Track position changed to @1!");
            Localized_Keys.Add(Localized.main_13, "Track download canceled!");
            Localized_Keys.Add(Localized.main_14, "The current track has been switched to: @1!");
        }
    }
}
