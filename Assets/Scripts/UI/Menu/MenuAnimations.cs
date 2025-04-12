using UnityEngine;
using DG.Tweening;

public class MenuAnimations : MonoBehaviour
{
    [SerializeField] private CanvasGroup _mainCanvasGroup;
    [SerializeField] private CanvasGroup _settingsCanvasGroup;
    private const float FadeTime = 0.5f;

    private void SwitchCanvas(CanvasGroup from, CanvasGroup to)
    {
        from.DOFade(0, FadeTime).OnComplete(() => from.interactable = false);
        to.DOFade(1, FadeTime).OnComplete(() => to.interactable = true);
    }

    public void ShowMainCanvas() => SwitchCanvas(_settingsCanvasGroup, _mainCanvasGroup);

    public void ShowSettingsCanvas() => SwitchCanvas(_mainCanvasGroup, _settingsCanvasGroup);
}
