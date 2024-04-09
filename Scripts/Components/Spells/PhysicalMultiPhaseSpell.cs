using System.Collections.Generic;
using UnityEngine;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using System;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells
{
    public class PhysicalMultiPhaseSpell : PhysicalSpell
    {
        public int currentPhase { get; protected set; } = 0;
        public int lastPhase => spellPhases.Count - 1;

        [SerializeField]
        List<PhysicalSpell> spellPhases = new List<PhysicalSpell>();

        public override Type playerSpellTargeter => spellPhases[currentPhase].playerSpellTargeter;
        public override bool IsType(Type type) => spellPhases[currentPhase].IsType(type);

        protected override void Awake()
        {
            AddCurrentPhaseListeners();
            base.Awake();
        }

        public override void InitializeSpellInformation(Spell spell, SpellCaster playerSpellCaster, HandSide handSide)
        {
            base.InitializeSpellInformation(spell, playerSpellCaster, handSide);

            spellPhases.ForEach(physicalSpell => physicalSpell.InitializeSpellInformation(correspondingSpell, spellCaster, handSide));
        }

        public void StartNextPhase()
        {       
            if (currentPhase >= lastPhase)
                return;

            currentPhase++;
            hasBeenCast = false;
            hasBeenChanneled = false;

            ClearListeners();
            AddCurrentPhaseListeners();

            spellCaster.OnSpellCreated.Invoke(this, handSide);
        }

        private void ClearListeners()
        {
            OnBeginCast.RemoveAllListeners();
            OnChannel.RemoveAllListeners();
            OnUpkeep.RemoveAllListeners();
            OnEndCast.RemoveAllListeners();
            OnDurationEnd.RemoveAllListeners();
            OnCollision.RemoveAllListeners();
        }

        private void AddCurrentPhaseListeners()
        {
            OnBeginCast.AddListener(spellPhases[currentPhase].OnBeginCast.Invoke);
            OnChannel.AddListener(spellPhases[currentPhase].OnChannel.Invoke);
            OnUpkeep.AddListener(spellPhases[currentPhase].OnUpkeep.Invoke);
            OnEndCast.AddListener(spellPhases[currentPhase].OnEndCast.Invoke);
            OnDurationEnd.AddListener(spellPhases[currentPhase].OnDurationEnd.Invoke);
            OnCollision.AddListener(spellPhases[currentPhase].OnCollision.Invoke);

            OnBeginCast.AddListener(() => hasBeenCast = true);
            OnChannel.AddListener(() => hasBeenChanneled = true);
        }
    }
}