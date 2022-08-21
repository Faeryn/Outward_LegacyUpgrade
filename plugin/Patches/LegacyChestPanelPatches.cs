using FaerynModCauldron.LegacyUpgrade.Extensions;
using HarmonyLib;
using UnityEngine;

namespace FaerynModCauldron.LegacyUpgrade.Patches {
	[HarmonyPatch(typeof(LegacyChestPanel))]
	public static class LegacyChestPanelPatches {
		[HarmonyPatch(nameof(LegacyChestPanel.StartInit)), HarmonyPostfix]
		private static void LegacyChestPanel_StartInit_Postfix(LegacyChestPanel __instance) {
			FooterButtonHolder footer = __instance.GetComponentInChildren<FooterButtonHolder>();
			if (footer == null) {
				return;
			}
			
			InputDisplay upgradeInputDisplay = footer.InfoInputDisplay;
			Object.Destroy(upgradeInputDisplay.m_lblActionText.GetComponent<UILocalize>());
			upgradeInputDisplay.ActionText = "Legacy Bond";
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
				characterUI.ShowInfoNotification("This item is unable to be bound");
				return false;
			}

			if (item.TryGetLegacyBond(out LegacyBond legacyBond)) {
				if (!legacyBond.IsActive(character)) {
					characterUI.ShowInfoNotification("This item is bound to someone else");
				} else if (!legacyBond.IsComplete()) {
					characterUI.ShowInfoNotification("The Legacy Bond is not strong enough");
				} else {
					characterUI.ShowInfoNotification($"{item.Name} upgraded");
					UpgradeLegacyItem(legacyChest);
					__instance.RefreshContainers();
				}
			} else if (item.HasLegacyBondEnchantment()) {
				CreateLegacyBond(legacyChest, character, item);
				characterUI.ShowInfoNotification($"Legacy Bond established with {item.Name}");
			}
			return false;
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