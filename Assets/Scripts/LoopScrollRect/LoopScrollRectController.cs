using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(LoopScrollRectBase))]
[DisallowMultipleComponent]
public class LoopScrollRectController : MonoBehaviour
{
    private LoopScrollRectBase loopCom = null;

    [HideInInspector]
    public GameObject ObjectPool = null;
    
    void Awake()
    {
        loopCom = GetComponent<LoopScrollRectBase>();
        loopCom.OnItemUpdate.AddListener(ProvideData);
        loopCom.OnItemGive.AddListener(GetObject);
        loopCom.OnItemReturn.AddListener(ReturnObject);
        loopCom.OnDragBegin.AddListener(OnBeginDrag);

        if(ObjectPool == null)
        {
            ObjectPool = new GameObject("Pool");
            ObjectPool.transform.parent = transform;
        }
    }

    void OnDestroy()
    {
        if(loopCom != null)
        {
            loopCom.OnItemUpdate.RemoveAllListeners();
            loopCom = null;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    
    public RectTransform GetObject(int index)
    {
        return null;
    }


    /// <summary>
    /// Use `DestroyImmediate` here if you don't need Pool
    /// </summary>
    /// <param name="trans"></param>
    
    public void ReturnObject(Transform trans)
    {
      
    }

    
    void ProvideData(Transform trans, int index)
    {
   
    }


    public void OnBeginDrag()
    {
        //OnItemDrag?.Call(luaTable);
    }


    /// <summary>
    /// 获取loopscroll的content相对路径
    /// </summary>
    /// <returns></returns>
    public string GetContentPath()
    {
        return loopCom.content.name;
    }

    /// <summary>
    /// loopscroll的totalcount封装
    /// </summary>
    public int TotalCount
    {
        get
        {
            return loopCom.TotalCount;
        }
        set
        {
            if(loopCom.TotalCount != value)
            {
                loopCom.TotalCount = value;
            }
            loopCom.RefillCells();
        }
    }

    /// <summary>
    /// loopscroll的RefreshCells封装
    /// 刷新目前显示的格子
    /// </summary>
    public void Refresh()
    {
        loopCom.RefreshCells();
    }

    /// <summary>
    /// 滚动到指定格子，注意目标格子是停留在最上/左方
    /// speed、time参数互斥，优先判定时长
    /// </summary>
    /// <param name="tgtIndex">目标格子标号，从0开始</param>
    /// <param name="time">移动总时长</param>
    /// <param name="speed">每秒移动像素</param>
    public void ScrollToCell(int tgtIndex, float time = -1, float speed = -1)
    {
        if (time > 0)
            loopCom.ScrollToCellWithinTime(tgtIndex, time);
        else
            loopCom.ScrollToCell(tgtIndex, speed);
    }

    public int GetConstraintCount()
    {
        return loopCom.GetConstrainCount();
    }
}