using BoomboxController_Content.Input;
using BoomboxController_Content.Network;
using BoomboxController_Content.Player_Inventory;
using BoomboxController_Content.Radio;
using BoomboxController_Content.Save;
using BoomboxController_Content.Shop;
using BoomboxController_Content.Spawn;
using BoomboxController_Content.Vision;
using HarmonyLib;
using JetBrains.Annotations;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zorro.Core.CLI;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace BoomboxController_Content
{
    internal class BoomboxController : Variables
    {
        internal static PluginApi plugin;

        internal static NetworkManager networkManager;

        public static SaveManager saveManager;

        public void InitializationBoombox()
        {
            plugin = new PluginApi();
            saveManager = new SaveManager();
            networkManager = new NetworkManager();
            musicList_local = new List<AudioClip>();
            musicList_net = new List<AudioClip>();
            RadioManager.LoadingSong();
            plugin.GetHarmony().PatchAll(typeof(PlayerInventoryManager));
            plugin.GetHarmony().PatchAll(typeof(RadioManager));
            plugin.GetHarmony().PatchAll(typeof(VisionManager));
            plugin.GetHarmony().PatchAll(typeof(InputManager));
            plugin.GetHarmony().PatchAll(typeof(SpawnManager));
            plugin.GetHarmony().PatchAll(typeof(ShopManager));
            //Plugin.HarmonyLib.PatchAll(typeof(AudioManager));
            //Plugin.HarmonyLib.PatchAll(typeof(BoomboxManager));
            //Plugin.HarmonyLib.PatchAll(typeof(CommandManager));
            //Plugin.HarmonyLib.PatchAll(typeof(MenuManager));
            //Plugin.HarmonyLib.PatchAll(typeof(OptionManager));
            //Plugin.HarmonyLib.PatchAll(typeof(SaveManager));
            //Plugin.HarmonyLib.PatchAll(typeof(StartupManager));
            //Plugin.HarmonyLib.PatchAll(typeof(VisionManager));
            //Plugin.HarmonyLib.PatchAll(typeof(MultiplayerManager));
        }

        public static void DrawString(string chatMessage, float time)
        {
            HelmetText.Instance.SetHelmetText(chatMessage, time);
        }
    }
}
