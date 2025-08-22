using System.Linq;
using TMPro;
using UnityEngine;

public class SpawnerStats : MonoBehaviour
{
    private CubeSpawner[] _cubeSpawners;
    private BombSpawner[] _bombSpawners;

    [SerializeField] private TextMeshProUGUI[] _cubesTexts;
    [SerializeField] private TextMeshProUGUI[] _bombsTexts;

    private int _totalCubesSpawned;
    private int _totalCubesCreated;
    private int _totalCubesActive;
    private int _totalBombsSpawned;
    private int _totalBombsCreated;
    private int _totalBombsActive;

    void Update()
    {
        _cubeSpawners = FindObjectsByType<CubeSpawner>(FindObjectsSortMode.None);
        _bombSpawners = FindObjectsByType<BombSpawner>(FindObjectsSortMode.None);

        _totalCubesSpawned = _cubeSpawners.Sum(s => s.SpawnedCount);
        _totalCubesCreated = _cubeSpawners.Sum(s => s.CreatedCount);
        _totalCubesActive = _cubeSpawners.Sum(s => s.ActiveCount);

        _totalBombsSpawned = _bombSpawners.Sum(s => s.SpawnedCount);
        _totalBombsCreated = _bombSpawners.Sum(s => s.CreatedCount);
        _totalBombsActive = _bombSpawners.Sum(s => s.ActiveCount);

        UpdateTexts();
    }

    private void UpdateTexts()
    {
        _cubesTexts[0].text = $"Количество заспавненых кубов за всё время: {_totalCubesSpawned}";
        _cubesTexts[1].text = $"Количество созданных кубов: {_totalCubesCreated}";
        _cubesTexts[2].text = $"Количество активных кубов на сцене: {_totalCubesActive}";

        _bombsTexts[0].text = $"Количество заспавненых бомб за всё время: {_totalBombsSpawned}";
        _bombsTexts[1].text = $"Количество созданных бомб: {_totalBombsCreated}";
        _bombsTexts[2].text = $"Количество активных бомб на сцене: {_totalBombsActive}";
    }
}