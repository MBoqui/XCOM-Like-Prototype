using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : Hittable
{
    void OnDestroy()
    {
        TreeManager.Instance.RemoveTree(this);
    }
}
