using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Items.WeaponLodging
{
    public class Wood : InGameMaterial
    {
        public override Identifiers.InGameMaterial identifier => Identifiers.InGameMaterial.Wood;
        public override float strikeStoppingDepenetrationDistance => 0.05f;
        public override bool canLodgeWeapon => true;

        protected Dictionary<Identifiers.InGameMaterial, ContactEffect> contactEffectsByMaterial = new Dictionary<Identifiers.InGameMaterial, ContactEffect>
        {
            { Identifiers.InGameMaterial.Flesh, ContactEffect.WoodChips },
            { Identifiers.InGameMaterial.Wood, ContactEffect.WoodChips },
            { Identifiers.InGameMaterial.Metal, ContactEffect.WoodChips },
            { Identifiers.InGameMaterial.Stone, ContactEffect.WoodChips },
            { Identifiers.InGameMaterial.Glass, ContactEffect.WoodChips },
            { Identifiers.InGameMaterial.Water, ContactEffect.Nothing },
            { Identifiers.InGameMaterial.Dirt, ContactEffect.Nothing },
            { Identifiers.InGameMaterial.Sand, ContactEffect.Nothing }
        };

        public override ContactEffect GetContactEffect(Identifiers.InGameMaterial contactMaterial) => contactEffectsByMaterial[contactMaterial];
    }
}