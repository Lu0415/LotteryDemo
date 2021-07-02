﻿using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Model.Bean;
using UnityEngine;

public class RouletteSlotsBetPanel : MonoBehaviour
{

    public Transform panelTransform;
    public GameObject rouletteBetItem;
    GameObject newRouletteBetItem;

    private List<SampleCharData> _tempSampleChar;

    private Transform[] _betItemTransArray;
    private RouletteSlotsBetItem[] _betItemArray;

    Action<SampleCharData, bool> m_onBetValueChanged;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitBetAction(Action<SampleCharData, bool> betValueChanged)
    {
        m_onBetValueChanged = betValueChanged;

    }

    /// <summary>
    /// 計算好資訊的畫面及項目後回傳
    /// </summary>
    /// <param name="data"></param>
    private void InitRouletteSlotsBetItem(RouletteSlotsBetData data)
    {
        var pointArray = data.PointArray;
        var itemW = data.ItemW;
        var itemH = data.ItemH;
        _tempSampleChar = data.TempSampleChar;

        _betItemTransArray = new Transform[pointArray.Length];
        _betItemArray = new RouletteSlotsBetItem[pointArray.Length];

        var i = 0;
        foreach (var point in pointArray)
        {
            newRouletteBetItem = Instantiate(rouletteBetItem);
            newRouletteBetItem.name = _tempSampleChar[i].reward;
            newRouletteBetItem.transform.SetParent(panelTransform);
            newRouletteBetItem.SetActive(true);

            RectTransform rt = newRouletteBetItem.GetComponent<RectTransform>();

            rt.anchorMin = new Vector2(0.5f, 0.5f); //錨點最小
            rt.anchorMax = new Vector2(0.5f, 0.5f); //錨點最大
            rt.pivot = new Vector2(0.5f, 0.5f); //軸心
            rt.localScale = new Vector2(1.0f, 1.0f); //縮放
            rt.sizeDelta = new Vector2(itemW, itemH); //長寬
            rt.localPosition = point;

            // 項目
            _betItemTransArray[i] = transform.GetChild(i);
            _betItemArray[i] = _betItemTransArray[i].GetComponent<RouletteSlotsBetItem>();
            _betItemArray[i].FetchData(_tempSampleChar[i], OnBetValueChanged); //.SetTitle(title: _tempSampleChar[i] + ", " + i.ToString());

            i += 1;
        }
    }

    public void SetRewardCount(SampleCharData data)
    {
        var i = 0;
        foreach (var item in _betItemArray)
        {
            if (item.RewardText.text == data.reward)
            {
                _betItemArray[i].SetRewardCountValue();
            }

            i += 1;
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="betItem"></param>
    void OnBetValueChanged(RouletteSlotsBetItem betItem)
    {
        Debug.Log("OnValueChanged(RouletteSlotsBetItem item)");
        Debug.Log("item.isSelected = " + betItem.isSelected);
        Debug.Log("item.name = " + betItem.name);

        foreach (var item in _tempSampleChar)
        {
            if (item.reward == betItem.name)
            {
                m_onBetValueChanged.Invoke(item, betItem.isSelected);
            }
        }
    }
}
