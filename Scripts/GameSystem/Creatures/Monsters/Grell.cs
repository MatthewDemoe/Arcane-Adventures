using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Items;
using com.AlteredRealityLabs.ArcaneAdventures.Identifiers;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Monsters
{
    public class Grell : HumanoidMonster
    {
        public Grell(Stats stats, Race race, Item leftHandItem, Item rightHandItem) :
            base(stats, race, leftHandItem, rightHandItem)
        { }
    }
}