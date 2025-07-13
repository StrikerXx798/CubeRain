using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Cube : MonoBehaviour
{
    private const int MinDestroyDelay = 2;
    private const int MaxDestroyDelay = 5;
    private const float Opacity = 1f;
    private const float MinHue = 0f;
    private const float MaxHue = 1f;
    private const float MinSaturation = 0.7f;
    private const float MaxSaturation = 1f;
    private const float MinBrightness = 0.7f;
    private const float MaxBrightness = 1f;

    private Renderer _renderer;
    private bool _isTouchPlatform;
    private Color _color;
    private IEnumerator _coroutine;

    public event Action<Cube> Destroyed;

    private void Awake()
    {
        _isTouchPlatform = false;
        _renderer = GetComponent<Renderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Platform>(out var _) == false)
            return;

        if (_coroutine != null)
            return;

        _coroutine = OnTouchedPlatform();
        StartCoroutine(_coroutine);
    }

    private void SetRandomColor()
    {
        if (_renderer is null)
            return;

        _color = Random.ColorHSV(
            MinHue,
            MaxHue,
            MinSaturation,
            MaxSaturation,
            MinBrightness,
            MaxBrightness,
            Opacity,
            Opacity
        );

        _renderer.material.color = _color;
    }

    private void ResetToDefault()
    {
        _renderer.material.color = Color.gray;
        _isTouchPlatform = false;
        _coroutine = null;
    }

    private IEnumerator OnTouchedPlatform()
    {
        if (_isTouchPlatform)
            yield return null;

        _isTouchPlatform = true;
        SetRandomColor();
        var delay = Random.Range(MinDestroyDelay, MaxDestroyDelay);
        var wait = new WaitForSeconds(delay);

        yield return wait;

        Destroyed?.Invoke(this);
        ResetToDefault();
    }
}