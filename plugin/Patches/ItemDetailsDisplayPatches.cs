using HarmonyLib;
using LegacyUpgrade.Extensions;

namespace LegacyUpgrade.Patches {
	[HarmonyPatch(typeof(ItemDetailsDisplay))]
	public static class ItemDetailsDisplayPatches {

		[HarmonyPatch(nameof(ItemDetailsDisplay.RefreshEnchantmentDetails)), HarmonyPostfix]
		private static void ItemDetailsDisplay_RefreshEnchantmentDetails_Postfix(ItemDetailsDisplay __instance) {
			if (__instance.m_lastItem && __instance.m_lastItem.IsEnchanted && __instance.m_lastItem is Equipment lastItem) {
				Enchantment activeEnchantment = lastItem.ActiveEnchantments[0];
				if (activeEnchantment.PresetID == LegacyUpgradeConstants.LegacyBondEnchantmentID) {
					if (lastItem.TryGetLegacyBond(out LegacyBond legacyBond)) {
						__instance.GetEnchantmentRow(0).SetInfo(LocalizationManager.Instance.GetLoc($"{LegacyUpgrade.GUID}.enchantment.bond_value", legacyBond.GetProgressText(), legacyBond.CharacterName), "");
						if (Global.CheatsEnabled) {
							__instance.GetEnchantmentRow(1).SetInfo("ProgressValue", legacyBond.Progress);
							__instance.GetEnchantmentRow(2).SetInfo("CharacterID", legacyBond.CharacterID);
							__instance.GetEnchantmentRow(3).SetInfo("LegacyChestID", legacyBond.LegacyChestID);
						}
					} else {
						__instance.GetEnchantmentRow(0).SetInfo(LocalizationManager.Instance.GetLoc($"{LegacyUpgrade.GUID}.enchantment.unbound"), "");
					}
				}
			}
		}

	}
}