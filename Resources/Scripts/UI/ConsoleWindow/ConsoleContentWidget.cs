using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ConsoleContentWidget : MonoBehaviour
{
    
    public GameObject consoleItemPrefab;
    public GridLayoutGroup gridLayout;
    public float defaultHeight = 400;
    public float curItemPos = 0;
    public float curContentPos = 0;
    public int   maxItemCount = 10;

    void CalcNewPos()
    {
        RectTransform rt = GetComponent<RectTransform>();
        if (transform.GetChildCount() > maxItemCount)
        {
            Transform oldItem = transform.GetChild(0);
            oldItem.SetParent(GameObject.Find("Recycle").transform);
            oldItem.gameObject.SetActive(false);
            GlobalClient.GetObjectPoolManager().Recycle<GameObject>(oldItem.gameObject);
        }else
        {
            curItemPos += gridLayout.cellSize.y;
        }
        if (curItemPos > rt.sizeDelta.y)
        {               
            rt.sizeDelta = new Vector2(0, rt.sizeDelta.y + gridLayout.cellSize.y);
            curContentPos += gridLayout.cellSize.y;
            rt.localPosition = new Vector3(0, curContentPos);

        } 
    }

    public void Clear()
    {

    }

    public void AddDefaultConsoleItem(string str)
    {
        CreateConsoleItem(str, Color.white, 13);
    }

    public void AddErrorMessage(string str)
    {
        CreateConsoleItem(str, Color.red, 13);
    }

    public void AddWarningMessage(string str)
    {
        CreateConsoleItem(str, Color.yellow, 13);
    }

    // Use this for initialization
    void Start()
    {
        maxItemCount = 20;
    }

    void CreateConsoleItem(string str, Color color, int size)
    {
         
        
        GameObject item = GlobalClient.GetObjectPoolManager().Get<GameObject>();
        if(item == null)
        {
            item = GameObject.Instantiate(consoleItemPrefab);
        }
        item.transform.SetParent(transform);
        item.SetActive(true);
        ConsoleItemWidget itemComp = item.GetComponent<ConsoleItemWidget>();
        itemComp.SetText(str);
        itemComp.SetColor(color);
        itemComp.SetSize(size);
        CalcNewPos();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
