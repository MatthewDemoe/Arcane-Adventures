using com.AlteredRealityLabs.ArcaneAdventures.Creatures.UMA;
using System.Collections;
using System.Collections.Generic;
using UMA.CharacterSystem;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Development
{
    public class AvatarHeightSetter
    {
        public static float GroupOneSmallModifier = 1f;
        public static float GroupOneBigModifier = 0f;

        public static float GroupTwoSmallModifier = 0.88f;
        public static float GroupTwoBigModifier = 0.37f;

        public static float GroupThreeSmallModifier = 0.88f;
        public static float GroupThreeBigModifier = 0.37f;

        private readonly static List<DnaName> dnaHeightScalingGroupOne = new List<DnaName>
        {
            DnaName.neckThickness,
            DnaName.armWidth,
            DnaName.forearmWidth,
            DnaName.upperMuscle,
            DnaName.lowerMuscle,
            DnaName.upperWeight,
            DnaName.lowerWeight
        };

        private readonly static List<DnaName> dnaHeightScalingGroupTwo = new List<DnaName>
        {
            DnaName.handsSize,
            DnaName.feetSize
        };

        private readonly static List<DnaName> dnaHeightScalingGroupThree = new List<DnaName>
        {
            DnaName.headSize
        };

        private DynamicCharacterAvatar avatar;
        private SkinnedMeshRenderer avatarRenderer;
        private GameSystem.Creatures.Races.Race race;
        private float umaMinimumHeightInMeters;
        private float umaMaximumHeightInMeters;

        public float enforcedMinimumHeightInDecimal { get; private set; }
        public float enforcedMaximumHeightInDecimal { get; private set; }
        public float defaultHeightInDecimal { get; private set; }

        public AvatarHeightSetter(DynamicCharacterAvatar avatar, Identifiers.Race raceIdentifier)
        {
            this.avatar = avatar;
            race = GameSystem.Creatures.Races.Race.Get(raceIdentifier);
        }

        public IEnumerator Initialize()
        {
            yield return new WaitUntil(() => TryGetAvatarRenderer());

            var originalHeightInMeters = avatarRenderer.bounds.size.y;
            SetHeight(0);
            yield return new WaitUntil(() => avatarRenderer.bounds.size.y != originalHeightInMeters);
            umaMinimumHeightInMeters = avatarRenderer.bounds.size.y;

            SetHeight(1);
            yield return new WaitUntil(() => avatarRenderer.bounds.size.y != umaMinimumHeightInMeters);
            umaMaximumHeightInMeters = avatarRenderer.bounds.size.y;

            enforcedMinimumHeightInDecimal = GetHeightInDecimal(race.maleMinimumHeight);//TODO: Support female.
            enforcedMaximumHeightInDecimal = GetHeightInDecimal(race.maleMaximumHeight);//TODO: Support female.
            defaultHeightInDecimal = (enforcedMinimumHeightInDecimal + enforcedMaximumHeightInDecimal) * 0.5f;
            SetHeight(defaultHeightInDecimal);
            yield return new WaitUntil(() => avatarRenderer.bounds.size.y != umaMaximumHeightInMeters);

            avatar.gameObject.SetActive(false);
            avatar.transform.position = new Vector3(avatar.transform.position.x, 0, avatar.transform.position.z);
        }

        private bool TryGetAvatarRenderer()
        {
            avatarRenderer = avatar.GetComponentInChildren<SkinnedMeshRenderer>();

            return avatarRenderer != null;
        }

        public float GetHeightInDecimal(float heightInMeters) => Mathf.InverseLerp(umaMinimumHeightInMeters, umaMaximumHeightInMeters, heightInMeters);
        public float GetHeightInMeters(float heightInDecimal) => Mathf.Lerp(umaMinimumHeightInMeters, umaMaximumHeightInMeters, heightInDecimal);

        public void SetHeight(float heightInDecimal)
        {
            var selectedAvatarDna = avatar.GetDNA();

            selectedAvatarDna[DnaName.height.ToString()].Set(heightInDecimal);

            var groupOneValue = GetScalingGroupValue(heightInDecimal, GroupOneBigModifier, GroupOneSmallModifier);

            foreach (var dnaName in dnaHeightScalingGroupOne)
            {
                selectedAvatarDna[dnaName.ToString()].Set(groupOneValue);
            }

            var groupTwoValue = GetScalingGroupValue(heightInDecimal, GroupTwoBigModifier, GroupTwoSmallModifier);

            foreach (var dnaName in dnaHeightScalingGroupTwo)
            {
                selectedAvatarDna[dnaName.ToString()].Set(groupTwoValue);
            }

            var groupThreeValue = GetScalingGroupValue(heightInDecimal, GroupThreeBigModifier, GroupThreeSmallModifier);

            foreach (var dnaName in dnaHeightScalingGroupThree)
            {
                selectedAvatarDna[dnaName.ToString()].Set(groupThreeValue);
            }

            avatar.ForceUpdate(true);
        }

        private float GetScalingGroupValue(float heightInDecimal, float bigModifier, float smallModifier) => (heightInDecimal > 0.5f) ?
            Mathf.Lerp(0.5f, bigModifier, (heightInDecimal - 0.5f) * 2) :
            Mathf.Lerp(smallModifier, 0.5f, heightInDecimal * 2);
    }
}