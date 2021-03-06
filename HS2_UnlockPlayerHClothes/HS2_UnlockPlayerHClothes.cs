﻿using HarmonyLib;

using BepInEx;
using BepInEx.Logging;

using System.Collections.Generic;
using System.Linq;

namespace HS2_UnlockPlayerHClothes {
    [BepInProcess("HoneySelect2")]
    [BepInPlugin(nameof(HS2_UnlockPlayerHClothes), nameof(HS2_UnlockPlayerHClothes), VERSION)]
    public class HS2_UnlockPlayerHClothes : BaseUnityPlugin
    {
        public const string VERSION = "1.4.1";
        
        public new static ManualLogSource Logger;

        private static readonly List<int> clothesKindList = new List<int>{0, 2, 4, 1, 3, 5, 6};

        private void Awake()
        {
            Logger = base.Logger;

            var harmony = new Harmony(nameof(HS2_UnlockPlayerHClothes));
            harmony.PatchAll(typeof(Transpilers));
            harmony.PatchAll(typeof(HS2_UnlockPlayerHClothes));
        }

        [HarmonyPostfix, HarmonyPatch(typeof(HScene), "SetStartVoice")]
        public static void HScene_SetStartVoice_ApplyClothesConfig(HScene __instance)
        {
            var hData = Manager.Config.HData;
            var males = __instance.GetMales();

            if (males[0] != null)
            {
                foreach (var kind in clothesKindList.Where(kind => males[0].IsClothesStateKind(kind)))
                    males[0].SetClothesState(kind, (byte)(hData.Cloth ? 0 : 2));
            
                males[0].SetAccessoryStateAll(hData.Accessory);
                males[0].SetClothesState(7, (byte)(!hData.Shoes ? 2 : 0));
            }
            
            if (males[1] != null)
            {
                foreach (var kind in clothesKindList.Where(kind => males[1].IsClothesStateKind(kind)))
                    males[1].SetClothesState(kind, (byte)(hData.SecondCloth ? 0 : 2));
            
                males[1].SetAccessoryStateAll(hData.SecondAccessory);
                males[1].SetClothesState(7, (byte)(!hData.SecondShoes ? 2 : 0));
            }
        }
    }
}
