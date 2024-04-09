using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Items;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.WizardSpells;
using com.AlteredRealityLabs.ArcaneAdventures.Identifiers;
using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Monsters
{
    public class OrcShaman : HumanoidMonster, ISpellUser
    {
        public Characters.CharacterClasses.PrimaryCharacterClass characterClass { get; protected set; } = Characters.CharacterClasses.PrimaryCharacterClass.Get(PrimaryCharacterClass.Wizard);
        public List<Spell> preparedSpells { get; protected set; }

        private ISpellUser.SpellCastEvent _onStartCastSpell = (Spell spell) => { };
        private ISpellUser.SpellCastEvent _onEquipSpell = (Spell spell) => { };

        public ISpellUser.SpellCastEvent OnStartCastSpell { get => _onStartCastSpell; set => _onStartCastSpell = value; }
        public ISpellUser.SpellCastEvent OnEquipSpell { get => _onEquipSpell; set => _onEquipSpell = value; }

        public OrcShaman(Stats stats, Race race, Item leftHandItem, Item rightHandItem) :
            base(stats, race, leftHandItem, rightHandItem)
        {
            preparedSpells = new List<Spell>()
             {
                 BasicSpell.Instance,
                 BasicStaffSpell.Instance,
                 BasicHealSpell.Instance,
                 CursedBlood.Instance,
                 ShadowBind.Instance,
             };
        }
    }
}