using BoomboxController_Content.Audio;
using BoomboxController_Content.Menu;
using HarmonyLib;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

namespace BoomboxController_Content.Radio
{
    internal class RadioManager : BoomboxController
    {
        public static async void LoadingSong()
        {
            DirectoryInfo directory = new DirectoryInfo(@"BoomboxController\other\local");
            foreach(FileInfo file in directory.GetFiles())
            {
                await Task.Run(async () =>
                {
                    await GetAudio(file.FullName, musicList_local);
                });
            }
        }

        public static int GetAudioType(string path)
        {
            FileInfo file = new FileInfo(path);
            switch (file.Extension)
            {
                case ".mp3": return 13;
                case ".wav": return 20;
            }
            return -1;
        }

        public static async Task GetAudio(string url, List<AudioClip> musicList)
        {
            plugin.Log(url + " " + (AudioType)GetAudioType(url));
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip($"file:///{url}", (AudioType)GetAudioType(url)))
            {
                var content = www.SendWebRequest();

                while (!content.isDone) await Task.Delay(100);

                if (www.result != UnityWebRequest.Result.ConnectionError)
                {
                    AudioClip myClip = DownloadHandlerAudioClip.GetContent(www);
                    musicList.Add(myClip);
                }
            }
        }

        public static int GetFindCount()
        {
            int check = 0;
            GameObject[] gf = GameObject.FindObjectsOfType<GameObject>();
            foreach (var f in gf)
            {
                if (f.name == "Radio(Clone)")
                {
                    check++;
                }
            }
            return check;
        }

        public static GameObject[] GetFindIndex()
        {
            List<GameObject> list = new List<GameObject>();
            GameObject[] gf = GameObject.FindObjectsOfType<GameObject>();
            foreach (var f in gf)
            {
                if (f.name == "Radio(Clone)")
                {
                    list.Add(f);
                }
            }
            return list.ToArray();
        }

        public static Player[] GetAllPlayer()
        {
            return (from pl in UnityEngine.Object.FindObjectsOfType<Player>() where !pl.ai select pl).ToArray();
        }

        [HarmonyPatch(typeof(ArtifactRadio), "Update")]
        [HarmonyPrefix]
        private static bool Update(ArtifactRadio __instance, ref BatteryEntry ___batteryEntry, ref OnOffEntry ___onOffEntry, ref StashAbleEntry ___stashAbleEntry, ref TimeEntry ___radioTimeEntry, ref float ___timeToAlert, ref float ___timeOnTheGround)
        {
            if (musicList_net.ToArray().Length == 0 && musicList_net != null)
            {
                if (MenuManager.dropdown != null) MenuManager.dropdown.transform.gameObject.SetActive(false);
            }
            else
            {
                if (MenuManager.dropdown != null) MenuManager.dropdown.transform.gameObject.SetActive(true);
            }
            if (Boombox_Start)
            {
                ___onOffEntry.on = true;
            }
            else
            {
                ___onOffEntry.on = false;
            }
            if (!InitializationMenu)
            {
                MenuManager.CreateMenu();
                InitializationMenu = true;
            }
            else
            {
                __instance.music.pitch = pitch;
                __instance.music.volume = volume;
                if (MenuManager.sliderPitch.value != pitch) MenuManager.sliderPitch.value = pitch;
                if (MenuManager.sliderVolume.value != volume) MenuManager.sliderVolume.value = volume;
            }
            if (plugin.GetConfig().requstbattery.Value)
            {
                if (___batteryEntry.m_charge < 0f)
                {
                    ___onOffEntry.on = false;
                    ___onOffEntry.SetDirty();
                }
            }
            bool on = ___onOffEntry.on;
            if (on != __instance.music.enabled)
            {
                if (on)
                {
                    plugin.Log(___radioTimeEntry.currentTime);
                    __instance.music.SetTime(___radioTimeEntry.currentTime);
                }
                __instance.music.enabled = on;
            }
            __instance.bright.enabled = on;
            if (on)
            {
                if (plugin.GetConfig().requstbattery.Value)
                {
                    ___batteryEntry.m_charge -= Time.deltaTime;
                }
                ___radioTimeEntry.currentTime += Time.deltaTime;
            }
            if (GameObject.Find("LOOP: Radio(Clone) - ") != null)
            {
                List<AudioClip> musicList = null; 
                switch (AudioManager.listmusic)
                {
                    case AudioManager.SwitchCatalog.Local:
                        musicList = musicList_local;
                        break;
                    case AudioManager.SwitchCatalog.Network:
                        musicList = musicList_net;
                        break;
                }
                AudioSource source = GameObject.Find("LOOP: Radio(Clone) - ").gameObject.GetComponent<AudioSource>();
                float totalTime = __instance.music.clip.length;
                if (Math.Floor(source.time) == Math.Floor(totalTime))
                {
                    if ((currectTrack + 1) == musicList.Count)
                    {
                        currectTrack = 0;
                        __instance.music.clip = musicList[currectTrack];
                        ___onOffEntry.on = false;
                        __instance.music.enabled = false;
                        ___radioTimeEntry.currentTime = 0f;
                    }
                    else
                    {
                        ++currectTrack;
                        __instance.music.clip = musicList[currectTrack];
                        ___onOffEntry.on = false;
                        __instance.music.enabled = false;
                        ___radioTimeEntry.currentTime = 0f;
                    }
                }
            }
            //if (!__instance.isHeld)
            //{
            //    ___timeOnTheGround += Time.deltaTime;
            //}
            //else
            //{
            //    ___timeOnTheGround = 0f;
            //}
            //if (___timeOnTheGround > __instance.maxTimeOnGround && ___onOffEntry.on)
            //{
            //    ___onOffEntry.on = false;
            //    plugin.Log("Radio turned off because it was on the ground for too long");
            //    ___onOffEntry.SetDirty();
            //}
            if (on)
            {
                ___timeToAlert -= Time.deltaTime;
            }
            if (on && ___timeToAlert < 0f)
            {
                SFX_Player.instance.PlayNoise(__instance.transform.position, 30f);
                ___timeToAlert = __instance.alertIntervall;
                //plugin.Log("Radio noise");
            }
            return false;
        }

