using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MenuItemView : MonoBehaviour 
{
   [SerializeField] private Button _button;
   
   [SerializeField] private CanvasGroup _backgroundBloom;
   [SerializeField] private float _bloomFadeDuration;
   
   [SerializeField] private List<CanvasGroup> _cornerAnimatedElements;
   [SerializeField] private float _cornersAnimationDuration;
   
   public Button ButtonReference => _button;

   public IEnumerator SetSelectionState(bool isSelected)
   {
      _button.interactable = !isSelected;
      
      foreach (var corner in _cornerAnimatedElements)
      {
         corner.transform.DOLocalMove(new Vector3(corner.transform.localPosition.x * -1, corner.transform.localPosition.y * -1, corner.transform.localPosition.z), _cornersAnimationDuration)
            .OnComplete(() => corner.transform.DOKill());

         corner.DOFade(isSelected ? 1f : 0f, _cornersAnimationDuration)
            .OnComplete(() => corner.DOKill());
      }

      
      yield return _backgroundBloom.DOFade(isSelected ? 1f : 0f, _bloomFadeDuration)
         .OnComplete(() => _backgroundBloom.DOKill())
         .WaitForCompletion();
   }

   public void QuickSetSelectionState(bool isSelected)
   {
      _button.interactable = !isSelected;
      
      foreach (var corner in _cornerAnimatedElements)
      {
         corner.transform.localPosition = new Vector3(corner.transform.localPosition.x * -1, corner.transform.localPosition.y * -1, corner.transform.localPosition.z);
         
         corner.alpha = isSelected ? 1f : 0f;
      }
      
      _backgroundBloom.alpha = isSelected ? 1f : 0f;
   }
}
