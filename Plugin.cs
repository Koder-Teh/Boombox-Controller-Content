using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;

namespace BoomboxController_Content
{
    [BepInPlugin("KoderTech.BoomboxController", "BoomboxController", Version)]
    internal class Plugin : BaseUnityPlugin
    {
        public static Harmony HarmonyLib;

        public static Configs config;

        public static BoomboxController controller;

        public static ManualLogSource logSource { get; set; }

        public const string Version = "1.0.0";

        private void Awake()
        {
            logSource = Logger;
            config = new Configs();
            controller = new BoomboxController();
            HarmonyLib = new Harmony("com.kodertech.BoomboxController");
            Startup();
        }

        public void WriteLogo()
        {
            Logger.LogInfo($"\n" +
                     $"                                                                                                                                                                  \n" +
                     $"`7MM\"\"\"Yp,                                   *MM                                 .g8\"\"\"bgd                  mm                   `7MM `7MM                  \n" +
                     $"  MM    Yb                                    MM                               .dP'     `M                  MM                     MM   MM                  \n" +
                     $"  MM    dP  ,pW\"Wq.   ,pW\"Wq.`7MMpMMMb.pMMMb. MM,dMMb.   ,pW\"Wq.`7M'   `MF'    dM'       `,pW\"Wq.`7MMpMMMbmmMMmm `7Mb,od8 ,pW\"Wq.  MM   MM  .gP\"Ya `7Mb,od8 \n" +
                     $"  MM\"\"\"bg. 6W'   `Wb 6W'   `Wb MM    MM    MM MM    `Mb 6W'   `Wb `VA ,V'      MM        6W'   `Wb MM    MM MM     MM' \"'6W'   `Wb MM   MM ,M'   Yb  MM' \"' \n" +
                     $"  MM    `Y 8M     M8 8M     M8 MM    MM    MM MM     M8 8M     M8   XMX        MM.       8M     M8 MM    MM MM     MM    8M     M8 MM   MM 8M\"\"\"\"\"\"  MM     \n" +
                     $"  MM    ,9 YA.   ,A9 YA.   ,A9 MM    MM    MM MM.   ,M9 YA.   ,A9 ,V' VA.      `Mb.     ,YA.   ,A9 MM    MM MM     MM    YA.   ,A9 MM   MM YM.    ,  MM     \n" +
                     $".JMMmmmd9   `Ybmd9'   `Ybmd9'.JMML  JMML  JMMLP^YbmdP'   `Ybmd9'.AM.   .MA.      `\"bmmmd' `Ybmd9'.JMML  JMML`Mbmo.JMML.   `Ybmd9'.JMML.JMML.`Mbmmd'.JMML.   \n" +
                     $"                                                                                                                                                                  ");
        }

        public void Startup()
        {
            new WinApi().SizeConsole(1500, 500);
            WriteLogo();
            //config.GetLang().SwitchLang(config.GetLanguages());
            if (!Directory.Exists(@$"BoomboxController\other")) Directory.CreateDirectory(@$"BoomboxController\other");
            if (!Directory.Exists(@$"BoomboxController\other\local")) Directory.CreateDirectory(@$"BoomboxController\other\local");
            if (!File.Exists(@$"BoomboxController\other\ffmpeg.exe"))
            {
                if (File.Exists(@$"BoomboxController\other\ffmpeg-master-latest-win64-gpl.zip"))
                {
                    if (!Downloader.Unpacking())
                    {
                        Thread thread = new Thread(() => Downloader.DownloadFilesToUnpacking(new Uri("https://github.com/BtbN/FFmpeg-Builds/releases/download/latest/ffmpeg-master-latest-win64-gpl.zip"), @"BoomboxController\other\ffmpeg-master-latest-win64-gpl.zip"));
                        thread.Start();
                    }
                }
                else
                {
                    Thread thread = new Thread(() => Downloader.DownloadFilesToUnpacking(new Uri("https://github.com/BtbN/FFmpeg-Builds/releases/download/latest/ffmpeg-master-latest-win64-gpl.zip"), @"BoomboxController\other\ffmpeg-master-latest-win64-gpl.zip"));
                    thread.Start();
                }
            }
            controller.InitializationBoombox();
        }
    }

    internal class GetPlugin
    {
        internal virtual void Log(object message)
        {
            Plugin.logSource.LogInfo(message);
        }

        public virtual Configs GetConfig()
        {
            return Plugin.config;
        }
        internal virtual BoomboxController GetBoombox()
        {
            return Plugin.controller;
        }

        public virtual Harmony GetHarmony()
        {
            return Plugin.HarmonyLib;
        }
    }
}