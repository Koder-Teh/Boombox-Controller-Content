using BoomboxController_Content.Network;
using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BoomboxController_Content
{
    internal class Variables : MonoBehaviour
    {
        #region Type
        //public static AudioBoomBox bom;
        //public static VisualBoombox vbom;
        //public static BoomboxItem boomboxItem = new BoomboxItem();
        //public static Dictionary<string, AudioClip> musicList;
        //public static QuitManager quit;
        //public static QuitManager quits;
        //public static KeyControl up = null;
        //public static KeyControl down = null;
        public static GameObject panelMenu;
        public static GameObject netmanager;
        public static ArtifactRadio radio;
        public static List<AudioClip> musicList_local;
        public static List<AudioClip> musicList_net;
        #endregion

        #region Int
        //public static int timesPlayedWithoutTurningOff = 0;
        //public static int isSendingItemRPC = 0;
        //public static int Id = 0;
        //public static int totalTack = 0;
        public static int currectTrack = 0;
        #endregion

        #region Double
        public static double cooldownMenu = 0;
        #endregion

        #region Float
        public static float volume = 0.5f;
        public static float pitch = 1.0f;
        #endregion

        #region Bool
        //public static bool startMusics = true;
        //public static bool LoadingMusicBoombox = false;
        //public static bool LoadingLibrary = false;
        //public static bool isplayList = false;
        //internal static bool blockcompatibility = false;
        //public static bool waitAutoNext = false;
        //public static bool netSwitch = true;
        //public static bool currentTrackChange = false;
        public static bool isOpenMenu = false;
        public static bool InitializationMenu = false;
        public static bool Boombox_Start = false;
        #endregion

        #region String
        //public static string LastMessage;
        //public static string LastnameOfUserWhoTyped;
        //public static string NameTrack;
        public static string[] sumbols = { "+", "#", "�" };
        //public static string[] multi_name = { "SyncSong", };
        public static string EquipLastRadio = string.Empty;
        #endregion
    }
}
