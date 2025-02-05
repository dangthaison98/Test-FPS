using PrimeTween;
using UnityEngine;

public class ScaleEffect : MonoBehaviour
{
    public bool playOnAwake = true;

    [SerializeField] TweenSettings<float> scaleSettings;

    public void Play()
    {
        Tween.Scale(transform, scaleSettings);
    }
    public void Stop()
    {
        Tween.StopAll(transform);
    }


    private void OnEnable()
    {
        if (playOnAwake)
        {
            Play();
        }
    }
    private void OnDisable()
    {
        Stop();
    }
}
