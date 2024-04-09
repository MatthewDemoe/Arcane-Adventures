using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Items.WeaponLodging
{
    public class Water : InGameMaterial
    {
        public override Identifiers.InGameMaterial identifier => Identifiers.InGameMaterial.Water;
        public override bool canLodgeWeapon => false;

        protected Dictionary<Identifiers.InGameMaterial, ContactEffect> contactEffectsByMaterial = new Dictionary<Identifiers.InGameMaterial, ContactEffect>
        {
            { Identifiers.InGameMaterial.Flesh, ContactEffect.WaterSplash },
            { Identifiers.InGameMaterial.Wood, ContactEffect.WaterSplash },
            { Identifiers.InGameMaterial.Metal, ContactEffect.WaterSplash },
            { Identifiers.InGameMaterial.Stone, ContactEffect.WaterSplash },
            { Identifiers.InGameMaterial.Glass, ContactEffect.WaterSplash },
            { Identifiers.InGameMaterial.Dirt, ContactEffect.WaterSplash },
            { Identifiers.InGameMaterial.Sand, ContactEffect.WaterSplash },
            { Identifiers.InGameMaterial.Water, ContactEffect.WaterSplash }
        };

        public override ContactEffect GetContactEffect(Identifiers.InGameMaterial contactMaterial) => contactEffectsByMaterial[contactMaterial];
    }
}