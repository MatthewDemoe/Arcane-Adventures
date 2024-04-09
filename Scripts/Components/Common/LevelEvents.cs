using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelEvents : MonoBehaviour
{
    [SerializeField]
    List<UnityEvent> levelEvents = new List<UnityEvent>();

    private static int currentEvent = 0;


    // Start is called before the first frame update
    void Start()
    {
        foreach (UnityEvent e in levelEvents)
        {
            e.AddListener(() => currentEvent += 1);
        }
    }

    public void InvokeCurrentEvent()
    {
        levelEvents[currentEvent].Invoke();
    }
}
