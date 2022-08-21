using BepInEx;
using BepInEx.Logging;
using FaerynModCauldron.LegacyUpgrade;
using HarmonyLib;
using SideLoader;
using UnityEngine;

namespace LegacyUpgrade {
	[BepInPlugin(GUID, NAME, VERSION)]
	public class LegacyUpgrade : BaseUnityPlugin {
		public const string GUID = "faeryn.legacyupgrade";
		public const string NAME = "LegacyUpgrade";
		public const string VERSION = "0.9.0";
		internal static ManualLogSource Log;

		internal void Awake() {
			Log = this.Logger;
			Log.LogMessage($"Starting {NAME} {VERSION}");
			InitializeSL();
			new Harmony(GUID).PatchAll();
		}
		
		private void InitializeSL() {
			SL.BeforePacksLoaded += SL_BeforePacksLoaded;
		}

		private void SL_BeforePacksLoaded() {
			SL_EnchantmentRecipe legacyEnchantment = new SL_EnchantmentRecipe() {
				EnchantmentID = LegacyUpgradeConstants.LegacyBondEnchantmentID,
				Name = "Legacy Bond",
				Description = "This item is prepared for a Legacy Bond.",
				IsEnchantingGuildRecipe = false,
				IncenseItemID = 6000240, // Morpho Incense
				PillarDatas = new [] {
					new SL_EnchantmentRecipe.PillarData() {
						Direction = SL_EnchantmentRecipe.Directions.North,
						IsFar = true
					},
					new SL_EnchantmentRecipe.PillarData() {
						Direction = SL_EnchantmentRecipe.Directions.North,
						IsFar = false
					},
					new SL_EnchantmentRecipe.PillarData() {
						Direction = SL_EnchantmentRecipe.Directions.East,
						IsFar = true
					},
					new SL_EnchantmentRecipe.PillarData() {
						Direction = SL_EnchantmentRecipe.Directions.South,
						IsFar = true
					},
					new SL_EnchantmentRecipe.PillarData() {
						Direction = SL_EnchantmentRecipe.Directions.South,
						IsFar = false
					},
					new SL_EnchantmentRecipe.PillarData() {
						Direction = SL_EnchantmentRecipe.Directions.West,
						IsFar = true
					}
				},
				CompatibleEquipment = new SL_EnchantmentRecipe.EquipmentData() {
					RequiredTag = "Item",
					Equipments = new[] {
						new SL_EnchantmentRecipe.IngredientData() {
							SelectorType = SL_EnchantmentRecipe.IngredientTypes.Tag,
							SelectorValue = "Item"
						}
					}
				},
				TimeOfDay = new []{ new Vector2(23, 1) },
				EnchantTime = 2f
			};
			legacyEnchantment.ApplyTemplate();
		}
	}
}