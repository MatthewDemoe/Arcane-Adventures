using com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Effects;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Targeting
{
    public class KnockBackIndicatorCreator : MonoBehaviour
    {
        CapsuleCollider attachedCollider;
        TriggerEvents triggerEvents;

        Dictionary<CreatureReference, GameObject> indicatorsByCreatureReference = new Dictionary<CreatureReference, GameObject>();

        private float knockBackDistance = 1.0f;

        void Awake()
        {
            attachedCollider = GetComponent<CapsuleCollider>();

            triggerEvents = gameObject.AddComponent<TriggerEvents>();
            triggerEvents.creatureEvents.OnEnter.AddListener(CreateKnockBackIndicator);
            triggerEvents.creatureEvents.OnExit.AddListener(DestroyKnockBackIndicator);
        }

        public void Initialize(Spell spell)
        {
            knockBackDistance = spell.knockbackDistance;
        }

        private void CreateKnockBackIndicator(CreatureReference creatureReference)
        {
            if (indicatorsByCreatureReference.ContainsKey(creatureReference))
                return;

            GameObject arrowDecal = Instantiate(Prefabs.UI.Load(Prefabs.UI.ArrowDecal));
            KnockBackIndicator knockBackIndicator = arrowDecal.AddComponent<KnockBackIndicator>();

            knockBackIndicator.Initialize(creatureReference.gameObject, attachedCollider, knockBackDistance);

            indicatorsByCreatureReference.Add(creatureReference, arrowDecal);
        }

        private void DestroyKnockBackIndicator(CreatureReference creatureReference)
        {
            if (!indicatorsByCreatureReference.ContainsKey(creatureReference))
                return;

            GameObject knockBackIndicator = indicatorsByCreatureReference[creatureReference];

            indicatorsByCreatureReference.Remove(creatureReference);

            Destroy(knockBackIndicator);
        }

        private void OnDestroy()
        {
            indicatorsByCreatureReference.Keys.ToList().ForEach((creatureReference) =>
            {
                DestroyKnockBackIndicator(creatureReference);
            });
        }
    }
}