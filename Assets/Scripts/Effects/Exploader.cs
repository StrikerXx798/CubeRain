using System.Collections.Generic;
using UnityEngine;

public class Exploder : MonoBehaviour
{
    private const float UpwardsModifier = 0.5f;

    [SerializeField] private ExplosionEffect _explosionEffectPrefab;
    [SerializeField] private float _baseExplosionRadius = 10f;
    [SerializeField] private float _baseExplosionForce = 5f;

    public void CreateExplosion(Vector3 explosionCenter)
    {
        var colliders = Physics.OverlapSphere(explosionCenter, _baseExplosionRadius);
        var processedRigidbodies = new HashSet<Rigidbody>();

        foreach (var includesCollider in colliders)
        {
            var rigidBody = includesCollider.attachedRigidbody;

            if (!rigidBody)
                continue;

            if (processedRigidbodies.Contains(rigidBody))
                continue;

            processedRigidbodies.Add(rigidBody);
            rigidBody.AddExplosionForce(
                _baseExplosionForce,
                explosionCenter,
                _baseExplosionRadius,
                UpwardsModifier,
                ForceMode.Impulse
            );
        }

        _explosionEffectPrefab.Play(transform.position);
    }
}