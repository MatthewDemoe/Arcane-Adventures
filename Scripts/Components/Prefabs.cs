using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Monsters;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Characters.CharacterClasses;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells;
using System;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Development;
using com.AlteredRealityLabs.ArcaneAdventures.DependencyInjection;
using com.AlteredRealityLabs.ArcaneAdventures.Identifiers;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components
{
    public static class Prefabs
    {
        public const string directory = nameof(Prefabs);

        public static GameObject Load(Creature creature)
        {
            if (creature is Character)
            {
                var character = creature as Character;

                if (character is PlayerCharacter)
                {
                    return Load(creature.race.Equals(Identifiers.Race.Ghost) ? PrefabNames.PlayerCharacter : PlayerCharacterPrefabName);
                }
            }
            else
            {
                if (creature is PossessedDummy)
                {
                    return Load(PrefabNames.PossessedDummy);
                }
                else if (creature is TutorialFairy)
                {
                    return Load(PrefabNames.TutorialFairy);
                }
                else if (creature is OrcRaider)
                {
                    return Load(PrefabNames.OrcRaider);
                }
                else if (creature is OrcShaman)
                {
                    return Load(PrefabNames.OrcShaman);
                }
                else if (creature is Grell)
                {
                    return Load(PrefabNames.Grell);
                }
                else if (creature is Ogre)
                {
                    return Load(PrefabNames.Ogre);
                }
            }

            throw new Exception("Creature prefab not found");
        }

        public static ResourceRequest LoadAsync(Creature creature)
        {
            if (creature is Character)
            {
                var character = creature as Character;

                if (character is PlayerCharacter)
                {
                    return LoadAsync(creature.race.Equals(Identifiers.Race.Ghost) ? PrefabNames.PlayerCharacter : PlayerCharacterPrefabName);
                }
            }
            else
            {
                if (creature is PossessedDummy)
                {
                    return LoadAsync(PrefabNames.PossessedDummy);
                }
                else if (creature is TutorialFairy)
                {
                    return LoadAsync(PrefabNames.TutorialFairy);
                }
                else if (creature is OrcRaider)
                {
                    return LoadAsync(PrefabNames.OrcRaider);
                }
                else if (creature is Grell)
                {
                    return LoadAsync(PrefabNames.Grell);
                }
            }

            throw new Exception("Creature prefab not found");
        }

        public static GameObject Load(Identifiers.Race race)
        {
            if (race == Identifiers.Race.Human)
            {
                return Avatars.Load(Avatars.Human);
            }
            else if (race == Identifiers.Race.Elf)
            {
                return Avatars.Load(Avatars.Elf);
            }

            throw new Exception("Creature prefab not found");
        }

        public static ResourceRequest LoadAsync(Identifiers.Race race)
        {
            if (race == Identifiers.Race.Human)
            {
                return Avatars.LoadAsync(Avatars.Human);
            }
            else if (race == Identifiers.Race.Elf)
            {
                return Avatars.LoadAsync(Avatars.Elf);
            }

            throw new Exception("Creature prefab not found");
        }
        
        public static GameObject Load(string filename) => Load(directory, filename);

        public static ResourceRequest LoadAsync(string filename) => LoadAsync(directory, filename);


        public static GameObject Load(string directory, string filename)
        {
            var path = $"{directory}/{filename}";

            return Resources.Load<GameObject>(path);
        }

        public static ResourceRequest LoadAsync(string directory, string filename)
        {
            var path = $"{directory}/{filename}";

            return Resources.LoadAsync<GameObject>(path);
        }

        public static GameObject[] LoadAll(string directory)
        {
            return Resources.LoadAll<GameObject>(directory);
        }

        public static string PlayerCharacterPrefabName =>
            InjectorContainer.Injector.TryGetInstance<SetupPhaseInputHandler.SetupPhaseSettings>(out var setupPhaseSettings)
            && setupPhaseSettings.mode.Equals(SetupPhaseInputHandler.SetupPhaseSettings.Mode.PuppetMaster) ?
                PrefabNames.NewPlayerCharacter : PrefabNames.PlayerCharacter;

        public static class PrefabNames
        {
            public const string PlayerCharacter = "Player Character/Player Character";
            public const string NewPlayerCharacter = "Player Character/New Human Player Character";
            public const string SoundObject = "SoundObject";
            public const string OrcRaider = "Monsters/Orc Raider";
            public const string Ogre = "Monsters/Ogre";
            public const string OrcShaman = "Monsters/Orc Shaman";
            public const string Grell = "Monsters/Grell";
            public const string PossessedDummy = "Monsters/Possessed Dummy";
            public const string TutorialFairy = "Monsters/Tutorial Fairy";
            public const string ItemUI = "Items/System/Item UI/Item UI";
            public const string ContactEffectPlayer = "In-Game Materials/Contact Effect Player";
            public const string GhostHand = "Hands/Ghost/Ghost";
            public const string GameSystems = "Game Systems";
            public const string Journal = "Items/Journal";
        }

        public static class Materials
        {
            public const string Dissolve = "Dissolve";

            private const string directory = Prefabs.directory + "/" + nameof(Materials) + "/";
            public static Material Load(string filename) => Resources.Load<Material>(directory + filename);
        }

        public static class UI
        {
            private const string directory = Prefabs.directory + "/" + nameof(UI);

            public const string combatUIDamageNumber = "Combat UI Damage Number";

            public const string WarriorIcon = "Classes/Warrior_Icon";
            public const string WizardIcon = "Classes/Wizard_Icon";

            public const string WarriorKeyArt = "Classes/WarriorKeyArt";
            public const string WizardKeyArt = "Classes/WizardKeyArt";

            public const string AoEDecal = "Decals/AoEDecal";
            public const string ArrowDecal = "Decals/ArrowDecal";
            public const string KnockBackIndicator = "Decals/KnockBackIndicator";

            public static GameObject Load(string filename) => Prefabs.Load(directory, filename);
        }

        public static class Avatars
        {
            private const string directory = Prefabs.directory + "/" + nameof(Avatars);

            public const string Human = nameof(Human);
            public const string Elf = nameof(Elf);

  
            public static GameObject Load(string filename) => Prefabs.Load(directory, filename);
            public static ResourceRequest LoadAsync(string filename) => Prefabs.LoadAsync(directory, filename);

        }

        public static class Spells
        {
            private const string directory = Prefabs.directory + "/" + nameof(Spells);
            public const string SuccessBurst = nameof(SuccessBurst);
            public const string FailureBurst = nameof(FailureBurst);

            public const string Basic = nameof(Basic);

            public const string StatusConditionSpell = "Status Condition Spell";

            public static GameObject Load(CharacterClassAttributes characterClassAttributes, Spell spellToLoad)
            {
                string fullDirectory = directory + "/" + characterClassAttributes.name + "/Level " + spellToLoad.spellLevel;
               
                return Prefabs.Load(fullDirectory, spellToLoad.name);
            }

            public static GameObject Load(string prefabName) => Prefabs.Load(directory, prefabName);
        }

        public static class Debug
        {
            private const string directory = Prefabs.directory + "/" + nameof(Debug);

            public const string MeasuringRay = "Measuring ray";
            public const string RedDebugIndicator = "Red Debug Indicator";
            public const string BlackDebugIndicator = "Black Debug Indicator";

            public static GameObject Load(string filename) => Prefabs.Load(directory, filename);
        }

        public static class Test
        {
            private const string directory = Prefabs.directory + "/" + nameof(Test);
            public const string Floor = nameof(Floor);
            public static GameObject Load(string filename) => Prefabs.Load(directory, filename);
        }

        public static class Audio
        {
            private const string directory = Prefabs.directory + "/" + nameof(Audio);
            public const string SpellAudioSource = nameof(SpellAudioSource);
            public const string AnimationAudioSource = nameof(AnimationAudioSource);

            public static GameObject Load(string filename) => Prefabs.Load(directory, filename);

        }
    }
}