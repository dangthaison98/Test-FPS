using UnityEngine;
using UnityEngine.UI;

public class ReloadButton : MonoBehaviour
{
    public float duration;
    
    public Button reloadButton;
    public FillAmountEffect fillAmountEffect;
    
    private void Start()
    {
        fillAmountEffect.completeAction += NormalMode;
    }
    public void onClick()
    {
        //Inter Ads
        reloadButton.interactable = false;
        //Reload
        fillAmountEffect.fillSettings.settings.duration = duration;
        fillAmountEffect.Play();
    }

    private void NormalMode()
    {
        reloadButton.interactable = true;
    }
}
