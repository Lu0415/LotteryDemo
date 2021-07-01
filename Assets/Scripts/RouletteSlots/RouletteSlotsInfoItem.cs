﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RouletteSlotsInfoItem : MonoBehaviour
{

    Action<RouletteSlotsInfoItem> m_onValueChanged;

    [SerializeField] GameObject m_selectedImage;
    
    public Text ScoreText;

    public Text RewardText;

    public Text CountText;

    public Button InfoButton;

    public bool isSelected;

    

    private void Update()
    {
        
    }

    public void FetchData(string title, Action<RouletteSlotsInfoItem> onValueChanged)
    {
        m_onValueChanged = onValueChanged;
        RewardText.text = title;
        isSelected = false;
        
        m_selectedImage.SetActive(false);
    }

    public void SetRewardCountValue()
    {
        Debug.Log("SetRewardValue");

        CountText.text = string.Format("{0}", int.Parse(CountText.text) + 1);
    }

    public void SelectButtonAction()
    {
        Debug.Log("SelectButtonAction = " + isSelected);
        
        if (isSelected)
        {
            isSelected = false;
            m_selectedImage.SetActive(false);
        }
        else
        {
            isSelected = true;
            m_selectedImage.SetActive(true);
        }

        m_onValueChanged.Invoke(this);
    }

    /// <summary>
    /// BroadcastMessage 還原壓注按鈕
    /// </summary>
    private void ResetRouletteSlotsInfoItem()
    {
        isSelected = false;
        m_selectedImage.SetActive(false);
    }
    
}
