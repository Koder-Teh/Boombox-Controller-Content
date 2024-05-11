using BoomboxController_Content.Audio;
using BoomboxController_Content.Menu;
using BoomboxController_Content.Shop;
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

namespace BoomboxController_Content.Network
{
    internal class NetworkManager : BoomboxController
    {
        public static PhotonView view;

        public NetworkManager()
        {
            if(netmanager == null)
            {
                netmanager = new GameObject("NetworkBoombox");
                netmanager.AddComponent<NetworkManager>();
                view = netmanager.AddComponent<PhotonView>();
                view.ViewID = 120;
                DontDestroyOnLoad(netmanager);
            }
        }

        public void CallRPC(string RPC_NAME, RpcTarget target, params object[] param)
        {
            view.RPC(RPC_NAME, target, param);
        }

        [PunRPC]
        public void RPC_OnOffMusic(bool on, bool swit)
        {
            switch (AudioManager.listmusic)
            {
                case AudioManager.SwitchCatalog.Local:
                    Variables.radio.music.clip = musicList_local[currectTrack];
                    break;
                case AudioManager.SwitchCatalog.Network:
                    currectTrack = 0;
                    Variables.radio.music.clip = musicList_net[currectTrack];
                    break;
            }
            GameObject radio = GameObject.Find("Radio(Clone)");
            ArtifactRadio rg = radio.GetComponent<ArtifactRadio>();
            object offonentry = rg.GetType().GetField("onOffEntry", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(rg);
            ((OnOffEntry)offonentry).on = on;
            Boombox_Start = swit;
        }

        [PunRPC]
        public void RPC_ChangeVolume(float val, float sliderVolume)
        {
            //GameObject radio = GameObject.Find("Radio(Clone)");
            //ArtifactRadio rg = radio.GetComponent<ArtifactRadio>();
            //rg.music.volume = val;
            volume = sliderVolume;
        }

        [PunRPC]
        public void RPC_ChangePitch(float val, float sliderPitch)
        {
            //GameObject radio = GameObject.Find("Radio(Clone)");
            //ArtifactRadio rg = radio.GetComponent<ArtifactRadio>();
            //rg.music.pitch = val;
            pitch = sliderPitch;
        }

        [PunRPC]
        public void RPC_SyncCatalog(int pos)
        {
            switch ((AudioManager.SwitchCatalog)pos)
            {
                case AudioManager.SwitchCatalog.Local:
                    AudioManager.listmusic = AudioManager.SwitchCatalog.Local;
                    radio.music.clip = musicList_local[currectTrack];
                    break;
                case AudioManager.SwitchCatalog.Network:
                    AudioManager.listmusic = AudioManager.SwitchCatalog.Network;
                    currectTrack = 0;
                    radio.music.clip = musicList_net[currectTrack];
                    break;
            }
        }

        [PunRPC]
        public void RPC_SyncPosition(int pos)
        {
            currectTrack = pos;
        }

        [PunRPC]
        public void RPC_SyncTrackPosition(float pos)
        {
            GameObject radio = GameObject.Find("Radio(Clone)");
            ArtifactRadio rg = radio.GetComponent<ArtifactRadio>();
            object dfg = rg.GetType().GetField("radioTimeEntry", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(rg);
            ((TimeEntry)dfg).currentTime = pos;
        }

        [PunRPC]
        public void RPC_LoadingAudio(string url)
        {
            Task.Run(async () => await AudioManager.LoadingSong(url));
        }
    }
}
