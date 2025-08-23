using UnityEngine;
using UnityEngine.Pool;

public abstract class Spawner<T> : MonoBehaviour where T : Element
{
    [SerializeField] private T _prefab;
    [SerializeField] private float _repeatRate;
    [SerializeField] private int _maxPoolSize;

    private int _spawnedCount;

    public event System.Action<int> SpawnedCountChanged;
    public event System.Action<int> CreatedCountChanged;
    public event System.Action<int> ActiveCountChanged;
    public event System.Action<T> ElementDestroyed;

    protected float RepeatRate => _repeatRate;
    protected ObjectPool<T> Pool { get; private set; }

    private void Awake()
    {
        Pool = new ObjectPool<T>
        (
            createFunc: OnCreatePoolElement,
            actionOnGet: OnGetPoolElement,
            actionOnRelease: OnReleasePoolElement,
            actionOnDestroy: OnDestroyPoolElement,
            defaultCapacity: _maxPoolSize,
            maxSize: _maxPoolSize,
            collectionCheck: true
        );
    }

    protected virtual T OnCreatePoolElement()
    {
        var element = Instantiate(_prefab, transform.position, Quaternion.identity);

        return element;
    }

    protected virtual void OnGetPoolElement(T element)
    {
        element.transform.position = transform.position;
        element.gameObject.SetActive(true);
        _spawnedCount++;
        SpawnedCountChanged?.Invoke(_spawnedCount);
        CreatedCountChanged?.Invoke(Pool.CountAll);
        ActiveCountChanged?.Invoke(Pool.CountActive);
    }

    protected virtual void OnReleasePoolElement(T element)
    {
        element.gameObject.SetActive(false);
        ActiveCountChanged?.Invoke(Pool.CountActive);
        ElementDestroyed?.Invoke(element);
    }

    protected virtual void OnDestroyPoolElement(T element)
    {
        CreatedCountChanged?.Invoke(Pool.CountAll);
        Destroy(element.gameObject);
    }
}