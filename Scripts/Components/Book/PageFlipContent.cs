using System.Collections;
using TMPro;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Book
{
    public class PageFlipContent : MonoBehaviour
    {
        [SerializeField]
        GameObject pageRenderer;

        [SerializeField]
        GameObject pageFront;
        GameObject pageFrontInstance;

        [SerializeField]
        GameObject pageBack;
        GameObject pageBackInstance;

        [SerializeField]
        TMP_FontAsset unlitFont;


        public void SetFrontPageContent(GameObject pageContent)
        {
            StartCoroutine(DisableAfterRender());

            pageFrontInstance = Instantiate(pageContent, pageFront.transform.position, pageFront.transform.rotation, pageFront.transform);
            pageFrontInstance.SetActive(true);

            foreach (TextMeshProUGUI text in pageFrontInstance.GetComponentsInChildren<TextMeshProUGUI>())
            {
                text.font = unlitFont;
            }
        }

        public void SetBackPageContent(GameObject pageContent)
        {
            StartCoroutine(DisableAfterRender());

            pageBackInstance = Instantiate(pageContent, pageBack.transform.position, pageBack.transform.rotation, pageBack.transform);
            pageBackInstance.SetActive(true);

            foreach (TextMeshProUGUI text in pageBackInstance.GetComponentsInChildren<TextMeshProUGUI>())
            {
                text.font = unlitFont;
            }
        }

        IEnumerator DisableAfterRender()
        {
            pageRenderer.SetActive(true);

            yield return null;

            pageRenderer.SetActive(false);
        }
    }
}