        [HarmonyPatch(typeof(ArtifactRadio), "ConfigItem")]
        [HarmonyPrefix]
        private static bool ConfigItem(ArtifactRadio __instance, ItemInstanceData data, PhotonView playerView, ref BatteryEntry ___batteryEntry, ref OnOffEntry ___onOffEntry, ref StashAbleEntry ___stashAbleEntry, ref TimeEntry ___radioTimeEntry)
        {
            UnityEngine.Random.State state = Random.state;
            Random.InitState(GameAPI.seed);
            AudioLoop components = __instance.GetComponent<AudioLoop>();
            __instance.music = components;
            currectTrack = 0;
            if(musicList_local.Count != 0 || musicList_net.Count != 0)
            {
                switch (AudioManager.listmusic)
                {
                    case AudioManager.SwitchCatalog.Local:
                        __instance.music.clip = musicList_local[currectTrack];
                        break;
                    case AudioManager.SwitchCatalog.Network:
                        currectTrack = 0;
                        __instance.music.clip = musicList_net[currectTrack];
                        break;
                }
            }
            Random.state = state;
            if (plugin.GetConfig().requstbattery.Value)
            {
                if (!data.TryGetEntry<BatteryEntry>(out ___batteryEntry))
                {
                    ___batteryEntry = new BatteryEntry
                    {
                        m_charge = __instance.maxBatteryCharge,
                        m_maxCharge = __instance.maxBatteryCharge
                    };
                    data.AddDataEntry(___batteryEntry);
                }
            }
            if (data.TryGetEntry<OnOffEntry>(out ___onOffEntry))
            {
                plugin.Log($"OnOff entry found, state: {___onOffEntry.on}");
            }
            else
            {
                ___onOffEntry = new OnOffEntry
                {
                    on = false
                };
                data.AddDataEntry(___onOffEntry);
                plugin.Log("OnOff entry not found, adding new entry with false.");
            }
            if (data.TryGetEntry<StashAbleEntry>(out ___stashAbleEntry))
            {
                plugin.Log($"stashAbleEntry entry found, isStashAble: {___stashAbleEntry.isStashAble}");
            }
            else
            {
                ___stashAbleEntry = new StashAbleEntry
                {
                    isStashAble = true
                };
                data.AddDataEntry(___stashAbleEntry);
                plugin.Log("stashAbleEntry entry not found, adding new entry with false. " + ___stashAbleEntry.isStashAble);
            }
            if (!data.TryGetEntry<TimeEntry>(out ___radioTimeEntry))
            {
                ___radioTimeEntry = new TimeEntry
                {
                    currentTime = 0f
                };
                data.AddDataEntry(___radioTimeEntry);
            }
            return false;
        }
    }
}
