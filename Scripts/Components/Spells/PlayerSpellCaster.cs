using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Debug;
using com.AlteredRealityLabs.ArcaneAdventures.Combat;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Conditions;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using UnityEngine;
using UnityEngine.Events;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells
{    
    public class PlayerSpellCaster : SpellCaster
    {
        private int numberOfControllersHoveringUI = 0;

        protected GameObject statusSpellPrefab;

        public UnityEvent<bool> OnHoverUI = new UnityEvent<bool>();
        public bool isHoveringUI => numberOfControllersHoveringUI > 0;

        public override float GetAimAngleAdjust => Mathf.Deg2Rad * CombatSettings.Spells.AimAngleAdjust;
        
        protected override void Start()
        {
            base.Start();

            statusSpellPrefab = Prefabs.Spells.Load(Prefabs.Spells.StatusConditionSpell);

            OnHoverUI.AddListener((isHovering) =>
            {
                numberOfControllersHoveringUI += isHovering ? 1 : -1;
            });
        }

        public override bool InitializeCharacterSpells()
        {
            if (!base.InitializeCharacterSpells())
                return false;

            if (castingCharacter is PlayerGhost)
            {
                enabled = false;
                return false;
            }            

            //TODO: Move these to their respective classes. 
            //This script is destroyed/recreated when the player avatar is rebuilt, so it's a bit wonky to subscribe to an event on start. 
            SpellMenu.Instance.InitializeSpellButtons(castingCharacter.preparedSpells);
            SpellAdjustmentPage.Instance.Initialize();

            return true;
        }

        public void CreateStatusSpell(AllStatusConditions.StatusConditionName statusConditionName, HandSide handSide)
        {
            equippedSpellReferenceByHandSide[handSide] = StatusConditionSpell.Instance;
            equippedSpellByHandSide[handSide] = equippedSpellReferenceByHandSide[handSide].CreateSpell(ref castingCreature);
            (equippedSpellByHandSide[handSide] as StatusConditionSpell).statusCondition = statusConditionName;

            if (castingCharacter.OnEquipSpell != null)
                castingCharacter.OnEquipSpell.Invoke(equippedSpellByHandSide[handSide]);

            spellObjectByHandSide[handSide] = Instantiate(statusSpellPrefab, SpellSourceTransform(handSide));

            equippedGameSpellByHandSide[handSide] = spellObjectByHandSide[handSide].GetComponent<PhysicalSpell>();
            equippedGameSpellByHandSide[handSide].InitializeSpellInformation(equippedSpellByHandSide[handSide], this, handSide);

            OnSpellCreated.Invoke(equippedGameSpellByHandSide[handSide], handSide);
        }

        public override void PrepareSpell(Spell spell)
        {
            base.PrepareSpell(spell);

            if (castingCharacter.preparedSpells.Contains(spell))
                return;

            castingCharacter.preparedSpells.Add(spell);
            spellCooldownTracker.InitializeSpellList(castingCharacter.preparedSpells);

            SpellMenu.Instance.InitializeSpellButtons(castingCharacter.preparedSpells);
        }

        public override void UnPrepareSpell(Spell spell)
        {
            base.UnPrepareSpell(spell);

            if (!castingCharacter.preparedSpells.Contains(spell))
                return;

            castingCharacter.preparedSpells.Remove(spell);
            spellCooldownTracker.InitializeSpellList(castingCharacter.preparedSpells);

            SpellMenu.Instance.InitializeSpellButtons(castingCharacter.preparedSpells);
        }

        public void HandleButtonPressed(HandSide handSide)
        {
            if (isHoveringUI)
                return;

            if (equippedGameSpellByHandSide[handSide] == null || !equippedGameSpellByHandSide[handSide].hasBeenCast)
            {
                StartCast(handSide);
                return;
            }

            if (!equippedGameSpellByHandSide[handSide].isChanneled)
                FinishCast(handSide);
        }

        public void HandleButtonReleased(HandSide handSide)
        {
            if (equippedGameSpellByHandSide[handSide] == null)
                return;

            if ((equippedGameSpellByHandSide[handSide].isChanneled && equippedGameSpellByHandSide[handSide].hasBeenCast)
                || equippedGameSpellByHandSide[handSide].name.Contains(Prefabs.Spells.Basic))
                FinishCast(handSide);
        }

        public void ResetHoveringUI()
        {
            numberOfControllersHoveringUI = 0;
        }
    }
}
