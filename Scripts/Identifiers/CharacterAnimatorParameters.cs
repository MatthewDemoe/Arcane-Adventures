using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.Identifiers
{
    static class CharacterAnimatorParameters
    {
        public const string XVelocity = "X Velocity";
        public const string YVelocity = "Y Velocity";
        public const string IsDead = "Is Dead";
        public const string IsUsingSingleAttack = "Is Using Single Attack";
        public const string SwingHeight = "Swing Height";
        public const string IsUsingComboAttack = "Is Using Combo Attack";
        public const string IsRecoilingFromHit = "Is Recoiling From Hit";
        public const string AttackWasParried = "Attack Was Parried";
        public const string AttackSpeed = "Attack Speed";
        public const string Restrained = "Restrained";
        public const string Maddened = "Maddened";
        public const string IsKnockedDown = "Is Knocked Down";
        public const string KnockedDown = "Knocked Down";
        public const string StandingUp = "Standing Up";
        public const string Warcry = "Warcry";
        public const string IsPickingUpItem = "Is Picking Up Item";

    }

    static class PlayerCharacterAnimatorParameters
    {
        public const string Grip = "Grip";
    }
    
    static class GrellAnimatorParameters
    {
        public const string IsUsingJab = "Is Using Jab";
        public const string IsUsingComboAttack = "Is Using Combo Attack";
        public const string IsUsingOverheadSwing = "Is Using Overhead Swing";

        public static List<string> attacks = new List<string>()
        {
            IsUsingJab, 
            IsUsingComboAttack,
            IsUsingOverheadSwing,
            CharacterAnimatorParameters.IsPickingUpItem
        };
    }

    static class OrcRaiderAnimatorParameters
    {
        public const string IsUsingSingleAttack = "Is Using Single Attack";
        public const string IsUsingComboAttack = "Is Using Combo Attack";

        public static List<string> attacks = new List<string>()
        {
            IsUsingSingleAttack, 
            IsUsingComboAttack,
            CharacterAnimatorParameters.IsPickingUpItem
        };
    }

    static class OrcShamanAnimatorParameters
    {
        public const string IsUsingOverheadSwing = "Is Using Overhead Swing";
        public const string IsCastingBasicSpell = "Is Casting Basic Spell";
        public const string IsCastingSpell = "Is Casting Spell";

        public static List<string> attacks = new List<string>()
        {
            IsUsingOverheadSwing,
            IsCastingBasicSpell,
            IsCastingSpell,
            CharacterAnimatorParameters.IsPickingUpItem
        };
    }

    static class OgreAnimatorParameters
    {
        public const string IsUsingOverheadSwing = "Is Using Overhead Swing";
        public const string IsUsingSlash = "Is Using Slash";
        public const string IsUsingSlashCombo = "Is Using Slash Combo";
        public const string IsUsingPunchCombo = "Is Using Punch Combo";
        public const string IsUsingHandSlam = "Is Using Hand Slam";
        public const string IsGrabbingPlayer = "Is Grabbing Player";
        public const string IsUsingStomp = "Is Using Stomp";
        public const string IsUsingCharge = "Is Using Charge";
        public const string IsUsingThrow = "Is Using Throw";
        public const string GrabInterrupted = "Grab Interrupted";

        public static List<string> attacks = new List<string>() 
        {
            IsUsingOverheadSwing, 
            IsUsingSlash, 
            IsUsingSlashCombo, 
            IsUsingPunchCombo,
            IsUsingHandSlam,
            IsUsingStomp,
            IsUsingThrow,
            IsUsingCharge,
            IsGrabbingPlayer
        };
    }
}