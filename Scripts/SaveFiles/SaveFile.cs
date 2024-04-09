using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.Appearance.UMA;
using System;
using UnityEngine;
using com.AlteredRealityLabs.ArcaneAdventures.ScriptableObjects;

namespace com.AlteredRealityLabs.ArcaneAdventures.SaveFiles
{
    public class SaveFile
    {
        public const int NumberOfSaveFiles = 5;
        public PlayerCharacter playerCharacter { get; }
        public int slotNumber;

        private SaveFile(int slotNumber, PlayerCharacter playerCharacter)
        {
            this.slotNumber = slotNumber;
            this.playerCharacter = playerCharacter;
        }

        public static SaveFile Load(int slotNumber)
        {
            if (slotNumber < 0 || slotNumber > NumberOfSaveFiles)//TODO: Should really check for <1, but the mirror in character creation uses 0 so that needs to be fixed before this can take its proper form without making breaking changes.
            {
                throw new Exception($"Slot number {slotNumber} was out of range (expected 0-{NumberOfSaveFiles})");
            }

            var playerCharacter = GetPlayerCharacter(slotNumber);

            if (playerCharacter == null)
                return null;

            return new SaveFile(slotNumber, playerCharacter);
        }

        public static SaveFile GetNewSaveFile(int slotNumber)
        {
            return new SaveFile(slotNumber, PlayerCharacter.BasicPlayerCharacter);
        }

        //TODO: pull necessary information from PlayerCharacter, and serialize SaveFile instead. 
        public static void Save(int slotNumber, PlayerCharacter playerCharacter)
        {
            string jsonCharacter = JsonUtility.ToJson(playerCharacter);

            PlayerPrefs.SetString(PlayerPrefsKeys.PlayerCharacter + slotNumber, jsonCharacter);

            PlayerPrefs.SetString(PlayerPrefsKeys.RightHandItem + slotNumber, playerCharacter.rightHandItem?.name);
            PlayerPrefs.SetString(PlayerPrefsKeys.LeftHandItem + slotNumber, playerCharacter.leftHandItem?.name);

            PlayerPrefs.SetInt(PlayerPrefsKeys.WardrobeTop + slotNumber, playerCharacter.selectedWardrobeFeatures[Wardrobe.Feature.Top]);
            PlayerPrefs.SetInt(PlayerPrefsKeys.WardrobeBottom + slotNumber, playerCharacter.selectedWardrobeFeatures[Wardrobe.Feature.Bottom]);

            PlayerPrefs.Save();
        }

        public static void Delete(int slotNumber)
        {
            PlayerPrefs.DeleteKey(PlayerPrefsKeys.PlayerCharacter + slotNumber);

            PlayerPrefs.Save();
        }

        public static bool HasSaveFileInSlot(int slotNumber) => PlayerPrefs.HasKey(PlayerPrefsKeys.PlayerCharacter + slotNumber);

        private static PlayerCharacter GetPlayerCharacter(int slotNumber)
        {
            if (!HasSaveFileInSlot(slotNumber))
                return null;

            PlayerCharacter player = new PlayerCharacter(JsonUtility.FromJson<PlayerCharacter>(PlayerPrefs.GetString(PlayerPrefsKeys.PlayerCharacter + slotNumber)));

            var leftHandItemName = PlayerPrefs.GetString(PlayerPrefsKeys.LeftHandItem + slotNumber);
            var rightHandItemName = PlayerPrefs.GetString(PlayerPrefsKeys.RightHandItem + slotNumber);

            player.leftHandItem = string.IsNullOrEmpty(leftHandItemName) ? null : ItemCache.GetItem(leftHandItemName);
            player.rightHandItem = string.IsNullOrEmpty(rightHandItemName) ? null : ItemCache.GetItem(rightHandItemName);

            player.selectedWardrobeFeatures[Wardrobe.Feature.Top] = PlayerPrefs.GetInt(PlayerPrefsKeys.WardrobeTop + slotNumber);
            player.selectedWardrobeFeatures[Wardrobe.Feature.Bottom] = PlayerPrefs.GetInt(PlayerPrefsKeys.WardrobeBottom + slotNumber);

            return player;
        }
    }
}
