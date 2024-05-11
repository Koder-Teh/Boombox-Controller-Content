using BepInEx;
using BoomboxController_Content.Audio;
using BoomboxController_Content.Radio;
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BoomboxController_Content.Menu
{
    internal class MenuManager : BoomboxController
    {
        public static TextMeshProUGUI buttonMain;
        public static bool OnOffButton = false;
        public static Slider sliderVolume;
        public static Slider sliderPitch;
        public static InputField urlText;
        public static Button buttonLoad;
        public static Dropdown dropdown;

        public static async void CreateMenu()
        {
            TMP_DefaultControls.Resources resources = new TMP_DefaultControls.Resources();
            DefaultControls.Resources resources1 = new DefaultControls.Resources();
            GameObject canvas = new GameObject("BoomboxMenu");
            panelMenu = canvas;
            canvas.AddComponent<RectTransform>();
            canvas.AddComponent<Canvas>();
            canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.AddComponent<CanvasScaler>();
            canvas.AddComponent<GraphicRaycaster>();
            GameObject panel = new GameObject("PanelMenu");
            panel.AddComponent<MenuManager>();
            panel.AddComponent<UnityEngine.UI.Image>();
            panel.AddComponent<RectTransform>();
            panel.AddComponent<CanvasRenderer>();
            panel.transform.localScale = new Vector3(11, 6, 1);
            panel.GetComponent<UnityEngine.UI.Image>().color = new Color(0, 0, 0.3f, 0.4f);
            panel.transform.SetParent(canvas.transform, false);

            GameObject gfg = TMP_DefaultControls.CreateButton(resources);
            gfg.transform.localScale = new Vector3(0.15f, 0.25f, 1);
            TextMeshProUGUI text = gfg.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            buttonMain = text;
            text.text = "Play";
            gfg.GetComponent<Button>().onClick.AddListener(ClickButton);
            gfg.transform.SetParent(panel.transform, false);

            GameObject gfg1 = DefaultControls.CreateSlider(resources1);
            GameObject gfsd = TMP_DefaultControls.CreateText(resources);
            gfsd.transform.localScale = new Vector3(0.6f, 0.6f, 1);
            gfsd.transform.localPosition = new Vector3(22f, 25.5f, 0);
            TextMeshProUGUI textgfsd = gfsd.GetComponent<TextMeshProUGUI>();
            textgfsd.text = "Volume";
            textgfsd.color = new Color(1f, 1f, 1f, 1f);
            textgfsd.transform.SetParent(gfg1.transform, false);
            gfg1.transform.localScale = new Vector3(0.15f, 0.25f, 1);
            gfg1.transform.localPosition = new Vector3(-29.5f, 0, 0);
            Slider slider = gfg1.GetComponent<Slider>();
            slider.value = 0.5f;
            slider.maxValue = 1f;
            slider.minValue = 0f;
            slider.onValueChanged.AddListener(ChangeSliderVolume);
            sliderVolume = slider;
            gfg1.transform.SetParent(panel.transform, false);

            GameObject gfg2 = DefaultControls.CreateSlider(resources1);
            GameObject gfsd1 = TMP_DefaultControls.CreateText(resources);
            gfsd1.transform.localScale = new Vector3(0.6f, 0.6f, 1);
            gfsd1.transform.localPosition = new Vector3(35.5f, 25.5f, 0);
            TextMeshProUGUI textgfsd1 = gfsd1.GetComponent<TextMeshProUGUI>();
            textgfsd1.text = "Pitch";
            textgfsd1.color = new Color(1f, 1f, 1f, 1f);
            textgfsd1.transform.SetParent(gfg2.transform, false);
            gfg2.transform.localScale = new Vector3(0.15f, 0.25f, 1);
            gfg2.transform.localPosition = new Vector3(29.5f, 0, 0);
            Slider slider1 = gfg2.GetComponent<Slider>();
            slider1.value = 1f;
            slider1.maxValue = 2f;
            slider1.minValue = 0f;
            slider1.onValueChanged.AddListener(ChangeSliderPitch);
            sliderPitch = slider1;
            gfg2.transform.SetParent(panel.transform, false);

            GameObject input = DefaultControls.CreateInputField(resources1);
            input.transform.localScale = new Vector3(0.15f, 0.25f, 1);
            input.transform.localPosition = new Vector3(0, -10.0f, 0);
            InputField field = input.GetComponent<InputField>();
            urlText = field;
            input.transform.SetParent(panel.transform, false);

            GameObject inputbutton = TMP_DefaultControls.CreateButton(resources);
            inputbutton.transform.localScale = new Vector3(0.15f, 0.25f, 1);
            inputbutton.transform.localPosition = new Vector3(0, -20.0f, 0);
            TextMeshProUGUI buttontext = inputbutton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            buttontext.text = "Load";
            inputbutton.GetComponent<Button>().onClick.AddListener(ClickLoadUrlAsync);
            buttonLoad = inputbutton.GetComponent<Button>();
            inputbutton.transform.SetParent(panel.transform, false);

            //gfsd1.transform.localPosition = new Vector3(35.5f, 25.5f, 0);
            GameObject gfd = DefaultControls.CreateDropdown(resources1);
            Dropdown dropfg = gfd.GetComponent<Dropdown>();
            dropfg.onValueChanged.AddListener(ChangeDropdown);
            dropdown = dropfg;
            gfd.transform.localScale = new Vector3(0.15f, 0.25f, 1);
            gfd.transform.localPosition = new Vector3(0, -30f, 0);
            gfd.transform.SetParent(panel.transform, false);

            GameObject logo = TMP_DefaultControls.CreateText(resources);
            logo.transform.localScale = new Vector3(0.14f, 0.2f, 1);
            logo.transform.localPosition = new Vector3(-8.2f, 23.5f, 0);
            TextMeshProUGUI logotext = logo.GetComponent<TextMeshProUGUI>();
            logotext.text = "Boombox Controller";
            logotext.color = new Color(1f, 1f, 1f, 1f);
            logotext.enableWordWrapping = false;
            logotext.transform.SetParent(panel.transform, false);
            HideMenu();
            dropfg.ClearOptions();
            dropfg.options.Add(new Dropdown.OptionData()
            {
                text = "Local Songs"
            });
            dropfg.options.Add(new Dropdown.OptionData()
            {
                text = "Network Songs"
            });
        }

        public static void ClickButton()
        {
            switch (OnOffButton)
            {
                case true:
                    OnOffButton = false;
                    Boombox_Start = false;
                    buttonMain.text = "Play";
                    networkManager.CallRPC("RPC_SyncPosition", RpcTarget.All, currectTrack);
                    networkManager.CallRPC("RPC_OnOffMusic", RpcTarget.All, OnOffButton, Boombox_Start);
                    break;
                case false:
                    if (AudioManager.listmusic == AudioManager.SwitchCatalog.Network)
                    {
                        if (musicList_net.ToArray().Length == 0)
                        {
                            HelmetText.Instance.SetHelmetText("Нету песен в каталоге Network!", 1f);
                            break;
                        }
                        OnOffButton = true;
                        Boombox_Start = true;
                        buttonMain.text = "Stop";
                        networkManager.CallRPC("RPC_SyncPosition", RpcTarget.All, currectTrack);
                        networkManager.CallRPC("RPC_OnOffMusic", RpcTarget.All, OnOffButton, Boombox_Start);
                        break;
                    }
                    else
                    {
                        if (musicList_local.ToArray().Length == 0)
                        {
                            HelmetText.Instance.SetHelmetText("Нету песен в каталоге Local!", 1f);
                            break;
                        }
                        OnOffButton = true;
                        Boombox_Start = true;
                        buttonMain.text = "Stop";
                        networkManager.CallRPC("RPC_SyncPosition", RpcTarget.All, currectTrack);
                        networkManager.CallRPC("RPC_OnOffMusic", RpcTarget.All, OnOffButton, Boombox_Start);
                    }
                    break;
            }
        }

        public static void ClickLoadUrlAsync()
        {
            networkManager.CallRPC("RPC_LoadingAudio", RpcTarget.All, urlText.text);
        }

        public static void ChangeSliderVolume(float val)
        {
            foreach (Player pl in RadioManager.GetAllPlayer())
            {
                if(pl.data.currentItem != null && pl.data.currentItem.item.name == "Radio")
                {
                    if(pl.refs.view.Controller.NickName == Player.localPlayer.refs.view.Controller.NickName)
                    {
                        networkManager.CallRPC("RPC_ChangeVolume", RpcTarget.All, val, sliderVolume.value);
                    }
                }
            }
            //Local_ChangeVolume(val);
        }

        public static void ChangeSliderPitch(float val)
        {
            foreach (Player pl in RadioManager.GetAllPlayer())
            {
                if (pl.data.currentItem != null && pl.data.currentItem.item.name == "Radio")
                {
                    if (pl.refs.view.Controller.NickName == Player.localPlayer.refs.view.Controller.NickName)
                    {
                        networkManager.CallRPC("RPC_ChangePitch", RpcTarget.All, val, sliderPitch.value);
                    }
                }
            }
            //Local_ChangePitch(val);
        }

        public static void ChangeDropdown(int val)
        {
            OnOffButton = false;
            Boombox_Start = false;
            buttonMain.text = "Play";
            networkManager.CallRPC("RPC_SyncTrackPosition", RpcTarget.All, 0f);
            networkManager.CallRPC("RPC_SyncPosition", RpcTarget.All, currectTrack);
            networkManager.CallRPC("RPC_SyncCatalog", RpcTarget.All, val);
            networkManager.CallRPC("RPC_OnOffMusic", RpcTarget.All, OnOffButton, Boombox_Start);
        }

        public static void OpenMenu()
        {
            panelMenu.SetActive(true);
        }

        public static void HideMenu()
        {
            panelMenu.SetActive(false);
        }

        //public static void Local_OnOffMusic(bool on)
        //{
        //    GameObject radio = GameObject.Find("Radio(Clone)");
        //    ArtifactRadio rg = radio.GetComponent<ArtifactRadio>();
        //    object offonentry = rg.GetType().GetField("onOffEntry", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(rg);
        //    ((OnOffEntry)offonentry).on = on;
        //}

        //public static void Local_ChangeVolume(float val)
        //{
        //    GameObject radio = GameObject.Find("Radio(Clone)");
        //    ArtifactRadio rg = radio.GetComponent<ArtifactRadio>();
        //    rg.music.volume = val;
        //}

        //public static void Local_ChangePitch(float val)
        //{
        //    GameObject radio = GameObject.Find("Radio(Clone)");
        //    ArtifactRadio rg = radio.GetComponent<ArtifactRadio>();
        //    rg.music.pitch = val;
        //}
    }
}
