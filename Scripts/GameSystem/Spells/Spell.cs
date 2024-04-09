using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;
using com.AlteredRealityLabs.ArcaneAdventures.Combat;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.SpellEffects;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Characters;
using System.Collections.Generic;
using System.Linq;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells
{
    public enum SpellType { Assault, Control, Enhancement }
    public enum Element { Inferno, Ocean, Earth, Storm }
    public enum SpellProperties { Damage, Range, AreaOfEffect, Force, }

    public struct Damage
    {
        public Damage(float damage, DamageType damageType) 
        {
            this.damage = damage;
            this.damageType = damageType;
        }

        public float damage;
        public DamageType damageType;
    }

    //TODO: Split up into more bite-sized derived classes
    public abstract class Spell
    {
        public Spell()
        {
            caster = null;
        }
        public Spell(ref Creature _caster)
        {
            caster = _caster;
            _damageSources = new List<Damage>() { _damage, };
        }

        public abstract string name { get; }
        public abstract string effectDescription { get; }
        public abstract int spellLevel { get; }

        public abstract SpellType spellType { get; }

        public abstract float initialCost { get; }

        public virtual float channelDuration { get; } = 0.0f;
        public virtual float channelCost { get; } = 0.0f;
        public virtual float channelInterval { get; } = 0.0f;
        public bool isChanneledFully => _channelDurationTimer >= channelDuration;
        private float _channelIntervalTimer = 1.0f;
        private float _channelDurationTimer = 0.0f;


        public virtual float upkeepCost { get; } = 0.0f;
        public virtual bool upkeepStarted { get; private set; } = false;
        public virtual float upkeepInterval { get; } = 1.0f;
        private const float UpkeepCostInterval = 1.0f;
        private float _upkeepEffectTimer = 1.0f;
        private float _upkeepCostTimer = 1.0f;

        public virtual bool hasDuration { get; } = false;
        public virtual bool hasUpkeep { get; } = false;

        protected virtual float baseDuration { get; } = 0.0f;
        public float duration => baseDuration * durationMultiplier;
        public float durationMultiplier = 1.0f;

        public abstract float cooldown { get; }
        protected virtual Damage _damage { get; } = new Damage(0.0f, DamageType.Magical);

        public virtual List<Element> elements { get; } = new List<Element>();
        protected virtual bool scaleDamageOverDuration => false;

        public virtual float knockbackDistance { get; } = 0.0f;

        protected virtual float _range { get; }  = 0.0f;
        public float range => _range + spellPropertyModifiersByName[SpellProperties.Range];

        protected virtual float _radius { get; } = 0.0f;
        public float radius => _radius + spellPropertyModifiersByName[SpellProperties.AreaOfEffect];

        public virtual bool isRangeCentered { get; } = false;
        public virtual bool isSelfTargeted { get; } = false;


        protected virtual float _force { get; } = CombatSettings.Spells.ProjectileForce;
        public float force => _force * spellPropertyModifiersByName[SpellProperties.Force];

        public bool persistentEffectIsActive = false;

        private float _elapsedTime = 0.0f;
        protected  Creature caster;
        protected Character character => caster as Character;

        public abstract Spell CreateSpell(ref Creature _caster);

        public List<SpellEffect> spellEffects = new List<SpellEffect>() { };

        protected List<Damage> _damageSources = new List<Damage>();
        public List<Damage> modifiedDamageSources => _damageSources.Select((damage) => new Damage(damage.damage / (scaleDamageOverDuration ? baseDuration : 1.0f) * spellPropertyModifiersByName[SpellProperties.Damage], damage.damageType)).ToList();

        public Dictionary<SpellProperties, float> spellPropertyModifiersByName { get; } = new Dictionary<SpellProperties, float>()
        {
            { SpellProperties.Damage, 1.0f },
            { SpellProperties.Range, 0.0f },
            { SpellProperties.AreaOfEffect, 0.0f },
            { SpellProperties.Force, 1.0f },
        };

        public bool IsSpellActive(float deltaTime)
        {       
            _elapsedTime += deltaTime;

            if ((hasDuration && (_elapsedTime <= baseDuration)) || persistentEffectIsActive || (hasUpkeep && upkeepStarted))
                return true;

            DurationEnd();
            return false; 
        }

        public virtual bool StartCast(Spell spell)
        {
            if (!caster.TryAdjustSpirit(initialCost, out float _))
                return false;

            caster.OnSpiritChange();

            foreach (SpellEffect spellEffect in spellEffects)
            {
                spellEffect.OnStartCast(ref spell);
            }

            upkeepStarted = true;

            return true;
        }

        public virtual bool ChannelCast(float deltaTime)
        {
            _channelIntervalTimer += deltaTime;
            _channelDurationTimer += deltaTime;

            if (_channelIntervalTimer < channelInterval)
                return false;

            if (!caster.TryAdjustSpirit(channelCost, out float _))
                return false;

            _channelIntervalTimer -= channelInterval;
            caster.OnSpiritChange();

            foreach (SpellEffect spellEffect in spellEffects)
            {
                spellEffect.OnChannel();
            }

            return true;
        }

        public bool TryToPerformUpkeep(float deltaTime)
        {
            if (!upkeepStarted)
                return false;

            InvokeUpkeepEffects(deltaTime);
            return TryToPayForUpkeep(deltaTime); 
        }

        protected bool TryToPayForUpkeep(float deltaTime)
        {       
            _upkeepCostTimer += deltaTime;

            if (_upkeepCostTimer < UpkeepCostInterval)
                return false;

            if (!caster.TryAdjustSpirit(upkeepCost, out float _))
            {
                DurationEnd();
                return false;
            }            

            _upkeepCostTimer -= upkeepInterval;

            caster.OnSpiritChange();
            return true;
        }

        protected void InvokeUpkeepEffects(float deltaTime)
        {
            _upkeepEffectTimer += deltaTime;

            if (_upkeepEffectTimer < upkeepInterval)
                return;

            _upkeepEffectTimer -= upkeepInterval;

            foreach (SpellEffect spellEffect in spellEffects)
            {
                spellEffect.OnUpkeep();
            };
        }

        public virtual void EndCast()
        {
            foreach (SpellEffect spellEffect in spellEffects)
            {
                spellEffect.OnCastEnd();
            }
        }

        public virtual void DurationEnd()
        {
            if (upkeepStarted == false)
                return;

            upkeepStarted = false;

            foreach (SpellEffect spellEffect in spellEffects)
            {
                spellEffect.OnDurationEnd();
            }
        }

        public void AddDamageSource(Damage newDamageSource)
        {
            _damageSources.Add(newDamageSource);
        }

        public Creature GetCaster()
        {
            return caster;
        }

        public void StopUpkeep()
        {
            DurationEnd();
        }
    }
}
