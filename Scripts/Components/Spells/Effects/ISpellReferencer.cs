using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells
{
    public interface ISpellReferencer
    {
        public PhysicalSpell physicalSpell { get; set; }
    }
}