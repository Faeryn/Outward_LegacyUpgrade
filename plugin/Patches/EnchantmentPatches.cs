using FaerynModCauldron.LegacyUpgrade.Extensions;
using HarmonyLib;

namespace FaerynModCauldron.LegacyUpgrade.Patches {
	//[HarmonyPatch(typeof(Enchantment))]
	public static class EnchantmentPatches {

		[HarmonyPatch(nameof(Enchantment.ApplyEnchantment)), HarmonyPostfix]
		private static void Enchantment_ApplyEnchantment_Postfix(Enchantment __instance, Equipment _equipment) {
			if (__instance.m_equipment.TryGetLegacyBond(out LegacyBond legacyBond)) {
				return;
			}
			__instance.m_equipment.SetLegacyBond(new LegacyBond(_equipment.UID));
		}

		[HarmonyPatch(nameof(Enchantment.UnapplyEnchantment)), HarmonyPostfix]
		private static void Enchantment_UnapplyEnchantmentt_Postfix(Enchantment __instance) {
			if (!__instance.m_equipment.TryGetLegacyBond(out LegacyBond legacyBond)) {
				return;
			}
			__instance.m_equipment.SetLegacyBond(null);
		}

	}
}