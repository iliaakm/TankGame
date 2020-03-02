using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventController : MonoBehaviour
{
    [SerializeField]
    UnityEvent[] AwakeEvents;

    [ SerializeField]
    UnityEvent[] OnEnableEvents;

    [SerializeField]
    UnityEvent[] OnStartEvents;   

    [SerializeField]
    UnityEvent[] OnDisableEvents;

    // Use this for initialization

    private void Awake()
    {
        foreach (UnityEvent ev in AwakeEvents)
            ev.Invoke();
    }

    private void OnEnable()
    {
        foreach (UnityEvent ev in OnEnableEvents)
            ev.Invoke();
    }

    void Start()
    {
        foreach (UnityEvent ev in OnStartEvents)
            ev.Invoke();
    }

    private void OnDisable()
    {
        foreach (UnityEvent ev in OnDisableEvents)
            ev.Invoke();
    }
}
