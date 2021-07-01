using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Model.Bean;
using UnityEngine;

public class RouletteSlotsInfoPanel : MonoBehaviour
{

    public Transform panelTransform;
    public GameObject rouletteInfoItem;
    GameObject newRouletteInfoItem;

    private string[] _tempSampleChar;

    private Transform[] _infoItemTransArray;
    private RouletteSlotsInfoItem[] _infoItemArray;

    Action<string, bool> m_onInfoValueChanged;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitInfoAction(Action<string, bool> infoValueChanged)
    {
        m_onInfoValueChanged = infoValueChanged;

    }

    /// <summary>
    /// 計算好資訊的畫面及項目後回傳
    /// </summary>
    /// <param name="data"></param>
    private void InitRouletteSlotsInfoItem(RouletteSlotsInfoData data)
    {
        var pointArray = data.PointArray;
        var itemW = data.ItemW;
        var itemH = data.ItemH;
        _tempSampleChar = data.TempSampleChar;

        _infoItemTransArray = new Transform[pointArray.Length];
        _infoItemArray = new RouletteSlotsInfoItem[pointArray.Length];

        var i = 0;
        foreach (var point in pointArray)
        {
            newRouletteInfoItem = Instantiate(rouletteInfoItem);
            newRouletteInfoItem.name = _tempSampleChar[i];
            newRouletteInfoItem.transform.SetParent(panelTransform);
            newRouletteInfoItem.SetActive(true);

            RectTransform rt = newRouletteInfoItem.GetComponent<RectTransform>();

            rt.anchorMin = new Vector2(0.5f, 0.5f); //錨點最小
            rt.anchorMax = new Vector2(0.5f, 0.5f); //錨點最大
            rt.pivot = new Vector2(0.5f, 0.5f); //軸心
            rt.localScale = new Vector2(1.0f, 1.0f); //縮放
            rt.sizeDelta = new Vector2(itemW, itemH); //長寬
            rt.localPosition = point;

            // 項目
            _infoItemTransArray[i] = transform.GetChild(i);
            _infoItemArray[i] = _infoItemTransArray[i].GetComponent<RouletteSlotsInfoItem>();
            _infoItemArray[i].FetchData(_tempSampleChar[i], OnInfoValueChanged); //.SetTitle(title: _tempSampleChar[i] + ", " + i.ToString());

            i += 1;
        }
    }

    public void SetRewardCount(string reward)
    {
        var i = 0;
        foreach (var item in _infoItemArray)
        {
            if (item.RewardText.text == reward)
            {
                _infoItemArray[i].SetRewardCountValue();
            }

            i += 1;
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="item"></param>
    void OnInfoValueChanged(RouletteSlotsInfoItem item)
    {
        Debug.Log("OnValueChanged(RouletteSlotsInfoItem item)");
        Debug.Log("item.isSelected = " + item.isSelected);
        Debug.Log("item.name = " + item.name);

        m_onInfoValueChanged.Invoke(item.name, item.isSelected);
    }
}
