using TMPro;
using UnityEngine;
using System.Collections.Generic;

public abstract class SpawnerStatsBase<T> : MonoBehaviour where T : Element
{
    [SerializeField] private Spawner<T> _spawner;
    [SerializeField] private List<TextMeshProUGUI> _stats;

    private void OnEnable()
    {
        _spawner.SpawnedCountChanged += UpdateSpawnedCount;
        _spawner.CreatedCountChanged += UpdateCreatedCount;
        _spawner.ActiveCountChanged += UpdateActiveCount;
    }

    private void OnDisable()
    {
        _spawner.SpawnedCountChanged -= UpdateSpawnedCount;
        _spawner.CreatedCountChanged -= UpdateCreatedCount;
        _spawner.ActiveCountChanged -= UpdateActiveCount;
    }

    private void UpdateSpawnedCount(int count)
    {
        if (_stats.Count > 0)
        {
            _stats[0].text = $"Количество заспавненых {typeof(T)} за всё время: {count}";
        }
    }

    private void UpdateCreatedCount(int count)
    {
        if (_stats.Count > 1)
        {
            _stats[1].text = $"Количество созданных {typeof(T)}: {count}";
        }
    }

    private void UpdateActiveCount(int count)
    {
        if (_stats.Count > 2)
        {
            _stats[2].text = $"Количество активных {typeof(T)} на сцене: {count}";
        }
    }
}