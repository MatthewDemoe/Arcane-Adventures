using System.Collections.Generic;
using System.Linq;
using TheKiwiCoder;

namespace com.AlteredRealityLabs.ArcaneAdventures.ScriptableObjects
{
    public static class BehaviourTreeCache
    {
        public const string Feared = nameof(Feared);
        public const string Restrained = nameof(Restrained);

        private static Dictionary<string, BehaviourTree> CachedBehaviourTreesByName;
        private static Dictionary<string, BehaviourTree> BehaviourTreesByName
        {
            get
            {
                if (CachedBehaviourTreesByName == null)
                {
                    CachePrefabsByBehaviourTree();
                }

                return CachedBehaviourTreesByName;
            }
        }

        public static BehaviourTree GetBehaviourTree(string behaviourTreeName)
        {
            return BehaviourTreesByName.Keys.Contains(behaviourTreeName) ? BehaviourTreesByName[behaviourTreeName] : null;
        }

        private static void CachePrefabsByBehaviourTree()
        {
            var behaviourTreeAssets = BehaviourTree.LoadAll();
            var prefabsByBehaviourTree = new Dictionary<string, BehaviourTree>();

            foreach (var behaviourTreeAsset in behaviourTreeAssets)
            {
                prefabsByBehaviourTree.Add(behaviourTreeAsset.name, behaviourTreeAsset.Clone());
            }

            CachedBehaviourTreesByName = prefabsByBehaviourTree;
        }
    }
}