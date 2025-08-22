using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Spawner<T> : MonoBehaviour where T : Element
{
    [SerializeField] private T _prefab;
    [SerializeField] private float _repeatRate;
    [SerializeField] private int _maxPoolSize;

    private ObjectPool<T> _pool;
    private Coroutine _spawnRoutine;

    private int _spawnedCount;


    public int SpawnedCount => _spawnedCount;
    public int CreatedCount => _pool.CountAll;
    public int ActiveCount => _pool.CountActive;

    private void Awake()
    {
        _pool = new ObjectPool<T>
        (
            createFunc: SpawnElement,
            actionOnGet: OnGetElement,
            actionOnRelease: (obj) => obj.gameObject.SetActive(false),
            actionOnDestroy:
            OnDestroyElement,
            defaultCapacity:
            _maxPoolSize,
            maxSize:
            _maxPoolSize,
            collectionCheck:
            true
        );
    }

    private void Start()
    {
        _spawnRoutine = StartCoroutine(SpawnRoutine());
    }

    public void Get()
    {
        _pool.Get();
    }

    private void OnDestroy()
    {
        if (_spawnRoutine != null)
        {
            StopCoroutine(_spawnRoutine);
        }
    }

    private T SpawnElement()
    {
        var element = Instantiate(_prefab, transform.position, Quaternion.identity);
        element.Destroyed += (e) => ReleaseElement(e as T);

        return element;
    }

    private void ReleaseElement(T element)
    {
        _pool.Release(element);
    }

    private IEnumerator SpawnRoutine()
    {
        var wait = new WaitForSeconds(_repeatRate);

        while (enabled)
        {
            _pool.Get();

            yield return wait;
        }
    }

    private void OnGetElement(T element)
    {
        element.transform.position = transform.position;
        element.gameObject.SetActive(true);
        _spawnedCount++;
    }

    private void OnDestroyElement(T element)
    {
        element.Destroyed -= (e) => ReleaseElement(e as T);
        Destroy(element.gameObject);
    }
}