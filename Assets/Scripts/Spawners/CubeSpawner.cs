using System.Collections;
using UnityEngine;

public class CubeSpawner : Spawner<Cube>
{
    [SerializeField] private BombSpawner _bombSpawner;
    
    private Coroutine _spawnRoutine;

    private void Start()
    {
        _spawnRoutine = StartCoroutine(SpawnRoutine());
        ElementDestroyed += _bombSpawner.AttachBombToCube;
    }

    private void OnDestroy()
    {
        if (_spawnRoutine is not null)
        {
            StopCoroutine(_spawnRoutine);
        }

        ElementDestroyed -= _bombSpawner.AttachBombToCube;
    }

    private IEnumerator SpawnRoutine()
    {
        var wait = new WaitForSeconds(RepeatRate);

        while (enabled)
        {
            Pool.Get();

            yield return wait;
        }
    }
}