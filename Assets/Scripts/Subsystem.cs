using UnityEngine;

public class Subsystem : MonoBehaviour
{
    public string systemName = "Subsystem";
    public float maxHP = 50f;
    public float currentHP;
    public bool isDestroyed = false;

    public delegate void SubsystemEvent(Subsystem s);
    public event SubsystemEvent OnSubsystemDestroyed;

    private void Awake()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(float amount)
    {
        if (isDestroyed)
            return;

        currentHP -= amount;
        if (currentHP <= 0f)
        {
            currentHP = 0f;
            isDestroyed = true;

            Debug.Log(systemName + " destroyed.");

            if (OnSubsystemDestroyed != null)
                OnSubsystemDestroyed(this);
        }
    }
}
