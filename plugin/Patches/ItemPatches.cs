using HarmonyLib;
using LegacyUpgrade.Extensions;

namespace LegacyUpgrade.Patches {
	[HarmonyPatch(typeof(Item))]
	public static class ItemPatches {
		private const string LegacyBondLegacyChestID = "LegacyBondLegacyChestID";
		private const string LegacyBondCharacterID = "LegacyBondCharacterID";
		private const string LegacyBondProgress = "LegacyBondProgress";
		private const string LegacyBondCharacterName = "LegacyBondCharacterName";

		[HarmonyPatch(nameof(Item.BuildExtraInfoData)), HarmonyPostfix]
		private static void Item_BuildExtraInfoData_Postfix(Item __instance, Item.SyncType _syncType) {
			if (!__instance.TryGetLegacyBond(out LegacyBond legacyBond)) {
				return;
			}
			__instance.AddExtraData(LegacyBondLegacyChestID, legacyBond.LegacyChestID);
			__instance.AddExtraData(LegacyBondCharacterID, legacyBond.CharacterID);
			__instance.AddExtraData(LegacyBondProgress, legacyBond.Progress);
			__instance.AddExtraData(LegacyBondCharacterName, legacyBond.CharacterName);
		}
		
		[HarmonyPatch(nameof(Item.ProcessExtraData)), HarmonyPostfix]
		private static void Item_ProcessExtraData_Postfix(Item __instance) {
			string legacyChestIDStr = __instance.GetExtraData(LegacyBondLegacyChestID);
			string characterIDStr = __instance.GetExtraData(LegacyBondCharacterID);
			if (string.IsNullOrEmpty(legacyChestIDStr) || string.IsNullOrEmpty(characterIDStr)) {
				return;
			}
			
			string characterNameStr = __instance.GetExtraData(LegacyBondCharacterName);
			string progressStr = __instance.GetExtraData(LegacyBondProgress);
			int.TryParse(progressStr, out int progress);

			__instance.SetLegacyBond(new LegacyBond(legacyChestIDStr, characterIDStr, __instance.UID, progress, characterNameStr));
		}

	}
}