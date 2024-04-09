using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.SystemExtensions;
using System.Linq;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat
{
    public class StatusConditionHit : Hit
    {
        public StatusConditionHit(Creature target, int damage) : base(null, target)
        {
            this.healthChange = damage;
        }

        public override void CalculateHealthChange()
        {
            healthChange = (int)(healthChange * target.modifiers.effects.Select((effect) => effect.damageTaken).Product());
        }
    }
}