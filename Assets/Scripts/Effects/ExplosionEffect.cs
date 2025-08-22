using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    private const float Duration = 1f;

    public void Play(Vector3 position)
    {
        var effect = Instantiate(gameObject, position, Quaternion.identity);

        Destroy(effect, Duration);
    }
}