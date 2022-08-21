namespace LegacyUpgrade {
	public static class LegacyUpgradeConstants {
		public const int ProgressRequired = 1000;
		public const int LegacyBondEnchantmentID = -12200;
		public const int LegacyBondEnchantmentRecipeItemID = -12210;
		
		public const string EnchantmentRecipeName = LegacyUpgrade.GUID+".legacyUpgradeRecipeItem";
		public const string EnchantmentDroptableUID = EnchantmentRecipeName+".droptable";
	}
}