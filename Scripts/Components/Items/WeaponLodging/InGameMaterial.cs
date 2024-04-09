namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Items.WeaponLodging
{
    public abstract class InGameMaterial
    {
        public abstract Identifiers.InGameMaterial identifier { get; }
        public virtual float strikeStoppingDepenetrationDistance => 0;
        public abstract bool canLodgeWeapon { get; }
        public virtual bool canShatter { get; } = false;
        public abstract ContactEffect GetContactEffect(Identifiers.InGameMaterial contactMaterial);
    }
}