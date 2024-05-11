using BoomboxController_Content.Menu;
using BoomboxController_Content.Radio;
using HarmonyLib;
using Photon.Realtime;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace BoomboxController_Content.Vision
{
    internal class VisionManager : BoomboxController
    {

        public static EquippedUI equippedUI;

        public static List<(IHaveUIData, TextMeshProUGUI)> m_entries;

        [HarmonyPatch(typeof(EquippedUI), "SetData")]
        [HarmonyPrefix]
        public static void SetData(EquippedUI __instance, ItemDescriptor itemDescriptor, ref ItemDescriptor ___m_lastItemDescriptor, ref List<(IHaveUIData, TextMeshProUGUI)> ___m_entries)
        {
            equippedUI = __instance;
            m_entries = ___m_entries;
            if (itemDescriptor.data != ___m_lastItemDescriptor.data || itemDescriptor.item == null)
            {
                foreach (var entry in ___m_entries)
                {
                    Destroy(entry.Item2.gameObject);
                }
                ___m_entries.Clear();
                bool flag = itemDescriptor.item != null;
                if (flag)
                {
                    if(itemDescriptor.item.name == "Radio")
                    {
                        Player player = Player.localPlayer;
                        if (player != null)
                        {
                            __instance.m_itemNameText.text = itemDescriptor.item.GetLocalizedDisplayName();
                            foreach (ItemDataEntry dataEntry in itemDescriptor.data.m_dataEntries)
                            {
                                if (dataEntry is IHaveUIData uiData2)
                                {
                                    SpawnText(uiData2);
                                }
                            }
                            itemDescriptor.item.GetTootipData().ForEach(SpawnText);
                            string localizedString = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.DropItem);
                            SpawnText(new ItemKeyTooltip("[Q] " + localizedString));
                            SpawnText(new ItemKeyTooltip("Open menu: [F9]"));
                            SpawnText(new ItemKeyTooltip("Developer by KoderTech"));
                            SpawnText(new ItemKeyTooltip("With love from Russia!"));
                        }
                    }
                    else
                    {
                        __instance.m_itemNameText.text = itemDescriptor.item.GetLocalizedDisplayName();
                        foreach (ItemDataEntry dataEntry in itemDescriptor.data.m_dataEntries)
                        {
                            if (dataEntry is IHaveUIData uiData2)
                            {
                                SpawnText(uiData2);
                            }
                        }
                        itemDescriptor.item.GetTootipData().ForEach(SpawnText);
                        string localizedString = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.DropItem);
                        SpawnText(new ItemKeyTooltip("[Q] " + localizedString));
                        SpawnText(new ItemKeyTooltip("Hold [Q] Throw Item"));
                    }
                }
                __instance.gameObject.SetActive(value: false);
                __instance.gameObject.SetActive(flag);
            }
            ___m_lastItemDescriptor = itemDescriptor;
        }

        public static void SpawnText(IHaveUIData uiData)
        {
            TextMeshProUGUI component = Instantiate(equippedUI.m_textPrefab, equippedUI.m_textPrefab.transform.parent).GetComponent<TextMeshProUGUI>();
            component.gameObject.SetActive(value: true);
            m_entries.Add((uiData, component));
        }

        [HarmonyPatch(typeof(Player), "FixedUpdate")]
        [HarmonyPostfix]
        public static void FixedUpdate_Player(Player __instance)
        {
            if (__instance.data.currentItem != null && __instance.data.currentItem.item.name == "Radio" && EquipLastRadio == Player.localPlayer.refs.view.Controller.NickName)
            {
                if (cooldownMenu == 0)
                {
                    if (UnityEngine.Input.GetKey(KeyCode.F9))
                    {
                        if (isOpenMenu == false)
                        {
                            MenuManager.OpenMenu();
                            isOpenMenu = true;
                            cooldownMenu = 10.0;
                        }
                        else
                        {
                            MenuManager.HideMenu();
                            isOpenMenu = false;
                            cooldownMenu = 10.0;
                        }
                    }
                }
                if (cooldownMenu > 0)
                {
                    cooldownMenu -= 0.5;
                }
            }

            if (RadioManager.GetFindCount() > 1)
            {
                GameObject radio = RadioManager.GetFindIndex()[RadioManager.GetFindCount() - 1];
                if (radio.transform.parent.gameObject.name == "PickupHolder(Clone)")
                {
                    SurfaceNetworkHandler.RoomStats.AddMoney(40);
                    Destroy(radio.transform.parent.gameObject);
                }
            }

            if (RadioManager.GetFindIndex().Length > 0)
            {
                if (RadioManager.GetFindIndex()[0] != null)
                {
                    radio = RadioManager.GetFindIndex()[0].GetComponent<ArtifactRadio>();
                }
            }
        }
    }
}
