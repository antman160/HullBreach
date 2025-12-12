using UnityEngine;

public class ArmorPlate2D : MonoBehaviour
{
    public float maxArmor = 40f;
    public float currentArmor = 40f;

    public bool IsDepleted
    {
        get { return currentArmor <= 0f; }
    }

    public void TakeArmorDamage(float dmg)
    {
        currentArmor -= dmg;
        if (currentArmor <= 0f)
        {
            currentArmor = 0f;
            BreakPlate();
        }
    }

    private void BreakPlate()
    {
        // remove armor plate when destroyed
        Destroy(gameObject);
    }
}
