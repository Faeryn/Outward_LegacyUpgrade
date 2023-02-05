using System;
using System.Collections.Generic;
using HarmonyLib;

namespace LegacyUpgrade.Patches {
	[HarmonyPatch(typeof(EnchantmentMenu))]
	public class EnchantmentMenuPatches {
			
		[HarmonyPatch(nameof(EnchantmentMenu.Show), new Type[] { }), HarmonyPostfix]
		public static void EnchantmentMenu_Show_Postfix(EnchantmentMenu __instance) {
			List<EquipmentSlot.EquipmentSlotIDs> equipmentTypes = __instance.m_inventoryDisplay.m_filter.m_equipmentTypes;
			if (!equipmentTypes.Contains(EquipmentSlot.EquipmentSlotIDs.Back)) {
				equipmentTypes.Add(EquipmentSlot.EquipmentSlotIDs.Back);
			}
		}
	}
}