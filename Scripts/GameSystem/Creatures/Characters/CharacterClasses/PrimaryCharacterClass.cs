using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Characters.CharacterClasses.WarriorClasses;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Characters.CharacterClasses.WizardClasses;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Items;
using System;
using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Characters.CharacterClasses
{
    public abstract class PrimaryCharacterClass : CharacterClass
    {
        public static Wizard Wizard => Wizard.Instance;
        public static Warrior Warrior => Warrior.Instance;

        public abstract List<EquipmentSet> equipmentSets { get; }
        public abstract List<CharacterClassArchetype> classArchetypes { get; }
        public abstract Identifiers.PrimaryCharacterClass identifier { get; }
        public abstract CharacterClassAttributes classAttributes { get; }

        public static PrimaryCharacterClass Get(Identifiers.PrimaryCharacterClass inClass)
        {
            switch (inClass)
            {
                case (Identifiers.PrimaryCharacterClass.Warrior):
                    return Warrior;

                case (Identifiers.PrimaryCharacterClass.Wizard):
                    return Wizard;

                default:
                    throw new ArgumentException("Class doesn't exist: " + inClass);
            }
        }
    }
}