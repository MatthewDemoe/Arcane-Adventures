using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Items.WeaponLodging
{
    public class Dirt : InGameMaterial
    {
        public override Identifiers.InGameMaterial identifier => Identifiers.InGameMaterial.Dirt;
        public override float strikeStoppingDepenetrationDistance => 0.1f;
        public override bool canLodgeWeapon => true;

        protected Dictionary<Identifiers.InGameMaterial, ContactEffect> contactEffectsByMaterial = new Dictionary<Identifiers.InGameMaterial, ContactEffect>
        {
            { Identifiers.InGameMaterial.Flesh, ContactEffect.Nothing },
            { Identifiers.InGameMaterial.Wood, ContactEffect.Nothing },
            { Identifiers.InGameMaterial.Metal, ContactEffect.Nothing },
            { Identifiers.InGameMaterial.Stone, ContactEffect.Nothing },
            { Identifiers.InGameMaterial.Glass, ContactEffect.Nothing },
            { Identifiers.InGameMaterial.Water, ContactEffect.Nothing },
            { Identifiers.InGameMaterial.Dirt, ContactEffect.Nothing },
            { Identifiers.InGameMaterial.Sand, ContactEffect.Nothing }
        };

        public override ContactEffect GetContactEffect(Identifiers.InGameMaterial contactMaterial) => contactEffectsByMaterial[contactMaterial];
    }
}