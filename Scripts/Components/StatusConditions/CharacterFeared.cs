using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.ScriptableObjects;
using UnityEngine;
using TheKiwiCoder;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.StatusConditions
{
    public class CharacterFeared : MonoBehaviour
    {
        CreatureReference creatureReference;
        BehaviourTreeRunner treeRunner;
        BehaviourTree previousTree;

        void Start()
        {
            creatureReference = GetComponentInParent<CreatureReference>();
            treeRunner = creatureReference.GetComponent<BehaviourTreeRunner>();
            previousTree = treeRunner.tree;

            treeRunner.SwapTrees(BehaviourTreeCache.GetBehaviourTree(BehaviourTreeCache.Feared));
        }

        private void OnDestroy()
        {
            treeRunner.SwapTrees(previousTree);
        }
    }
}