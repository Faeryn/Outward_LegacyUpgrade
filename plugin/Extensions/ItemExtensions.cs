using System.Runtime.CompilerServices;

namespace FaerynModCauldron.LegacyUpgrade.Extensions {
	public static class ItemExtensions {
		private static ConditionalWeakTable<Item, ItemExt> ItemExts = new ConditionalWeakTable<Item, ItemExt>();

		private static ItemExt Ext(this Item item) {
			if (!ItemExts.TryGetValue(item, out ItemExt ext)) {
				ext = new ItemExt();
				ItemExts.Add(item, ext);
			}
			return ext;
		}

		public static bool TryGetLegacyBond(this Item item, out LegacyBond legacyBond) {
			legacyBond = item.Ext().LegacyBond;
			return legacyBond != null;
		}

		public static void SetLegacyBond(this Item item, LegacyBond legacyBond) {
			item.Ext().LegacyBond = legacyBond;
		}

		public static bool HasLegacyBondEnchantment(this Item item) {
			if (item.IsEnchanted && item is Equipment equipment) {
				Enchantment activeEnchantment = equipment.ActiveEnchantments[0];
				return activeEnchantment.PresetID == LegacyUpgradeConstants.LegacyBondEnchantmentID;
			}
			return false;
		}

		public static bool HasLegacyItem(this Item item) {
			return item.LegacyItemID != -1 && item.LegacyItemID != item.ItemID;
		}
	}
}