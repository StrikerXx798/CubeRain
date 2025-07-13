using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Cube))]
public class Spawner : MonoBehaviour
{
    [SerializeField] private int _spawnAreaWidth = 200;
    [SerializeField] private int _spawnAreaLength = 200;

    [SerializeField] private Cube _cube;
    [SerializeField] private float _repeatRate;
    [SerializeField] private int _maxPoolSize;

    private ObjectPool<Cube> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Cube>
        (
            createFunc: SpawnCube,
            actionOnGet: ActionOnGet,
            actionOnRelease: (obj) => obj.gameObject.SetActive(false),
            actionOnDestroy: ActionOnDestroy,
            defaultCapacity: _maxPoolSize,
            maxSize: _maxPoolSize,
            collectionCheck: true
        );
    }

    private void Start()
    {
        InvokeRepeating(nameof(GetCube), 0.0f, _repeatRate);
    }

    private Cube SpawnCube()
    {
        var cube = Instantiate(_cube, transform.position, Quaternion.identity);
        cube.Destroyed += ReleaseCube;

        return cube;
    }

    private void GetCube()
    {
        var spawnPoint = new Vector3(Random.Range(0, _spawnAreaWidth), transform.position.y,
            Random.Range(0, _spawnAreaLength));
        var cube = _pool.Get();
        cube.transform.position = spawnPoint;
    }

    private void ReleaseCube(Cube cube)
    {
        _pool.Release(cube);
    }

    private void ActionOnGet(Cube cube)
    {
        cube.transform.position = transform.position;
        cube.gameObject.SetActive(true);
    }

    private void ActionOnDestroy(Cube cube)
    {
        cube.Destroyed -= ReleaseCube;
        Destroy(cube);
    }
}