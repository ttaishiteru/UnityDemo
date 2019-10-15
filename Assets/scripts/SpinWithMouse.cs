using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinWithMouse : MonoBehaviour {

    private bool isClick = false;
    private Vector3 nowPos;
    private Vector3 oldPos;
    // Use this for initialization
    void OnMouseUp()//鼠标抬起
    {
        isClick = false;
    }
     void OnMouseDown()//鼠标按下
    {
        isClick = true;
    }
    private void Update()
    {
        nowPos = Input.mousePosition;
        if (isClick)//鼠标按下不松手
        {
            Vector3 offset = nowPos - oldPos;
            if(Mathf.Abs(offset.x) > Mathf.Abs(offset.y) && Mathf.Abs(offset.x)> 5f)
            {
                transform.Rotate(Vector3.up, -offset.x);
            }
        }
        oldPos = Input.mousePosition;
    }
}
