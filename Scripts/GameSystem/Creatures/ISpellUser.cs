using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Characters.CharacterClasses;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Items;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells;
using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures
{
    public interface ISpellUser : IItemWielder
    {
        public PrimaryCharacterClass characterClass { get; }
        public List<Spell> preparedSpells { get; }

        public delegate void SpellCastEvent(Spell spell);
        public SpellCastEvent OnStartCastSpell { get; set; }
        public SpellCastEvent OnEquipSpell { get; set; }
    }
}