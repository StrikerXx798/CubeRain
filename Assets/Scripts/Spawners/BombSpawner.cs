using UnityEngine;

public class BombSpawner : Spawner<Bomb>
{
    public void AttachBombToCube(Cube cube)
    {
        var bomb = Pool.Get();
        bomb.transform.position = cube.transform.position;
    }
}