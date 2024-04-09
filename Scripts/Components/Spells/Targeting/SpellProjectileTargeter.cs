using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Targeting
{
    public class SpellProjectileTargeter : SpellTargeter
    {
        public override Vector3 targetDirection => GetTargetDirection();
    }
}
