
namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells
{
    public enum BasicSpellType { Wand, Staff }

    public class EnumToSpellConverter
    {
        public static Spell EnumToSpell(BasicSpellType spellType)
        {
            if (spellType == BasicSpellType.Wand)
                return BasicSpell.Instance;

            return BasicStaffSpell.Instance;
        }
    }
}