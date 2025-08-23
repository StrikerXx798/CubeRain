using System.Collections;
using UnityEngine;

public class Bomb : Element
{
    private const float MinTime = 0f;
    private const float MinAlpha = 0f;
    private const float MaxAlpha = 1f;

    [SerializeField] private Exploder _exploder;
    [SerializeField] private float _minLifeTime = 2f;
    [SerializeField] private float _maxLifeTime = 5f;
    [SerializeField] private float _alphaChangeSpeedMultiplier = 1f;

    private Renderer _renderer;
    private Coroutine _explodeRoutine;

    private void OnEnable()
    {
        _renderer = GetComponent<Renderer>();
        _explodeRoutine = StartCoroutine(ExplodeCoroutine());
    }

    private void OnDisable()
    {
        if (_explodeRoutine is not null)
        {
            StopCoroutine(_explodeRoutine);
            _explodeRoutine = null;
        }
        ChangeAlpha(MaxAlpha);
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
		ChangeAlpha(MaxAlpha);
	}
}