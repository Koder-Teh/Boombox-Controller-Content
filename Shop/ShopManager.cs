using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BoomboxController_Content.Shop
{
    internal class ShopManager : BoomboxController
    {
        public static void AddItemShop()
        {
            if (ItemDatabase.TryGetItemFromID(36, out var item))
            {
                ShopItem itemshop = new ShopItem(item);
                object m_CategoryItemDic = ShopHandler.Instance.GetType().GetField("m_CategoryItemDic", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(ShopHandler.Instance);
                object m_ItemsForSaleDictionary = ShopHandler.Instance.GetType().GetField("m_ItemsForSaleDictionary", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(ShopHandler.Instance);
                ((Dictionary<ShopItemCategory, List<ShopItem>>)m_CategoryItemDic)[ShopItemCategory.Misc].Add(itemshop);
                ((Dictionary<byte, ShopItem>)m_ItemsForSaleDictionary).Add(36, itemshop);
            }
        }

        [HarmonyPatch(typeof(ShopHandler), "InitShop")]
        [HarmonyPostfix]
        private static void InitShop()
        {
            AddItemShop();
        }
    }
}
