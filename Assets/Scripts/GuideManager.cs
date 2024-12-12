using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideManager : MonoBehaviour {
    [SerializeField] private GameManager gameManager;
    [SerializeField] private bool _needGuides = true;

    public bool needGuides {
        get { return _needGuides; }
        set {
            _needGuides = value;
            UpdateGuidanceFlags();
        }
    }

    void Start() {
        gameManager = GameManager.Instance; // Assign the GameManager instance
    }

    public void UpdateGuidanceFlags() {
        if (_needGuides) { // show all guidances
            // Get all the Goods objects in the scene
            Goods[] allGoods = FindObjectsOfType<Goods>();

            // Get the next item from the shopping list
            GameManager.ShoppingListItem nextItem = gameManager.GetNextShoppingListItem();

            // Scan all the goods in the scene and set the isNextTargetItem flag
            foreach (Goods goods in allGoods)
            {
                if (goods.goodsType == nextItem.goodsType)
                {
                    goods.isNextTargetItem = true;
                }
                else
                {
                    goods.isNextTargetItem = false;
                }
            }

            // If the shopping list is clear, set the isNextTargetItem flag for the Cashier goods
            if (nextItem.goodsType == GameManager.GoodsType.None)
            {
                foreach (Goods goods in allGoods)
                {
                    if (goods.goodsType == GameManager.GoodsType.Cashier)
                    {
                        goods.isNextTargetItem = true;
                    }
                    else
                    {
                        goods.isNextTargetItem = false;
                    }
                }
            }

        }
        else
        { // clear all guidances
            // Get all the Goods objects in the scene
            Goods[] allGoods = FindObjectsOfType<Goods>();
            foreach (Goods goods in allGoods)
                goods.isNextTargetItem = false;
        }
    }
}
