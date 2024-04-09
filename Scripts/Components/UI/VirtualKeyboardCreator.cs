using UnityEngine;
using UnityEngine.UI;
using TMPro;
using VRKeyboard.Utils;
using com.AlteredRealityLabs.ArcaneAdventures.Components.CreatureReferences;

public class VirtualKeyboardCreator : MonoBehaviour
{
    [SerializeField]
    GameObject keyboardPrefab;

    GameObject keyboardInstance = null;

    [SerializeField]
    float playerZOffset = 1.0f;

    TMP_InputField inputField = null;
    Text keyboardInput = null;

    public void CreateKeyboard(TMP_InputField selectedInputField)
    {
        if (keyboardInstance == null)
        {
            inputField = selectedInputField;
            
            Vector3 offsetVector = new Vector3(0.0f, Camera.main.transform.position.y, playerZOffset);
            keyboardInstance = Instantiate(keyboardPrefab, PlayerCharacterReference.Instance.transform.position + offsetVector, PlayerCharacterReference.Instance.transform.rotation, PlayerCharacterReference.Instance.transform);

            keyboardInstance.GetComponentInChildren<KeyboardManager>().maxInputLength = inputField.characterLimit;

            keyboardInput = keyboardInstance.GetComponentInChildren<KeyboardManager>().inputText;
        }
    }

    public void DestroyKeyboard()
    {
        Destroy(keyboardInstance);
        keyboardInstance = null;
        keyboardInput = null;
    }


    private void Update()
    {
        if ((inputField != null) && (keyboardInput != null))
        {
            inputField.text = keyboardInput.text;
        }
    }
}