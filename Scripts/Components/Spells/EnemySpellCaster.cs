using com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Effects;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells
{
    public class EnemySpellCaster : SpellCaster
    {
        AnimationEventInvoker animationEventInvoker;

        protected override void Start()
        {
            base.Start();

            animationEventInvoker = GetComponentInChildren<AnimationEventInvoker>();
            animationEventInvoker.OnAnimationCancelled.AddListener(() => CancelActiveSpells());
        }
    }
}
