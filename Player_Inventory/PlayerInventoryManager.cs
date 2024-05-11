using BoomboxController_Content.Menu;
using BoomboxController_Content.Radio;
using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BoomboxController_Content.Player_Inventory
{
    internal class PlayerInventoryManager : BoomboxController
    {

        [HarmonyPatch(typeof(PlayerItems), "ChangeToSlot")]
        [HarmonyPrefix]
        private static bool ChangeToSlot(PlayerItems __instance, int slotID, Player ___player)
        {
            if (___player.TryGetInventory(out var o))
            {
                if (__instance.m_displayingSlot != slotID && o.TryGetItemInSlot(__instance.m_displayingSlot, out var item) && item.item != null && item.data.TryGetEntry<StashAbleEntry>(out var t) && !t.isStashAble)
                {
                    plugin.Log("Dropping because " + item.item.name + " it's not stashable");
                    ___player.data.throwCharge = 0.3f;
                    __instance.DropItem(__instance.m_displayingSlot, notCurrentItem: true);
                }
                __instance.m_displayingSlot = slotID;
                if (o.TryGetItemInSlot(slotID, out var item2))
                {
                    __instance.GetType().GetMethod("EquipItem", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(__instance, new object[1] { item2 });
                }
                else
                {
                    __instance.GetType().GetMethod("Unequip", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(__instance, new object[0] { });
                }
            }
            return false;
        }

        [HarmonyPatch(typeof(PlayerItems), "EquipItem")]
        [HarmonyPrefix]
        private static bool EquipItem(PlayerItems __instance, ItemDescriptor item, Player ___player)
        {
            if (item.item.name == "Radio") EquipLastRadio = ___player.refs.view.Controller.NickName;
            bool flag = ___player.data.currentItem != null && ___player.data.currentItem.item == item.item;
            __instance.GetType().GetMethod("Unequip", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(__instance, new object[0] { });
            if (!(item.item == null || flag))
            {
                __instance.GetType().GetMethod("Equip", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(__instance, new object[1] { item });
            }
            return false;
        }

        [HarmonyPatch(typeof(PlayerItems), "Unequip")]
        [HarmonyPrefix]
        private static void Unequip(PlayerItems __instance, Player ___player, ref Bodypart ___itemBodypart)
        {
            if (___player.data.currentItem != null)
            {
                ___player.data.currentItem.onUnequip?.Invoke(___player);
                object body = ___player.refs.ragdoll.GetType().GetMethod("GetBodypart", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(___player.refs.ragdoll, new object[1] { BodypartType.Item });
                Destroy(((Bodypart)body).animationTarget.gameObject);
                ___player.refs.ragdoll.GetType().GetMethod("RemoveItem", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(___player.refs.ragdoll, new object[1] { ___player.data.currentItem });
                Destroy(___player.data.currentItem.gameObject);
            }
            ___itemBodypart = null;
            ___player.data.currentItem = null;
        }

        [HarmonyPatch(typeof(Pickup), "RPC_RequestPickup")]
        [HarmonyPrefix]
        public static bool RPC_RequestPickup(Pickup __instance, int photonView, byte ___m_itemID, ItemInstanceData ___instanceData)
        {
            bool flag = false;
            Player component = PhotonNetwork.GetPhotonView(photonView).GetComponent<Player>();
            if (ItemDatabase.TryGetItemFromID(___m_itemID, out var item) && component.TryGetInventory(out var o))
            {
                ItemInstanceData data = ___instanceData.Copy();
                if (TryAddItem(new ItemDescriptor(item, data), out var slot, o))
                {
                    component.refs.view.RPC("RPC_SelectSlot", component.refs.view.Owner, slot.SlotID);
                    __instance.m_photonView.RPC("RPC_Remove", RpcTarget.MasterClient);
                    flag = true;
                }
            }
            if (!flag)
            {
                __instance.m_photonView.RPC("RPC_FailedToPickup", component.refs.view.Owner);
            }
            return false;
        }

        [HarmonyPatch(typeof(PlayerInventory), "RPC_AddToSlot")]
        [HarmonyPrefix]
        public static bool RPC_AddToSlot(PlayerInventory __instance, int slotID, byte itemID, byte[] data)
        {
            ItemInstanceData itemInstanceData = new ItemInstanceData(Guid.Empty);
            itemInstanceData.Deserialize(data);
            if (ItemDatabase.TryGetItemFromID(itemID, out var item))
            {
                if (__instance.TryGetSlot(slotID, out var slot))
                {
                    slot.AddLocal(new ItemDescriptor(item, itemInstanceData));
                    plugin.Log($"adding item:{itemID} to slot: {slotID}");
                }
                else
                {
                    plugin.Log($"Failed to get slot {slotID}");
                }
            }
            return false;
        }

        public static bool TryAddItem(ItemDescriptor item, PlayerInventory o)
        {
            InventorySlot slot;
            return TryAddItem(item, out slot, o);
        }

        public static bool TryAddItem(ItemDescriptor item, out InventorySlot slot, PlayerInventory o)
        {
            if (o.TryGetFeeSlot(out slot) && slot.SlotID < 3)
            {
                slot.Add(item);
                return true;
            }
            slot = null;
            return false;
        }
    }
}
