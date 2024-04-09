using com.AlteredRealityLabs.ArcaneAdventures.SaveFiles;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Startup
{
    #if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(SaveFileSelection))]
    public class SaveFileSelectionDrawer : PropertyDrawer
    {
        private const string OptionPrefix = "Slot ";
        private const string NoOptionsAvailable = "(no save files in any slot)";

        private bool isStartScene => SceneManager.GetActiveScene().buildIndex == 0;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => isStartScene ? 0 :base.GetPropertyHeight(property, label);

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (isStartScene)
                return;

            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            var options = GetOptions();

            if (!options.Any())
            {
                GUI.backgroundColor = Color.red;
                options = new string[] { NoOptionsAvailable };
            }

            var userIndexProperty = property.FindPropertyRelative(nameof(SaveFileSelection.optionIndex));

            EditorGUI.BeginChangeCheck();

            var optionIndex = EditorGUI.Popup(position, userIndexProperty.intValue, options);
            var option = options[optionIndex];

            if (EditorGUI.EndChangeCheck() && option.StartsWith(OptionPrefix))
            {
                userIndexProperty.intValue = optionIndex;

                var slotNumberProperty = property.FindPropertyRelative(nameof(SaveFileSelection.slotNumber));
                slotNumberProperty.intValue = int.Parse(option.Replace(OptionPrefix, string.Empty));
            }

            EditorGUI.EndProperty();
        }

        private string[] GetOptions()
        {
            var options = new List<string>();

            for (var i = 0; i < SaveFile.NumberOfSaveFiles; i++)
            {
                var slotNumber = i + 1;

                if (SaveFile.HasSaveFileInSlot(slotNumber))
                {
                    options.Add($"{OptionPrefix}{slotNumber}");
                }
            }

            return options.ToArray();
        }
    }
    #endif
}