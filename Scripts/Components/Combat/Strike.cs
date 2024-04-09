using com.AlteredRealityLabs.ArcaneAdventures.Combat;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;
using System;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Combat
{
    public class Strike
    {
        public int velocityPoints
        {
            get
            {
                if (averageVelocity < CombatSettings.Strikes.VelocityTwoPointThreshold)
                    return 0;
                else if (averageVelocity < CombatSettings.Strikes.VelocityThreePointThreshold)
                    return 2;
                else
                    return 3;
            }
        }

        public int momentumPoints
        {
            get
            {
                if (distance < CombatSettings.Strikes.DistanceTwoPointThreshold)
                    return 0;
                else if (distance < CombatSettings.Strikes.DistanceThreePointThreshold)
                    return 2;
                else
                    return 3;
            }
        }

        public int strikePoints => velocityPoints + momentumPoints;
        public StrikeType strikeType => GetStrikeType(strikePoints);

        public int velocityPointsOnHit { get; private set; }
        public int momentumPointsOnHit { get; private set; }
        public int strikePointsOnHit => velocityPointsOnHit + momentumPointsOnHit;
        public StrikeType strikeTypeOnHit => GetStrikeType(strikePointsOnHit);

        private readonly Transform controller;
        private readonly Vector3 startPoint;
        private Vector3 endPoint;
        private readonly float startTime;
        private float stopTime;

        public bool isFinished { get; private set; }
        public bool hit { get; private set; }
        public Guid strikeGuid { get; private set; }
        public float distance => Vector3.Distance(Vector3.Scale(startPoint, CombatSettings.Strikes.AxisScale), Vector3.Scale(endPoint, CombatSettings.Strikes.AxisScale));
        public float time => stopTime - startTime;
        public float averageVelocity => distance / time;
        public Vector3 direction => (endPoint - startPoint).normalized;

        public Strike(Transform controller)
        {
            strikeGuid = Guid.NewGuid();
            this.controller = controller;
            startPoint = this.controller.position;
            endPoint = startPoint;
            startTime = Time.time;
            stopTime = startTime;
        }

        public void Update(bool isThrustingNow, bool isFlickingNow)
        {
            if (isFinished)
            {
                return;
            }
            else if (!isThrustingNow && !isFlickingNow)
            {
                isFinished = true;
                return;
            }

            endPoint = controller.position;
            stopTime = Time.time;
        }

        public void ReportHit()
        {
            hit = true;
            velocityPointsOnHit = velocityPoints;
            momentumPointsOnHit = momentumPoints;
        }

        public void SetAsThrusting()
        {
            if (isFinished)
            {
                return;
            }

            //TODO: Resurrect isThrusting, set to true here and use in calculations?
        }

        private StrikeType GetStrikeType(int strikePoints)
        {
            if (strikePoints < 3)
                return StrikeType.NotStrike;
            else if (strikePoints < 4)
                return StrikeType.Incomplete;
            else if (strikePoints < 5)
                return StrikeType.Imperfect;
            else
                return StrikeType.Perfect;
        }
    }
}