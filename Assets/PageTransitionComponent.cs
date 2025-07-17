using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class PageTransitionComponent : MonoBehaviour
{
    [SerializeField] private CanvasGroup _pageCanvasGroup;
    [SerializeField] private float _fadeDuration = 0.5f;

    [SerializeField] private CanvasGroup _targetPageCg;

    public void ChangePage()
    {
        _pageCanvasGroup.interactable = false;
        _pageCanvasGroup.blocksRaycasts = false;
        
        _pageCanvasGroup.DOFade(0f, _fadeDuration)
            .OnComplete(() =>
            {
                _pageCanvasGroup.DOKill();
                
                _targetPageCg.DOFade(1f, _fadeDuration)
                    .OnComplete(() =>
                    {
                        _targetPageCg.interactable = true;
                        _targetPageCg.blocksRaycasts = true;
                        _targetPageCg.DOKill();
                    });
            });
    }

    private void OnDestroy()
    {
        _pageCanvasGroup.DOKill();
        _targetPageCg.DOKill();
    }
}
