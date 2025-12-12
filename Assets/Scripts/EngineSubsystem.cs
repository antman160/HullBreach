using UnityEngine;

public class EngineSubsystem : MonoBehaviour
{
    private Subsystem subsystem;

    public bool IsOnline
    {
        get { return subsystem != null && !subsystem.isDestroyed; }
    }

    private void Awake()
    {
        subsystem = GetComponent<Subsystem>();

        if (subsystem == null)
            Debug.LogWarning("EngineSubsystem requires a Subsystem component.");
    }
}
