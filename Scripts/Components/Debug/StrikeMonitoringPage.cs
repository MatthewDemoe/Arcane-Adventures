using com.AlteredRealityLabs.ArcaneAdventures.Components.CreatureReferences;
using com.AlteredRealityLabs.ArcaneAdventures.Components.ItemEquipper;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Controls;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using com.AlteredRealityLabs.ArcaneAdventures.Combat;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Items;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Debug
{
    public class StrikeMonitoringPage : DebugMenuPage
    {
        [SerializeField] private TextMeshProUGUI leftValues;
        [SerializeField] private TextMeshProUGUI rightValues;


        private readonly Dictionary<HandSide, GameObject> cachedEquippedItemByHandSide = new Dictionary<HandSide, GameObject>
        {
            { HandSide.Left, null},
            { HandSide.Right, null}
        };
        private readonly Dictionary<HandSide, StrikeCalculator> cachedStrikeCalculatorByHandSide = new Dictionary<HandSide, StrikeCalculator>
        {
            { HandSide.Left, null},
            { HandSide.Right, null}
        };

        private PlayerItemEquipper playerItemEquipper => PlayerCharacterReference.Instance.playerItemEquipper;

        private void Update()
        {
            UpdateValues(HandSide.Left);
            UpdateValues(HandSide.Right);
        }

        private void UpdateValues(HandSide handSide)
        {
            var newValuesText = GetValues(handSide);

            if (newValuesText != null)
            {
                var values = handSide.Equals(HandSide.Left) ? leftValues : rightValues;
                values.text = newValuesText;
            }
        }

        private string GetValues(HandSide handSide)
        {
            if (!playerItemEquipper.IsEquipped(handSide))
            {
                return null;
            }
            else if (!ReferenceEquals(playerItemEquipper.GetItemInHand(handSide), cachedEquippedItemByHandSide[handSide]))
            {
                cachedEquippedItemByHandSide[handSide] = playerItemEquipper.GetItemInHand(handSide);
                cachedStrikeCalculatorByHandSide[handSide] = ((PhysicalWeapon) ControllerLink.Get(handSide).connectedItem).GetStrikeCalculator(handSide);
            }

            var strikeCalculator = cachedStrikeCalculatorByHandSide[handSide];
            var strike = strikeCalculator.currentStrike;

            if (strike == null)
            {
                return null;
            }

            return
                "\n" +
                $"{strike.strikeGuid.ToString().Substring(0, 6)}\n" +
                $"{strike.isFinished}\n" +
                $"{strike.hit}\n" +
                $"{strike.distance}\n" +
                $"{strike.time}\n" +
                $"{strike.averageVelocity}\n" +
                $"{strike.velocityPoints} / {strike.momentumPoints} / {strike.strikePoints}\n" +
                $"{strike.strikeType}\n" +
                $"{strike.velocityPointsOnHit} / {strike.momentumPointsOnHit} / {strike.strikePointsOnHit}\n" +
                $"{strike.strikeTypeOnHit}";
        }

        protected override bool TryInitialize() { return true; }
    }
}