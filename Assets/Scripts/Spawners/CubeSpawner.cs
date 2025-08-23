using System.Collections;
using UnityEngine;

public class CubeSpawner : Spawner<Cube>
{
    [SerializeField] private BombSpawner _bombSpawner;
    
    private Coroutine _spawnRoutine;

    private void Start()
    {
        _spawnRoutine = StartCoroutine(SpawnRoutine());
    }

    private void OnDestroy()
    {
        if (_spawnRoutine is not null)
        {
            StopCoroutine(_spawnRoutine);
        }
    }

    protected override void OnGetPoolElement(Cube element)
    {
        base.OnGetPoolElement(element);
        element.Destroyed += OnCubeDestroyed;
    }

    protected override void OnReleasePoolElement(Cube element)
    {
        element.ResetToDefault();
        base.OnReleasePoolElement(element);
    }

    protected override void OnDestroyPoolElement(Cube element)
    {
        base.OnDestroyPoolElement(element);
    }

    private void OnCubeDestroyed(Element element)
    {
        element.Destroyed -= OnCubeDestroyed;
        _bombSpawner.AttachBombToCube(element as Cube);
        Pool.Release(element as Cube);
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