using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.WarriorSpells;
using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Characters.CharacterClasses.WarriorClasses
{
    public class WeaponMaster : CharacterClassArchetype
    {
        public override string name => "Weapon Master";
        public override string classDescription => "Through constant and diligent training you have honed your Spirit and Body to become one. With Spirit empowering your incredible technique you are sure to crush your opposition.";
        public override Identifiers.CharacterClassArchetype identifier => Identifiers.CharacterClassArchetype.WeaponMaster;
        public static WeaponMaster Instance { get; } = new WeaponMaster();
        protected WeaponMaster() { }

        public override List<Trait> traits => new List<Trait>() 
        { 
            AdaptiveFighting.Instance,
            AgileWeaponry.Instance,
            DominantSpells.Instance,
            DurableFighter.Instance,
            WellPrepared.Instance,
        };

        public override List<Spell> spells => new List<Spell>() 
        {
            SlicingDash.Instance,
            DisarmingManeuver.Instance,
            HeavyBlow.Instance, 
            PredictedMovement.Instance,
            TakeDown.Instance, 
            SharpenedStance.Instance,
            DeepenedResolve.Instance,
            ReactiveBarrier.Instance,
            FeintingStrike.Instance,
        };
    }
}
