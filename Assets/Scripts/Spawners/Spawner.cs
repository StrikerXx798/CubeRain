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
            createFunc: SpawnElement,
            actionOnGet: OnGetElement,
            actionOnRelease: (obj) => obj.gameObject.SetActive(false),
            actionOnDestroy:
            OnDestroyElement,
            defaultCapacity: _maxPoolSize,
            maxSize: _maxPoolSize,
            collectionCheck: true
        );
    }

    private T SpawnElement()
    {
        var element = Instantiate(_prefab, transform.position, Quaternion.identity);
        element.Destroyed += (e) => ReleaseElement(e as T);

        return element;
    }

    private void ReleaseElement(T element)
    {
        Pool.Release(element);
        ActiveCountChanged?.Invoke(Pool.CountActive);
        ElementDestroyed?.Invoke(element);
    }

    private void OnGetElement(T element)
    {
        element.transform.position = transform.position;
        element.gameObject.SetActive(true);
        _spawnedCount++;
        SpawnedCountChanged?.Invoke(_spawnedCount);
        CreatedCountChanged?.Invoke(Pool.CountAll);
        ActiveCountChanged?.Invoke(Pool.CountActive);
    }

    private void OnDestroyElement(T element)
    {
        element.Destroyed -= (e) => ReleaseElement(e as T);
        CreatedCountChanged?.Invoke(Pool.CountAll);
        Destroy(element.gameObject);
    }
}