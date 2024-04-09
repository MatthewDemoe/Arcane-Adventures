using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Items;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Monsters
{
    public class OrcRaider : HumanoidMonster
    {
        public OrcRaider(Stats stats, Identifiers.Race race, Item leftHandItem, Item rightHandItem) : 
            base(stats, race, leftHandItem, rightHandItem) 
        {
            traits = new System.Collections.Generic.List<Trait>()
            {
                Trait.GetTrait(Brave.Instance, this)
            };
        }
    }
}