using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TowerInfoUI : MonoBehaviour
{
    private float _fadeTime = 0.1f;
    [SerializeField] private GameObject tooltipUI;

    private void Start()
    {
        var manager = ServiceLocator.Get<TowerManager>();
        foreach (var tower in manager.AllTowers)
        {
            tower.OnMouseEnterTower += Show;
            tower.OnMouseExitTower += Hide;
        }

        manager.OnTowerAdded += tower =>
        {
            tower.OnMouseEnterTower += Show;
            tower.OnMouseExitTower += Hide;
        };
    }

    private void OnDisable()
    {
        var manager = ServiceLocator.Get<TowerManager>();
        foreach (var tower in manager.AllTowers)
        {
            tower.OnMouseEnterTower -= Show;
            tower.OnMouseExitTower -= Hide;
        }

        manager.OnTowerAdded -= tower =>
        {
            tower.OnMouseEnterTower -= Show;
            tower.OnMouseExitTower -= Hide;
        };
    }

    private void Show(Tower tower)
    {
        Debug.Log("show");
        //Fade in UI.
        //Populate it with tower data.
    }

    private void Hide(Tower tower)
    {
        Debug.Log("hide");
        //Fade out UI.
    }
}
