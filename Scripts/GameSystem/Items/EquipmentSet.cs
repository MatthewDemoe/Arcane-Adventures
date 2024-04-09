namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Items
{
    public class EquipmentSet
    {
        public readonly string name;
        public readonly string contentDescription;
        public readonly Weapon leftHandItem;
        public readonly Weapon rightHandItem;

        public EquipmentSet(string name, string contentDescription, Weapon leftHandItem, Weapon rightHandItem)
        {
            this.name = name;
            this.contentDescription = contentDescription;
            this.leftHandItem = leftHandItem;
            this.rightHandItem = rightHandItem;
        }
    }
}