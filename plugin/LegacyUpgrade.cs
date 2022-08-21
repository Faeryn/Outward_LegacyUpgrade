using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using SideLoader;
using UnityEngine;

namespace LegacyUpgrade {
	[BepInPlugin(GUID, NAME, VERSION)]
	public class LegacyUpgrade : BaseUnityPlugin {
		public const string GUID = "faeryn.legacyupgrade";
		public const string NAME = "LegacyUpgrade";
		public const string VERSION = "0.9.1";
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
				TimeOfDay = new [] {
					new Vector2(11, 13)
				},
				EnchantTime = 5f
			};
			legacyEnchantment.ApplyTemplate();
			

			SL_EnchantmentRecipeItem enchantmentRecipeItem = new SL_EnchantmentRecipeItem {
				Target_ItemID = 5800003,
				New_ItemID = LegacyUpgradeConstants.LegacyBondEnchantmentRecipeItemID,
				Name = "Enchanting: Legacy Bond",
				StatsHolder = new SL_ItemStats {
					BaseValue = 100,
					RawWeight = 0,
					MaxDurability = -1
				},
				Recipes = new [] {
					legacyEnchantment.EnchantmentID
				}
			};
			enchantmentRecipeItem.ApplyTemplate();

			SL_DropTable recipeDT = new SL_DropTable {
				UID = LegacyUpgradeConstants.EnchantmentDroptableUID,
				GuaranteedDrops = {
					new SL_ItemDrop {
						MinQty = 1,
						MaxQty = 1,
						DroppedItemID = enchantmentRecipeItem.New_ItemID
					}
				}
			};
			recipeDT.ApplyTemplate();
			
			SL_DropTableAddition potionAndRecipeForMerchants = new SL_DropTableAddition {
				SelectorTargets = {"-MSrkT502k63y3CV2j98TQ", "G_GyAVjRWkq8e2L8WP4TgA"}, // Soroborean Caravanner
				DropTableUIDsToAdd = {recipeDT.UID}
				
			};
			potionAndRecipeForMerchants.ApplyTemplate();
		}
	}
}