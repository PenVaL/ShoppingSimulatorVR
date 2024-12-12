using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goods : MonoBehaviour {
    public GameManager.GoodsType goodsType;

    [SerializeField] private bool _isHoverEntered = false;
    public bool isHoverEntered {
        get { return _isHoverEntered; }
        set {
            _isHoverEntered = value;
        }
    }

    [SerializeField] private bool _isNextTargetItem = false;
    public bool isNextTargetItem {
        get { return _isNextTargetItem; }
        set {
            _isNextTargetItem = value;
        }
    }
}