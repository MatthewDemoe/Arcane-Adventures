using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Characters.CharacterClasses.WarriorClasses;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Characters.CharacterClasses.WizardClasses;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Characters.CharacterClasses
{
    public abstract class CharacterClassArchetype : CharacterClass
    {
        public abstract string name { get; }
        public abstract string classDescription { get; }

        public abstract Identifiers.CharacterClassArchetype identifier { get; }

        public static ArcaneAdept ArcaneAdept => ArcaneAdept.Instance;
        public static Elementalist Elementalist => Elementalist.Instance;
        public static EssenceMage EssenceMage => EssenceMage.Instance;

        public static Berserker Berserker => Berserker.Instance;
        public static Sentinel Sentinel => Sentinel.Instance;
        public static WeaponMaster WeaponMaster => WeaponMaster.Instance;
    }
}