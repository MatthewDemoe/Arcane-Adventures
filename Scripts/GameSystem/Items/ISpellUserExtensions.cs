using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Items;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures
{
    public static class ISpellUserExtensions
    {
        public static bool IsWieldingWeaponNeededToCastSpells(this ISpellUser spellUser, HandSide handSide)
        {
            var weaponInHandSide = handSide == HandSide.Left ? spellUser.leftHandItem : spellUser.rightHandItem;

            return weaponInHandSide != null &&
                weaponInHandSide is Weapon weapon &&
                (
                    (weapon.isArcaneFocus && spellUser.characterClass.identifier == Identifiers.PrimaryCharacterClass.Wizard) || 
                    (!weapon.isArcaneFocus && spellUser.characterClass.identifier == Identifiers.PrimaryCharacterClass.Warrior)
                );
        }
    }
}