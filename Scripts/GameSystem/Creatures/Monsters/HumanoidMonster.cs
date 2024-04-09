using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Items;
using com.AlteredRealityLabs.ArcaneAdventures.Identifiers;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Monsters
{
    public abstract class HumanoidMonster : Monster, IItemWielder
    {
        public Item leftHandItem { get; set; }
        public Item rightHandItem { get; set; }

        public HumanoidMonster(Stats stats, Race race, Item leftHandItem, Item rightHandItem) : base(stats, race)
        {
            this.leftHandItem = leftHandItem;
            this.rightHandItem = rightHandItem;
        }

        public override int GetWeaponDamage()
        {
            int damage = 0;

            if (leftHandItem != null && leftHandItem is Weapon)
                damage += (leftHandItem as Weapon).GetDamage();

            if (rightHandItem != null && rightHandItem is Weapon)
                damage += (rightHandItem as Weapon).GetDamage();

            if ((leftHandItem is Weapon) && (rightHandItem is Weapon))
                damage /= 2;

            return damage;
        }
    }
}