using UnityEngine;
using UnityEngine.UI;

public class InvisibleButton : MonoBehaviour
{
    public float duration;
    
    public Button invisibleButton;
    public FillAmountEffect fillAmountEffect;

    private void Start()
    {
        fillAmountEffect.completeAction += NormalMode;
    }

    public void onClick()
    {
        //Reward Ads
        InvisibleMode();
        fillAmountEffect.fillSettings.settings.duration = duration;
        fillAmountEffect.Play();
    }

    private void InvisibleMode()
    {
        invisibleButton.interactable = false;
    }

    private void NormalMode()
    {
        invisibleButton.interactable = true;
    }
}
