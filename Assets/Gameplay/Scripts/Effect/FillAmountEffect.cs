using System;
using PrimeTween;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FillAmountEffect : MonoBehaviour
{
    public bool playOnAwake = true;

    [SerializeField] public TweenSettings<float> fillSettings;
    
    public Image image;
    public Action completeAction;
    
    public void Play()
    {
        if (!image)
        {
            image = GetComponent<Image>();
        }
        Tween.UIFillAmount(image, fillSettings).OnComplete(target: this, target => target.CompleteTween());
    }
    public void Stop()
    {
        Tween.StopAll(onTarget: image);
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

    private void CompleteTween()
    {
        completeAction?.Invoke();
    }
}
