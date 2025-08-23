using UnityEngine;

public class BombSpawner : Spawner<Bomb>
{
    public void AttachBombToCube(Cube cube)
    {
        var bomb = Pool.Get();
        bomb.transform.position = cube.transform.position;
    }

    protected override void OnGetPoolElement(Bomb element)
    {
        base.OnGetPoolElement(element);
        element.Destroyed += OnBombDestroyed;
    }

    protected override void OnDestroyPoolElement(Bomb element)
    {
        base.OnDestroyPoolElement(element);
    }

    private void OnBombDestroyed(Element element)
    {
        element.Destroyed -= OnBombDestroyed;
        Pool.Release(element as Bomb);
    }
}