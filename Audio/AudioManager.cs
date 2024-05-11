using BepInEx;
using BoomboxController_Content.Menu;
using BoomboxController_Content.Radio;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

namespace BoomboxController_Content.Audio
{
    internal class AudioManager : BoomboxController
    {
        public static SwitchCatalog listmusic = SwitchCatalog.Local;

        public enum SwitchCatalog
        {
            Local,
            Network
        }

        public static async Task LoadingSong(string urlText)
        {
            if (Uri.IsWellFormedUriString(urlText, UriKind.Absolute))
            {
                var url = urlText.Remove(0, 8);
                switch (url.Substring(0, url.IndexOf('/')))
                {
                    case "www.youtube.com":
                        if (url.Remove(0, url.IndexOf('/')) == "/watch") break;
                        musicList_net.Clear();
                        MenuManager.urlText.enabled = false;
                        MenuManager.buttonLoad.enabled = false;
                        string NameTrack = string.Empty;
                        FileInfo[] file = new DirectoryInfo(@"BoomboxController\other").GetFiles("*.mp3");
                        if (file.Length == 1)
                        {
                            File.Delete(@$"BoomboxController\other\{file[0].Name}");
                        }
                        await Task.Run(() =>
                        {
                            bool succeeded = false;
                            bool part = false;
                            Process info = new Process();
                            info.StartInfo.FileName = @"BoomboxController\other\yt-dlp.exe";
                            info.StartInfo.UseShellExecute = false;
                            info.StartInfo.Arguments = $"-f bestaudio --extract-audio --ignore-config --audio-format mp3 --audio-quality 0 {urlText}";
                            info.StartInfo.WorkingDirectory = @$"BoomboxController\other";
                            info.StartInfo.CreateNoWindow = true;
                            info.Start();
                            while (!succeeded)
                            {
                                if (part)
                                {
                                    if (File.Exists(@$"BoomboxController\other\{NameTrack}"))
                                    {
                                        succeeded = true;
                                        break;
                                    }
                                }
                                else
                                {
                                    foreach (FileInfo f in new DirectoryInfo(@"BoomboxController\other").GetFiles("*.mp3"))
                                    {
                                        if (f.Exists)
                                        {
                                            NameTrack = f.Name;
                                        }
                                    }
                                    if (Process.GetProcessById(info.Id).HasExited)
                                    {
                                        if (File.Exists(@$"BoomboxController\other\{NameTrack}"))
                                        {
                                            part = true;
                                        }
                                    }
                                }
                                System.Threading.Thread.Sleep(1000);
                            }
                        });
                        if (File.Exists(@$"BoomboxController\other\{NameTrack}"))
                        {
                            bool sumbBlock = false;
                            List<string> sumbol = new List<string>();
                            FileInfo ext = new FileInfo(@$"BoomboxController\other\{NameTrack}");
                            foreach (string sumb in sumbols)
                            {
                                if (ext.Name.Contains(sumb))
                                {
                                    sumbol.Add(sumb);
                                    sumbBlock = true;
                                }
                            }
                            if (sumbBlock)
                            {
                                string NameFile = String.Empty;
                                foreach (string sumb in sumbol)
                                {
                                    NameFile = NameTrack.Replace(sumb, "");
                                    ext.MoveTo(@$"BoomboxController\other\{NameTrack.Replace(sumb, "")}");
                                }
                                currectTrack = 0;
                                GameObject radio = GameObject.Find("Radio(Clone)");
                                ArtifactRadio rg = radio.GetComponent<ArtifactRadio>();
                                object offonentry = rg.GetType().GetField("radioTimeEntry", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(rg);
                                ((TimeEntry)offonentry).currentTime = 0f;
                                await Task.Run(async () =>
                                {
                                    await RadioManager.GetAudio(Paths.GameRootPath + @$"\BoomboxController\other\{NameFile}", musicList_net);
                                });
                                plugin.Log("ЗАГРУЗИЛ!!!!");
                                MenuManager.urlText.enabled = true;
                                MenuManager.buttonLoad.enabled = true;
                            }
                            else
                            {
                                currectTrack = 0;
                                GameObject radio = GameObject.Find("Radio(Clone)");
                                ArtifactRadio rg = radio.GetComponent<ArtifactRadio>();
                                object offonentry = rg.GetType().GetField("radioTimeEntry", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(rg);
                                ((TimeEntry)offonentry).currentTime = 0f;
                                await Task.Run(async () =>
                                {
                                    await RadioManager.GetAudio(Paths.GameRootPath + @$"\BoomboxController\other\{NameTrack}", musicList_net);
                                });
                                plugin.Log("ЗАГРУЗИЛ!!!!");
                                MenuManager.urlText.enabled = true;
                                MenuManager.buttonLoad.enabled = true;
                            }
                        }
                        break;
                }
            }
        }
    }
}
