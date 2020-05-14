using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OutLineManager : SingleMono<OutLineManager>
{
    public OutLine GetOutLine(string key)
    {
        OutLine outLine;
        switch (key)
        {
            case "Up":
                outLine = new OutLineUp();
                break;
            case "Right":
                outLine = new OutLineRight();
                break;
            case "Down":
                outLine = new OutLineDown();
                break;
            case "Left":
                outLine = new OutLineLeft();
                break;
            case "LeftUp":
                outLine = new OutPointLeftUp();
                break;
            case "RightUp":
                outLine = new OutPointRightUp();
                break;
            case "RightDown":
                outLine = new OutPointRightDown();
                break;
            case "LeftDown":
                outLine = new OutPointLeftDown();
                break;
            case "Middle":
                outLine = new OutMoveMiddle();
                break;
            default:
                outLine = null;
                break;
        }
        if(outLine!=null)
            outLine.insLineName = key;
        return outLine;
    }
}
