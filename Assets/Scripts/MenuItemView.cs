using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Structures;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class MenuItemView : MonoBehaviour 
{
   [SerializeField] private Button _button;

   [SerializeField] private Image _icon;
   [SerializeField] private Color _chosenColor;
   
   [SerializeField] private CanvasGroup _backgroundBloom;
   [SerializeField] private float _bloomFadeDuration;
   
   [SerializeField] private List<AnimatedCornerStruct> _cornerAnimatedElements;
   [SerializeField] private float _cornersAnimationDuration;
   
   [SerializeField] private string _address;
   [SerializeField] private int _intValue;
   
   public Button ButtonReference => _button;
   
   public bool IsSelected { get; private set; }

   public IEnumerator SetSelectionState(bool isSelected)
   {
      _button.interactable = !isSelected;
      
      IsSelected = isSelected;
      
      foreach (var corner in _cornerAnimatedElements)
      {
         _icon.DOColor(isSelected ? _chosenColor : Color.white, _cornersAnimationDuration)
            .OnComplete(() => _icon.DOKill());

         if (isSelected)
         {
            corner.CornerIconRT.DOAnchorPos(new Vector2(corner.CornerIconRT.anchoredPosition.x * -1, corner.CornerIconRT.anchoredPosition.y * -1), _cornersAnimationDuration)
               .OnComplete(() => corner.CornerIconRT.DOKill());
         }
         else
         {
            corner.CornerIconRT.DOAnchorPos(new Vector2(Math.Abs(corner.CornerIconRT.anchoredPosition.x) * corner.CornerSignMultiplier.x,  Math.Abs(corner.CornerIconRT.anchoredPosition.y) * corner.CornerSignMultiplier.y), _cornersAnimationDuration)
               .OnComplete(() => corner.CornerIconRT.DOKill());
         }
         
         corner.CornerIconRT.GetComponent<CanvasGroup>().DOFade(isSelected ? 1f : 0f, _cornersAnimationDuration)
            .OnComplete(() => corner.CornerIconRT.DOKill());
      }
      
      yield return _backgroundBloom.DOFade(isSelected ? 1f : 0f, _bloomFadeDuration)
         .OnComplete(() => _backgroundBloom.DOKill())
         .WaitForCompletion();
   }

   public virtual void OnSelectionHandler()
   { 
      Debug.Log("Sending data");
      OSC_settingsController.SendOscMessage(_address, _intValue);
   }

   public void QuickSetSelectionState(bool isSelected)
   {
      _button.interactable = !isSelected;
      
      foreach (var corner in _cornerAnimatedElements)
      {
         if (isSelected)
         {
            corner.CornerIconRT.anchoredPosition = new Vector2(corner.CornerIconRT.anchoredPosition.x * -1, corner.CornerIconRT.anchoredPosition.y * -1);
         }
         else
         {
            corner.CornerIconRT.anchoredPosition = new Vector2(Math.Abs(corner.CornerIconRT.anchoredPosition.x) * corner.CornerSignMultiplier.x, Math.Abs(corner.CornerIconRT.anchoredPosition.y) * corner.CornerSignMultiplier.y);
         }
         
         _icon.color = isSelected ? _chosenColor : Color.white;
         
         corner.CornerIconRT.GetComponent<CanvasGroup>().alpha = isSelected ? 1f : 0f;
      }
      
      _backgroundBloom.alpha = isSelected ? 1f : 0f;
   }
}
