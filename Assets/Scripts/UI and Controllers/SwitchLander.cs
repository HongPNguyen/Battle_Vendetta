using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
public class SwitchLander : MonoBehaviour
{
    private int switchState = 1;
    public GameObject toggleBtn;
    //https://www.youtube.com/watch?v=c6MklI37rl8&ab_channel=OverflowArchives
    public void OnSwitchButtonClicked()
    {
        toggleBtn.transform.DOLocalMoveX(-toggleBtn.transform.localPosition.x, 0.2f);
        switchState = Math.Sign(-toggleBtn.transform.localPosition.x);

        Debug.Log("btn state - " + switchState);
    }
}
