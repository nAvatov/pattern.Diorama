using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private CanvasGroup _controllerCanvasGroup;
    [SerializeField] private List<MenuItemView> _menuItems;

    private void Start()
    {
        InitializeItems();
    }

    private void InitializeItems()
    {
        foreach (var item in _menuItems)
        {
            item.ButtonReference.onClick.AddListener(() => StartCoroutine(SelectItem(item)));
        }
    }

    private IEnumerator SelectItem(MenuItemView selectedItem)
    {
        _controllerCanvasGroup.interactable = false;
        _controllerCanvasGroup.blocksRaycasts = false;
        
        _menuItems.ForEach(item =>
        {
            if (item.IsSelected)
            {
                StartCoroutine(item.SetSelectionState(false));
            }
        });

        yield return selectedItem.SetSelectionState(true);
        
        _controllerCanvasGroup.interactable = true;
        _controllerCanvasGroup.blocksRaycasts = true;
    }
}
