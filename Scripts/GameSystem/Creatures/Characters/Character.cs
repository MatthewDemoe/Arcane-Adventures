using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Characters.CharacterClasses;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Items;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells;
using com.AlteredRealityLabs.ArcaneAdventures.Appearance.UMA;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits;
using System.Collections.Generic;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Characters
{
    [System.Serializable]
    public class Character : Creature, ISpellUser
    {
        public Item leftHandItem { get; set; }
        public Item rightHandItem { get; set; }

        bool isWritable = true;
        public PrimaryCharacterClass characterClass { get; protected set; }

        [SerializeField]
        private Identifiers.PrimaryCharacterClass classIdentifier;

        public List<Spell> preparedSpells { get; protected set; }

        [SerializeField]
        private Identifiers.Gender _gender = Identifiers.Gender.NotSet;
        public Identifiers.Gender gender { get { return _gender; } set { _gender = isWritable ? value : _gender; } }

        public Trait trait { get; private set; }
        public List<Trait> archetypeTraits => characterClass.classArchetypes[0].traits;

        [SerializeField]
        private CharacterAppearance _appearance = CharacterAppearance.Custom;
        public CharacterAppearance appearance { get { return _appearance; } set { _appearance = isWritable ? value : _appearance; } }

        public  Dictionary<Wardrobe.Feature, int> selectedWardrobeFeatures = new Dictionary<Wardrobe.Feature, int>()
        {
            { Wardrobe.Feature.Gender, 0 },
            { Wardrobe.Feature.Top, 0 },
            { Wardrobe.Feature.Bottom, 0 },
        };

        private  ISpellUser.SpellCastEvent _onStartCastSpell = (Spell spell) => { };
        private ISpellUser.SpellCastEvent _onEquipSpell = (Spell spell) => { };

        public ISpellUser.SpellCastEvent OnStartCastSpell { get => _onStartCastSpell; set => _onStartCastSpell = value; }
        public ISpellUser.SpellCastEvent OnEquipSpell { get => _onEquipSpell; set => _onEquipSpell = value; }

        public Character(Stats stats, Identifiers.Race race, Identifiers.Gender gender, Identifiers.PrimaryCharacterClass characterClass, Item leftHandItem, Item rightHandItem, CharacterAppearance appearance) 
            : base(stats, race)
        {
            this.gender = gender;
            this.characterClass = PrimaryCharacterClass.Get(characterClass);
            this.classIdentifier = characterClass;

            this.leftHandItem = leftHandItem;
            this.rightHandItem = rightHandItem;

            this.appearance = appearance;

            preparedSpells = new List<Spell>(this.characterClass.spells) { BasicSpell.Instance, BasicStaffSpell.Instance, StatusConditionSpell.Instance };

            trait = Trait.GetTrait(this.characterClass.classArchetypes[0].traits[0], this);
        }

        public Character(Character copyCharacter) : this(copyCharacter.stats, copyCharacter.race, copyCharacter.gender, copyCharacter.classIdentifier, copyCharacter.leftHandItem, copyCharacter.rightHandItem, copyCharacter.appearance)
        {
        }

        public void SetUnwritable()
        {
            isWritable = false;
        }

        public override int GetWeaponDamage()
        {
            int damage = 0;

            if (leftHandItem != null && leftHandItem is Weapon)
                damage += (leftHandItem as Weapon).GetDamage();

            if (rightHandItem != null && rightHandItem is Weapon)
                damage += (rightHandItem as Weapon).GetDamage();

            if ((leftHandItem is Weapon) && (rightHandItem is Weapon))
                damage /= 2;

            return damage;
        }

        public void SetTrait(Trait newTrait)
        {
            trait.Disable();
            trait = newTrait;
        }
    }
}
