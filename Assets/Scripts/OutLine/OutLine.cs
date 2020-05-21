using System;
using UnityEngine;

public class OutLine : IOutLine
{
    /// <summary>
    /// 自身的框transform
    /// </summary>
    public RectTransform selfRect;

    /// <summary>
    /// 生成的物体名字
    /// </summary>
    public string insLineName;
    /// <summary>
    /// 是否初始化
    /// </summary>
    public bool isInit = false;
    /// <summary>
    /// 是否正在拖拽
    /// </summary>
    public bool isDrag = false;

    /// <summary>
    /// 外框预设
    /// </summary>
    GameObject outLinePrefab;
    /// <summary>
    /// 鼠标图片icon
    /// </summary>
    Texture2D enterIcon;
    public GameObject lineObj;
    protected RectTransform lineObjRect;

    protected Vector2 startDragMousePos;
    /// <summary>
    /// 开始拖拽的时候物体sizeDelta的X值
    /// </summary>
    protected float startDragObjSizeDeltaX;
    protected float startDragObjSizeDeltaY;
    /// <summary>
    /// 开始拖拽时候物体距离父物体边界距离
    /// </summary>
    protected float startDragObjPosX;
    protected float startDragObjPosY;

    /// <summary>
    /// 鼠标移动后计算出来的物体size
    /// </summary>
    protected float newObjDisX;
    protected float newObjDisY;

    /// <summary>
    /// 记录物体世界坐标临时值
    /// </summary>
    Vector2 worldPos;

    public virtual void Init(GameObject go)
    {
        selfRect = go.GetComponent<RectTransform>();
        outLinePrefab = Resources.Load<GameObject>("Prefabs/OutLine");
        enterIcon = Resources.Load<Texture2D>("Texture/MouseEnterIcon");
        lineObj = GameObject.Instantiate(outLinePrefab, selfRect);
        lineObj.name = insLineName;
        lineObjRect = lineObj.GetComponent<RectTransform>();
        EventTriggerListener.Get(lineObj).OnMouseDrag = DragLine;
        EventTriggerListener.Get(lineObj).OnMouseBeginDrag = BeginDragLine;
        EventTriggerListener.Get(lineObj).OnMouseEndDrag = EndDragLine;
        EventTriggerListener.Get(lineObj).OnMouseEnter = EnterLine;
        EventTriggerListener.Get(lineObj).OnMouseExit = ExitLine;
        EventTriggerListener.Get(lineObj).OnMouseDown = ClickAction;
        isInit = true;

    }

    private void ClickAction()
    {
        GameManager.Instance.lastSelectGameObject = GameManager.Instance.selectGameobject;
        GameManager.Instance.selectGameobject = selfRect.gameObject;
    }

    /// <summary>
    /// updata中刷新调用（后续可添加颜色、材质球等属性）
    /// </summary>
    /// <param name="points">物体的四个边界顶点</param>
    /// <param name="lineWidth">线条的宽度</param>
    public virtual void RefreshRect(Vector2[] points, float lineWidth, Color lineColor)
    {

    }
    /// <summary>
    /// 鼠标进入事件 更改鼠标icon
    /// </summary>
    void EnterLine()
    {
        if (!isDrag)
        {
            Cursor.SetCursor(enterIcon, Vector2.zero, CursorMode.Auto);
        }
    }
    /// <summary>
    /// 鼠标退出事件，恢复鼠标icon
    /// </summary>
    void ExitLine()
    {
        if (!isDrag)
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }

    Vector2 startPos;
    Vector2 startSize;
    /// <summary>
    /// 开始拖拽事件
    /// </summary>
    void BeginDragLine()
    {
        //SendCommand();
        isDrag = true;
        startDragMousePos = Input.mousePosition;

        worldPos = selfRect.position;//先记录先物体的世界坐标，防止在更改锚点的时候无法恢复原位

        startDragObjSizeDeltaX = selfRect.sizeDelta.x;
        startDragObjSizeDeltaY = selfRect.sizeDelta.y;

        SetAnchoredPos(); //更改锚点设置
        selfRect.ForceUpdateRectTransforms();//强制刷新下
        selfRect.position = worldPos;
        startPos = selfRect.position;
        startSize = selfRect.sizeDelta;
        GetStartDragObjPos();
    }
    /// <summary>
    /// 更改锚点设置
    /// </summary>
    protected virtual void SetAnchoredPos()
    {

    }
    /// <summary>
    /// 获取距离父物体边界值
    /// </summary>
    protected virtual void GetStartDragObjPos()
    {

    }
    /// <summary>
    /// 拖拽事件
    /// </summary>
    protected virtual void DragLine()
    {

    }
    /// <summary>
    /// 拖拽结束
    /// </summary>
    void EndDragLine()
    {
        isDrag = false;

        SendCommand(selfRect.position, selfRect.sizeDelta, (pos, size) =>
        {
            EventCenter.BroadcastEvent<GameObject, string, string>(EventSendType.InspectorChange, GameManager.Instance.selectGameobject, "PosVectorX", pos.x.ToString());
            EventCenter.BroadcastEvent<GameObject, string, string>(EventSendType.InspectorChange, GameManager.Instance.selectGameobject, "PosVectorY", pos.y.ToString());
            EventCenter.BroadcastEvent<GameObject, string, string>(EventSendType.InspectorChange, GameManager.Instance.selectGameobject, "SizeVectorX", size.x.ToString());
            EventCenter.BroadcastEvent<GameObject, string, string>(EventSendType.InspectorChange, GameManager.Instance.selectGameobject, "SizeVectorY", size.y.ToString());

        });
        ExcuteAllCommand();
    }


    private void SendCommand(Vector2 endPos, Vector2 sizeDelte, Action<Vector2, Vector2> RefreshAction)
    {
        RectReciver reciver = new RectReciver();
        reciver.DoAction += RefreshAction;
        reciver.UnDoAction += RefreshAction;
        reciver.selfRect = selfRect;
        reciver.startPos = startPos;
        reciver.startSizeDelte = startSize;
        reciver.endPos = endPos;
        reciver.endSizeDelte = sizeDelte;
        Command c = new Command(reciver);
        CommadManager.Instance.AddCommand(c);

    }
    void ExcuteAllCommand()
    {
        Debug.Log("执行命令");
        CommadManager.Instance.ExcuteAllCommand();
    }
}

public interface IOutLine
{
    void Init(GameObject go);
    void RefreshRect(Vector2[] points, float lineWidth, Color lineColor);
}
