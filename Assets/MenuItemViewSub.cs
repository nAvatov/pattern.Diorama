using UnityEngine;

public class MenuItemViewSub : MenuItemView
{
    [SerializeField] private PageTransitionComponent _pageTransitionComponent;
    public override void OnSelectionHandler()
    {
        _pageTransitionComponent.ChangePage();
    }
}
