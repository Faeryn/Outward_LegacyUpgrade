using System.Collections.Generic;
using FaerynModCauldron.LegacyUpgrade.Extensions;
using HarmonyLib;
using UnityEngine;

namespace FaerynModCauldron.LegacyUpgrade.Patches {
	[HarmonyPatch(typeof(Character))]
	public static class CharacterPatches {

		[HarmonyPatch(nameof(Character.OnReceiveHit)), HarmonyPostfix]
		private static void Character_OnReceiveHit_Postfix(Character __instance, Weapon _weapon, float _damage, DamageList _damageList, Vector3 _hitDir, Vector3 _hitPoint, float _angle, float _angleDir, Character _dealerChar, float _knockBack) {
			if (_dealerChar == null || _dealerChar.Inventory == null || _dealerChar.Inventory.Equipment == null) {
				return;
			}
			List<LegacyBond> legacyBonds = new List<LegacyBond>();
			foreach (EquipmentSlot slot in _dealerChar.Inventory.Equipment.EquipmentSlots) {
				if (slot == null) {
					continue;
				}
				Equipment item = slot.EquippedItem;
				if (item != null && item.TryGetLegacyBond(out LegacyBond legacyBond) && legacyBond.IsActive(_dealerChar)) {
					legacyBonds.Add(legacyBond);
				}
			}
			int numLegacyBonds = legacyBonds.Count;
			foreach (LegacyBond legacyBond in legacyBonds) {
				legacyBond.TryAddProgress(_dealerChar, Mathf.RoundToInt(_damage/numLegacyBonds));
			}
		}

	}
}