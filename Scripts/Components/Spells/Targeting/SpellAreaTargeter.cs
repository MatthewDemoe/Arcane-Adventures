using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Targeting
{
    public class SpellAreaTargeter : SpellPointTargeter
    {
        float scale = 0.0f;

        public override void InitializeWithSpellInformation(Spell spell, SpellCaster spellCaster, HandSide handSide)
        {
            base.InitializeWithSpellInformation(spell, spellCaster, handSide);

            scale = spell.radius * 2.0f;
            range = spell.range;

            decalInstance = Instantiate(Prefabs.UI.Load(Prefabs.UI.AoEDecal), transform.position, transform.rotation);

            decalInstance.transform.localScale = new Vector3(scale, 1.0f, scale);

            PositionDecal();
        }

        protected override void PositionDecal()
        {
            base.PositionDecal();

            decalInstance.transform.localScale = new Vector3(decalInstance.transform.localScale.x, Mathf.Clamp(Mathf.Abs(downHit.point.y - forwardHit.point.y), 0.5f, 2.0f), decalInstance.transform.localScale.z);
            decalInstance.transform.up = forwardHit.normal;

            decalInstance.transform.position = Vector3.MoveTowards(decalInstance.transform.position, GetMaxRangeGroundPosition(), frameLerpSpeed);
        }
    }
}