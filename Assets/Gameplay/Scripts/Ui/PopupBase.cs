using PrimeTween;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public abstract class PopupBase : MonoBehaviour
{
    [SerializeField] protected bool isAppearAnim;
    [SerializeField] protected bool isDisappearAnim;
    [SerializeField, ShowIf("@this.isAppearAnim || this.isDisappearAnim")] protected GameObject popup;
    [SerializeField, ShowIf("@this.isAppearAnim || this.isDisappearAnim")] protected Image background;
    private float backgroundAlpha;
    
    
    [HideInInspector] public string className;

    protected virtual void OnEnable()
    {
        Appear();
    }

    private void Appear()
    {
        if (!isAppearAnim) return;

        if (backgroundAlpha == 0)
        {
            backgroundAlpha = background.color.a;
        }
        
        popup.transform.localScale = Vector3.zero;
        var color = background.color;
        color.a = 0;
        background.color = color;
        Tween.Scale(popup.transform, endValue: 1f, duration: 0.5f, Ease.OutBack, useUnscaledTime: true);
        Tween.Alpha(background, endValue: backgroundAlpha, duration: 0.5f, useUnscaledTime: true);
    }

    private void Disappear()
    {
        if (!isAppearAnim) return;
        popup.transform.localScale = Vector3.one;
        var color = background.color;
        color.a = backgroundAlpha;
        background.color = color;
        Tween.Scale(popup.transform, endValue: 0f, duration: 0.5f, Ease.InBack, useUnscaledTime: true).OnComplete(target: this, target => target.DespawnPopup());
        Tween.Alpha(background, endValue: 0f, duration: 0.5f, useUnscaledTime: true);
    }

    public virtual void Close()
    {
        if (!isDisappearAnim)
        {
            DespawnPopup();
        }
        else
        {
            Disappear();
        }
    }

    private void DespawnPopup()
    {
        PoolManager.Instance.Despawn(className, gameObject);
    }
}

public class PopupBase<T> : PopupBase where T : PopupBase
{
    public static T Create()
    {
        return Create($"Prefabs/Popup/{typeof(T).FullName}");
    }

    private static T Create(string path)
    {
        var prefab = Resources.Load<T>(path);
        var instance = PoolManager.Instance.Spawn(typeof(T).FullName, prefab);
        instance.className = typeof(T).FullName;
        instance.gameObject.SetActive(true);
        return instance;
    }
}