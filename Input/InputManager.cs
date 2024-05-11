using BoomboxController_Content.Menu;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BoomboxController_Content.Input
{
    internal class InputManager : BoomboxController
    {
        [HarmonyPatch(typeof(GlobalInputHandler), "CanTakeInput")]
        [HarmonyPostfix]
        public static void CanTakeInput(GlobalInputHandler __instance, ref bool __result)
        {
            if (isOpenMenu)
            {
                __result = false;
            }
        }

        [HarmonyPatch(typeof(CursorHandler), "Update")]
        [HarmonyPostfix]
        public static void Update(CursorHandler __instance)
        {
            if (isOpenMenu)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
}
