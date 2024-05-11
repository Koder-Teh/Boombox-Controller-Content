using BoomboxController_Content.Audio;
using BoomboxController_Content.Player_Inventory;
using BoomboxController_Content.Shop;
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zorro.Core.CLI;

namespace BoomboxController_Content.Debug
{
    internal class Console : BoomboxController
    {
        #region GiveBoombox
        [ConsoleCommand]
        public static void EquipRadio()
        {
            if (ItemDatabase.TryGetItemFromID(36, out var item))
            {
                if (Player.localPlayer.TryGetInventory(out var o))
                {
                    PlayerInventoryManager.TryAddItem(new ItemDescriptor(item, new ItemInstanceData(Guid.NewGuid())), o);
                }
            }
        }
        #endregion

        #region AddItemShop
        [ConsoleCommand]
        public static void AddItemShop()
        {
            ShopManager.AddItemShop();
            networkManager.CallRPC("RPCA_AddItemToRadio", RpcTarget.All);
        }
        #endregion

        #region AddMoney
        [ConsoleCommand]
        public static void AddMoney()
        {
            SurfaceNetworkHandler.RoomStats.AddMoney(10000);
        }

        [ConsoleCommand]
        public static void ChangePosition(int index)
        {
            GameObject radio = GameObject.Find("Radio(Clone)");
            ArtifactRadio rg = radio.GetComponent<ArtifactRadio>();
            switch (AudioManager.listmusic)
            {
                case AudioManager.SwitchCatalog.Local:
                    rg.music.clip = musicList_local[index];
                    break;
                case AudioManager.SwitchCatalog.Network:
                    rg.music.clip = musicList_net[index];
                    break;
            }
        }
        #endregion

        #region ChangePosititonPlayer
        [ConsoleCommand]
        public static void PositionPlayer()
        {
            Player.localPlayer.input.movementInput.y += 100f;
        }
        #endregion
    }
}
