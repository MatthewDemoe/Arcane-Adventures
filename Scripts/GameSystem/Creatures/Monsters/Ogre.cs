using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Items;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.EnemySpells;
using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Monsters
{
    public class Ogre : HumanoidMonster, ISpellUser
    {
        public List<Spell> preparedSpells { get; protected set; }
        public Characters.CharacterClasses.PrimaryCharacterClass characterClass { get; protected set; } = Characters.CharacterClasses.PrimaryCharacterClass.Get(Identifiers.PrimaryCharacterClass.Warrior);

        private ISpellUser.SpellCastEvent _onStartCastSpell = (Spell spell) => { };
        private ISpellUser.SpellCastEvent _onEquipSpell = (Spell spell) => { };

        public ISpellUser.SpellCastEvent OnStartCastSpell { get => _onStartCastSpell; set => _onStartCastSpell = value; }
        public ISpellUser.SpellCastEvent OnEquipSpell { get => _onEquipSpell; set => _onEquipSpell = value; }

        public Ogre(Stats stats, Identifiers.Race race, Item leftHandItem, Item rightHandItem) :
            base(stats, race, leftHandItem, rightHandItem)
        {
            traits = new List<Trait>()
            {
                Trait.GetTrait(BossMonster.Instance, this),
                Trait.GetTrait(Heavy.Instance, this)
            };

            preparedSpells = new List<Spell>()
            {
                TreeSlam.Instance,
                Stomp.Instance, 
                Charge.Instance
            };
        }
    }
}