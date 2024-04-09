namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Items
{
    public abstract class Item
    {
        public readonly string name;

        public Item(string name)
        {
            this.name = name;
        }
    }
}