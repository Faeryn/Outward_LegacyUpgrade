using System;

namespace LegacyUpgrade {
	[Serializable]
	public class LegacyBond {
		public UID LegacyChestID { get; }
		public UID CharacterID { get; }
		public UID ItemID { get; }
		public int Progress { get; private set; }
		public string CharacterName { get; private set; }

		public LegacyBond(UID itemID) {
			ItemID = itemID;
		}

		public LegacyBond(UID legacyChestID, UID characterID, UID itemID, int progress, string characterName) {
			LegacyChestID = legacyChestID;
			ItemID = itemID;
			CharacterID = characterID;
			Progress = progress;
			CharacterName = characterName;
		}

		public string GetProgressText() {
			float progressRatio = (float)Progress/LegacyUpgradeConstants.ProgressRequired;
			int progressLevel;
			if (progressRatio < 0.2f) {
				progressLevel = 0;
			} else if (progressRatio < 0.4f) {
				progressLevel = 1;
			} else if (progressRatio < 0.6f) {
				progressLevel = 2;
			} else if (progressRatio < 0.8f) {
				progressLevel = 3;
			} else if (progressRatio < 1f) {
				progressLevel = 4;
			} else {
				progressLevel = 5;
			}
			return LocalizationManager.Instance.GetLoc($"{LegacyUpgrade.GUID}.bond.progress_{progressLevel}");
		}

		public bool IsComplete() {
			return Progress >= LegacyUpgradeConstants.ProgressRequired;
		}

		public bool IsActive(Character character) {
			return character.UID == CharacterID;
		}

		public bool TryAddProgress(Character character, int progress) {
			if (!IsActive(character)) {
				return false;
			}

			if (string.IsNullOrEmpty(CharacterName)) {
				CharacterName = character.Name;
			}
			
			Progress += progress;
			return true;
		}
	}
}