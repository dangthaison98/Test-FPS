using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class VisionButton : MonoBehaviour
{
    public UniversalRenderPipelineAsset normalRenderer;
    public UniversalRenderPipelineAsset visionRenderer;

    public float duration;
    
    public Button visionButton;
    public FillAmountEffect fillAmountEffect;

    private void Start()
    {
        fillAmountEffect.completeAction += NormalMode;
    }

    public void onClick()
    {
        //Reward Ads
        VisionMode();
        fillAmountEffect.fillSettings.settings.duration = duration;
        fillAmountEffect.Play();
    }

    private void VisionMode()
    {
        visionButton.interactable = false;
        GraphicsSettings.renderPipelineAsset = visionRenderer;
    }

    private void NormalMode()
    {
        visionButton.interactable = true;
        GraphicsSettings.renderPipelineAsset = normalRenderer;
    }
}
