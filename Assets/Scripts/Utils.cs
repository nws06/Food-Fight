using UnityEngine;

public class Utils
{
    public static bool IsOffCooldown(float lastInstanceTime, float cooldown)
    {
        return (Time.time - lastInstanceTime >= cooldown);
    }
}
