using HarmonyLib;
using LegacyUpgrade.Extensions;
using UnityEngine;

namespace LegacyUpgrade.Patches {
	[HarmonyPatch(typeof(LegacyChestPanel))]
	public static class LegacyChestPanelPatches {
		[HarmonyPatch(nameof(LegacyChestPanel.StartInit)), HarmonyPostfix]
		private static void LegacyChestPanel_StartInit_Postfix(LegacyChestPanel __instance) {
			FooterButtonHolder footer = __instance.GetComponentInChildren<FooterButtonHolder>();
			if (footer == null) {
				return;
			}
			
			InputDisplay upgradeInputDisplay = footer.InfoInputDisplay;
			upgradeInputDisplay.m_lblActionText.GetComponent<UILocalize>().Key = Loc($"{LegacyUpgrade.GUID}.legacy_chest.action.legacy_bond");
			upgradeInputDisplay.ActionText = Loc("action.legacy_bond");
		}

		[HarmonyPatch(nameof(LegacyChestPanel.OnInfoInput)), HarmonyPrefix]
		private static bool LegacyChestPanel_OnInfoInput_Prefix(LegacyChestPanel __instance) {
			if (!__instance.m_refItemInChest) {
				return false;
			}

			ItemContainer legacyChest = __instance.m_refLegacyChest;
			Item item = __instance.m_refItemInChest;
			Character character = __instance.LocalCharacter;
			CharacterUI characterUI = character.CharacterUI;
			if (!item.HasLegacyItem()) {
				characterUI.ShowInfoNotification(Loc("fail.wrong_item"));
				return false;
			}

			if (item.TryGetLegacyBond(out LegacyBond legacyBond)) {
				if (!legacyBond.IsActive(character)) {
					characterUI.ShowInfoNotification(Loc("fail.wrong_player"));
				} else if (!legacyBond.IsComplete()) {
					characterUI.ShowInfoNotification(Loc("fail.weak_bond"));
				} else {
					characterUI.ShowInfoNotification(Loc("item_upgraded"));
					UpgradeLegacyItem(legacyChest);
					__instance.RefreshContainers();
				}
			} else if (item.HasLegacyBondEnchantment()) {
				CreateLegacyBond(legacyChest, character, item);
				characterUI.ShowInfoNotification(Loc("bond_success"));
			} else {
				characterUI.ShowInfoNotification(Loc("fail.no_enchantment"));
			}
			return false;
		}

		private static string Loc(string key, params string[] args) {
			return LocalizationManager.Instance.GetLoc($"{LegacyUpgrade.GUID}.legacy_chest.{key}", args);
		}

		private static void CreateLegacyBond(ItemContainer legacyChest, Character character, Item item) {
			item.SetLegacyBond(new LegacyBond(legacyChest.UID, character.UID, item.UID, 0, character.Name));
		}

		private static void UpgradeLegacyItem(ItemContainer legacyChest) {
			if (legacyChest.ItemCount < 1) {
				return;
			}
			Item item = legacyChest.GetContainedItems()[0];
			if (item == null || !item.HasLegacyItem()) {
				return;
			}
			Item newItem = ItemManager.Instance.GenerateItemNetwork(item.LegacyItemID);
			ItemManager.Instance.DestroyItem(item);
			newItem.ChangeParent(legacyChest.transform);
		}
	}
}