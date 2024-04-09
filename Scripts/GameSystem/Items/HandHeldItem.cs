using com.AlteredRealityLabs.ArcaneAdventures.ScriptableObjects;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Items
{
    public class HandHeldItem : Item
    {
        public readonly bool canWieldWithTwoHands;

        public HandHeldItem(ItemAsset itemAsset) : base(itemAsset.name)
        {
            this.canWieldWithTwoHands = itemAsset.canWieldWithTwoHands;
        }
    }
}