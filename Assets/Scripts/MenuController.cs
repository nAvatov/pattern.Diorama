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
        StartCoroutine(SelectItem(_initialItem));
        InitializeItems();
    }

    private void InitializeItems()
    {
        foreach (var item in _menuItems)
        {
            item.ButtonReference.onClick.AddListener(() =>
            {
                Debug.Log("click");
                StartCoroutine(SelectItem(item));
            });
        }
    }

    private IEnumerator SelectItem(MenuItemView selectedItem)
    {
        Debug.Log("Routine start");
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
        
        _controllerCanvasGroup.interactable = true;
        _controllerCanvasGroup.blocksRaycasts = true;
        Debug.Log("Routine end");
    }
}
