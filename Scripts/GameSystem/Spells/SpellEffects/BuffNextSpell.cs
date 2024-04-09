using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.SpellEffects
{

    //TODO: Maybe split this class
    public class BuffNextSpell : SpellEffect
    {
        private float _amount;
        private SpellProperties? _spellPropertyToBuff = null;
        private Damage? _damageSource = null;

        public BuffNextSpell(ref Creature caster, SpellProperties spellPropertyToBuff, float amount) : base(ref caster)
        {
            _amount = amount;
            _spellPropertyToBuff = spellPropertyToBuff;
        }

        public BuffNextSpell(ref Creature caster, Damage damageSourceToAdd) : base(ref caster)
        {
            _damageSource = damageSourceToAdd;
        }

        public override void OnStartCast(ref Spell spell)
        {
            if (character.OnEquipSpell == null)
                character.OnEquipSpell = AddBuffToNextSpell;

            else
                character.OnEquipSpell += AddBuffToNextSpell;
        }

        private void AddBuffToNextSpell(Spell spell)
        {
            if (character.OnStartCastSpell == null)
                character.OnStartCastSpell = RemoveBuff;

            else
                character.OnStartCastSpell += RemoveBuff;

            if (_damageSource != null)
                AddDamageSourceToSpell(spell);

            if (_spellPropertyToBuff != null)
                AddModifierToSpell(spell);
        }

        private void RemoveBuff(Spell spell)
        {
            character.OnEquipSpell -= AddBuffToNextSpell;

            character.OnStartCastSpell -= AddDamageSourceToSpell;
            character.OnStartCastSpell -= AddModifierToSpell;

            character.OnStartCastSpell -= RemoveBuff;
        }

        private void AddDamageSourceToSpell(Spell spell)
        {
            spell.AddDamageSource(_damageSource ?? default(Damage));
        }

        private void AddModifierToSpell(Spell spell)
        {
            spell.spellPropertyModifiersByName[_spellPropertyToBuff ?? default(SpellProperties)] += _amount;
        }
    }
}
