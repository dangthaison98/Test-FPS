using PrimeTween;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FillAmountEffect : MonoBehaviour
{
    public bool playOnAwake = true;

    [SerializeField] TweenSettings<float> fillSettings;
    
    public Image image;

    public void Play()
    {
        if (!image)
        {
            image = GetComponent<Image>();
        }
        Tween.UIFillAmount(image, fillSettings);
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
}
