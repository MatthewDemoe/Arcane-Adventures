using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Items.WeaponLodging
{
    public class Metal : InGameMaterial
    {
        public override Identifiers.InGameMaterial identifier => Identifiers.InGameMaterial.Metal;
        public override bool canLodgeWeapon => false;

        protected Dictionary<Identifiers.InGameMaterial, ContactEffect> contactEffectsByMaterial = new Dictionary<Identifiers.InGameMaterial, ContactEffect>
        {
            { Identifiers.InGameMaterial.Flesh, ContactEffect.Nothing },
            { Identifiers.InGameMaterial.Wood, ContactEffect.Nothing },
            { Identifiers.InGameMaterial.Metal, ContactEffect.Sparks },
            { Identifiers.InGameMaterial.Stone, ContactEffect.Sparks },
            { Identifiers.InGameMaterial.Glass, ContactEffect.Nothing },
            { Identifiers.InGameMaterial.Dirt, ContactEffect.Nothing },
            { Identifiers.InGameMaterial.Sand, ContactEffect.Nothing },
            { Identifiers.InGameMaterial.Water, ContactEffect.Nothing }
        };

        public override ContactEffect GetContactEffect(Identifiers.InGameMaterial contactMaterial) => contactEffectsByMaterial[contactMaterial];
    }
}