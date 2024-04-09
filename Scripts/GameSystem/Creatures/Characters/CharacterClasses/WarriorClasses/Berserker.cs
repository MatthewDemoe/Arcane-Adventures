using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.WarriorSpells;
using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Characters.CharacterClasses.WarriorClasses
{
    public class Berserker : CharacterClassArchetype
    {
        public override string name => "Berserker";
        public override string classDescription => "Channeling your own rage you cast powerful spells that enhance your Body past its natural limits and devastate your foes with incredible feats of strength!";
        public override Identifiers.CharacterClassArchetype identifier => Identifiers.CharacterClassArchetype.Berserker;
        public static Berserker Instance { get; } = new Berserker();
        protected Berserker() { }

        public override List<Trait> traits => new List<Trait>() 
        { 
            //TODO: Enable when completed
            //DeathThroes.Instance,
            FrothingRage.Instance,
            PleasureInDeath.Instance,
            //RagingSpirit.Instance,
            TunnelVision.Instance,
        };

        public override List<Spell> spells => new List<Spell>()
        {
            BullRush.Instance,
            RecklessAbandon.Instance,
            GiantsCleave.Instance,
            Enrage.Instance, 
            UnrelentingForce.Instance,
            Brutalize.Instance,
            TitansGrip.Instance,
            BloodReaver.Instance,
            MeteorCrash.Instance,
        };
    }
}
