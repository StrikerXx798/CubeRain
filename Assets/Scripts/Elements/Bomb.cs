using System.Collections;
using UnityEngine;

public class Bomb : Element
{
    private const float MinTime = 0f;
    private const float MinAlpha = 0f;

    [SerializeField] private Exploder _exploder;
    [SerializeField] private float _minLifeTime = 2f;
    [SerializeField] private float _maxLifeTime = 5f;
    [SerializeField] private float _alphaChangeSpeedMultiplier = 1f;

    private Renderer _renderer;
    private Coroutine _explodeRoutine;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();

        if (_explodeRoutine is not null)
            return;

        StartCoroutine(ExplodeCoroutine());
    }

    private void OnDestroy()
    {
        if (_explodeRoutine is null)
            return;

        StopCoroutine(_explodeRoutine);
    }

    private void ChangeAlpha(float alpha)
    {
        var color = _renderer.material.color;
        color.a = alpha;
        _renderer.material.color = color;
    }

    private IEnumerator ExplodeCoroutine()
    {
        var delay = Random.Range(_minLifeTime, _maxLifeTime);
        var wait = new WaitForSeconds(delay);

        var timer = MinTime;
        var startAlpha = _renderer.material.color.a;

        while (timer < delay)
        {
            timer += Time.deltaTime * _alphaChangeSpeedMultiplier;
            var newAlpha = Mathf.Lerp(startAlpha, MinAlpha, timer / delay);
            ChangeAlpha(newAlpha);

            yield return null;
        }

        yield return wait;

        _exploder.CreateExplosion(gameObject.transform.position);
        Destroyed?.Invoke(this);
    }
}