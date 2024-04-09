using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Items.WeaponLodging
{
    public class Glass : InGameMaterial
    {
        public override Identifiers.InGameMaterial identifier => Identifiers.InGameMaterial.Glass;
        public override bool canLodgeWeapon => false;
        public override bool canShatter => true;

        protected Dictionary<Identifiers.InGameMaterial, ContactEffect> contactEffectsByMaterial = new Dictionary<Identifiers.InGameMaterial, ContactEffect>
        {
            { Identifiers.InGameMaterial.Flesh, ContactEffect.ShatteredGlass },
            { Identifiers.InGameMaterial.Wood, ContactEffect.ShatteredGlass },
            { Identifiers.InGameMaterial.Metal, ContactEffect.ShatteredGlass },
            { Identifiers.InGameMaterial.Stone, ContactEffect.ShatteredGlass },
            { Identifiers.InGameMaterial.Glass, ContactEffect.ShatteredGlass },
            { Identifiers.InGameMaterial.Dirt, ContactEffect.ShatteredGlass },
            { Identifiers.InGameMaterial.Sand, ContactEffect.Nothing },
            { Identifiers.InGameMaterial.Water, ContactEffect.Nothing }
        };

        public override ContactEffect GetContactEffect(Identifiers.InGameMaterial contactMaterial) => contactEffectsByMaterial[contactMaterial];
    }
}