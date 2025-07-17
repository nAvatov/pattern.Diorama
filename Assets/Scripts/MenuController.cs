using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private CanvasGroup _controllerCanvasGroup;
    [SerializeField] private List<MenuItemView> _menuItems;
    [SerializeField] private MenuItemView _initialItem;

    private void Start()
    {
        if (OSC_settingsController.Instance == null){
            new OSC_settingsController(true);
        }

        SetInitialItem();
        
        InitializeItems();
    }

    public void SetInitialItem()
    {
        if (_initialItem)
        {
            StartCoroutine(SelectItem(_initialItem));
        }
    }

    private void InitializeItems()
    {
        foreach (var item in _menuItems)
        {
            item.ButtonReference.onClick.AddListener(() =>
            {
                StartCoroutine(SelectItem(item));
            });
        }
    }

    private IEnumerator SelectItem(MenuItemView selectedItem)
    {
        _controllerCanvasGroup.interactable = false;
        _controllerCanvasGroup.blocksRaycasts = false;


        foreach (var item in _menuItems)
        {
            if (item.IsSelected)
            {
                yield return item.SetSelectionState(false);
            }
        }

        yield return selectedItem.SetSelectionState(true);
        
        selectedItem.OnSelectionHandler();
        
        _controllerCanvasGroup.interactable = true;
        _controllerCanvasGroup.blocksRaycasts = true;
    }

    public void Refresh()
    {
        _menuItems.ForEach(item => item.QuickSetSelectionState(false));
    }
}
