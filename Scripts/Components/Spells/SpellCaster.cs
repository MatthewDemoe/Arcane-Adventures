using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Targeting;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Items;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells
{
    public abstract class SpellCaster : MonoBehaviour
    {
        protected Creature castingCreature;
        protected ISpellUser castingCharacter;
        public ItemEquipper.ItemEquipper itemEquipper { get; protected set; }

        protected Dictionary<HandSide, Spell> equippedSpellByHandSide = new Dictionary<HandSide, Spell>() { { HandSide.Left, null }, { HandSide.Right, null } };
        protected Dictionary<HandSide, Spell> equippedSpellReferenceByHandSide = new Dictionary<HandSide, Spell>() { { HandSide.Left, null }, { HandSide.Right, null } };
        public Dictionary<HandSide, GameObject> spellObjectByHandSide { get; protected set; } = new Dictionary<HandSide, GameObject>() { { HandSide.Left, null }, { HandSide.Right, null } };
        public Dictionary<HandSide, PhysicalSpell> equippedGameSpellByHandSide = new Dictionary<HandSide, PhysicalSpell>() { { HandSide.Left, null }, { HandSide.Right, null } };
        public Dictionary<HandSide, bool> isCastingByHandSide { get; protected set; } = new Dictionary<HandSide, bool>() { { HandSide.Left, false }, { HandSide.Right, false } };
        protected static string[] spellExceptions = new string[] { Prefabs.Spells.Basic, Prefabs.Spells.StatusConditionSpell };

        public bool isCasting => isCastingByHandSide.Values.Any(isCasting => isCasting);

        protected List<PhysicalSpell> activeUpkeepSpells = new List<PhysicalSpell>();
        protected List<GameObject> spellPrefabs = new List<GameObject>();

        protected bool _isInitialized = false;
        public SpellTargetTypeSwitcher spellTargetTypeSwitcher { get; protected set; }

        public SpellCooldownTracker spellCooldownTracker { get; protected set; }

        public UnityEvent<PhysicalSpell, HandSide> OnSpellCreated = new UnityEvent<PhysicalSpell, HandSide>();

        public Transform SpellSourceTransform(HandSide handSide) => itemEquipper.GetWeaponInHand(handSide).spellSource.transform;
        public Transform WeaponTransform(HandSide handSide) => itemEquipper.GetWeaponInHand(handSide).transform;

        public virtual float GetAimAngleAdjust => 0.0f;

        protected virtual void Start()
        {
            itemEquipper = GetComponent<ItemEquipper.ItemEquipper>();

            _isInitialized = InitializeCharacterSpells();

            spellTargetTypeSwitcher = new SpellTargetTypeSwitcher(this);
        }

        private void Update()
        {
            spellCooldownTracker.UpdateCooldowns();

            SpellUpkeep();
        }

        public virtual bool InitializeCharacterSpells()
        {
            castingCreature = GetComponentInParent<CreatureReference>().creature;

            if (castingCreature == null || !(castingCreature is ISpellUser))
                return false;

            castingCharacter = castingCreature as ISpellUser;
            spellCooldownTracker = new SpellCooldownTracker();
            spellCooldownTracker.InitializeSpellList(castingCharacter.preparedSpells);

            castingCharacter.preparedSpells.ForEach(spell => spellPrefabs.Add(spellExceptions.Any(exceptionName => spell.name.Contains(exceptionName)) 
                ? Prefabs.Spells.Load(spell.name) : Prefabs.Spells.Load(castingCharacter.characterClass.classAttributes, spell)));

            return true;
        }

        public bool TryEquipSpell(Spell spellToEquip, HandSide handSide)
        {
            HandSide approvedHandSide = handSide;

            if (!castingCharacter.IsWieldingWeaponNeededToCastSpells(approvedHandSide))
                approvedHandSide = handSide == HandSide.Left ? HandSide.Right : HandSide.Left;

            if (!castingCharacter.IsWieldingWeaponNeededToCastSpells(approvedHandSide))
                return false;

            CreateSpell(spellToEquip, approvedHandSide);

            if(!equippedGameSpellByHandSide[approvedHandSide].isChanneled)
                StartCast(approvedHandSide);

            return true;
        }

        protected virtual void CreateSpell(Spell spellToEquip, HandSide handSide)
        {
            equippedSpellReferenceByHandSide[handSide] = spellToEquip;
            equippedSpellByHandSide[handSide] = spellToEquip.CreateSpell(ref castingCreature);

            if (castingCharacter.OnEquipSpell != null)
                castingCharacter.OnEquipSpell.Invoke(equippedSpellByHandSide[handSide]);

            InstantiateSpellObject(handSide);

            equippedGameSpellByHandSide[handSide] = spellObjectByHandSide[handSide].GetComponent<PhysicalSpell>();
            equippedGameSpellByHandSide[handSide].InitializeSpellInformation(equippedSpellByHandSide[handSide], this, handSide);

            OnSpellCreated.Invoke(equippedGameSpellByHandSide[handSide], handSide);
        }

        protected virtual void CreateBasicSpell(HandSide handSide)
        {
            equippedSpellReferenceByHandSide[handSide] = itemEquipper.GetItemInHand(HandSide.Right).GetComponent<PhysicalWeapon>().basicSpellInstance;

            if (spellCooldownTracker.IsOnCooldownBySpell(equippedSpellReferenceByHandSide[handSide]))
                return;

            equippedSpellByHandSide[handSide] = equippedSpellReferenceByHandSide[handSide].CreateSpell(ref castingCreature);
            InstantiateSpellObject(handSide);

            equippedGameSpellByHandSide[handSide] = spellObjectByHandSide[handSide].GetComponent<PhysicalSpell>();
            equippedGameSpellByHandSide[handSide].InitializeSpellInformation(equippedSpellByHandSide[handSide], this, handSide);

            OnSpellCreated.Invoke(equippedGameSpellByHandSide[handSide], handSide);
        }

        private void InstantiateSpellObject(HandSide handSide)
        {
            GameObject prefab = spellPrefabs.Find(gameObject => gameObject.name.Contains(equippedSpellByHandSide[handSide].name));

            spellObjectByHandSide[handSide] = Instantiate(prefab, SpellSourceTransform(handSide));
        }

        public virtual void PrepareSpell(Spell spell) { }

        public virtual void UnPrepareSpell(Spell spell) { }

        public virtual void StartCast(HandSide handSide)
        {
            if (!_isInitialized || !castingCharacter.IsWieldingWeaponNeededToCastSpells(handSide) || !castingCreature.isSpellcastingEnabled)
                return;

            if (equippedGameSpellByHandSide[handSide] == null ? true : equippedGameSpellByHandSide[handSide].hasBeenCast)
            {
                if (castingCharacter.characterClass.identifier == Identifiers.PrimaryCharacterClass.Wizard)
                    CreateBasicSpell(handSide);

                else
                    return;
            }

            if (spellCooldownTracker.IsOnCooldownBySpell(equippedSpellReferenceByHandSide[handSide]) || !equippedSpellByHandSide[handSide].StartCast(equippedSpellByHandSide[handSide]))
            {
                equippedSpellByHandSide[handSide] = null;
                equippedGameSpellByHandSide[handSide] = null;

                return;
            }

            if (castingCharacter.OnStartCastSpell != null)
                castingCharacter.OnStartCastSpell(equippedSpellByHandSide[handSide]);

            equippedGameSpellByHandSide[handSide].OnBeginCast.Invoke();

            if (equippedSpellByHandSide[handSide].hasUpkeep)
            {
                activeUpkeepSpells.Add(equippedGameSpellByHandSide[handSide]);
                spellCooldownTracker.ResetCooldown(equippedSpellReferenceByHandSide[handSide]);
            }

            else
                isCastingByHandSide[handSide] = true;

            if (equippedGameSpellByHandSide[handSide].finishCastImmediately)
                FinishCast(handSide);
        }

        public void ChannelSpell(HandSide handSide)
        {
            if ((equippedSpellByHandSide[handSide] == null) || !isCastingByHandSide[handSide])
                return;

            if (!equippedSpellByHandSide[handSide].ChannelCast(Time.deltaTime))
                return;

            equippedGameSpellByHandSide[handSide].OnChannel.Invoke();
        }

        public virtual void SpellUpkeep()
        {
            foreach (var activeUpkeepSpell in activeUpkeepSpells)
            {
                if (activeUpkeepSpell.correspondingSpell.TryToPerformUpkeep(Time.deltaTime))
                    activeUpkeepSpell.OnUpkeep.Invoke();
            }
        }

        public bool CancelActiveSpells()
        {
            if (equippedSpellByHandSide.Values.ToList().TrueForAll((spell) => spell == null) || isCastingByHandSide.Values.ToList().TrueForAll((isCasting) => !isCasting))
                return false;

            CancelActiveSpell(HandSide.Left);
            CancelActiveSpell(HandSide.Right);

            return true;
        }

        public void CancelActiveSpell(HandSide handSide)
        {
            spellTargetTypeSwitcher.DestroyCurrentTargeter(handSide);
            Destroy(spellObjectByHandSide[handSide]);
            isCastingByHandSide[handSide] = false;
        }

        public void CancelUpkeepSpells()
        {
            foreach (var activeUpkeepSpell in activeUpkeepSpells)
            {
                activeUpkeepSpell.OnDurationEnd.Invoke();
                Destroy(activeUpkeepSpell.gameObject);
            }

            activeUpkeepSpells.Clear();
        }

        public void FinishCast(HandSide handSide)
        {
            if ((equippedSpellByHandSide[handSide] == null) || !isCastingByHandSide[handSide] ||
               (!equippedGameSpellByHandSide[handSide].IsType(typeof(PhysicalProjectileSpell)) && spellTargetTypeSwitcher.spellTargeterByHandSide[handSide] is SpellSingleTargeter singleTargeter && singleTargeter.targetedCreatureReference is null))
                return;            

            spellTargetTypeSwitcher.DestroyCurrentTargeter(handSide);
            if (SpellSourceTransform(handSide).TryGetComponent(out Collider targetingCollider))
                Destroy(targetingCollider);            

            PhysicalSpell currentSpell = equippedGameSpellByHandSide[handSide];

            castingCreature.OnSpellCasted.Invoke(equippedSpellByHandSide[handSide]);

            bool onLastPhase = currentSpell is PhysicalMultiPhaseSpell spell && spell.currentPhase < spell.lastPhase;

            currentSpell.OnEndCast.Invoke();

            if (onLastPhase)
                return;
            
            if (equippedSpellByHandSide[handSide].channelDuration >= 0.0f && !equippedSpellByHandSide[handSide].isChanneledFully)
            {
                CancelActiveSpell(handSide);
                return;
            }

            spellCooldownTracker.ResetCooldown(equippedSpellReferenceByHandSide[handSide]);

            isCastingByHandSide[handSide] = false;
            equippedSpellByHandSide[handSide] = null;
            equippedGameSpellByHandSide[handSide] = null;
        }

        public void CancelSpells()
        {
            if (CancelActiveSpells())
                return;

            CancelUpkeepSpells();
        }
    }
}
