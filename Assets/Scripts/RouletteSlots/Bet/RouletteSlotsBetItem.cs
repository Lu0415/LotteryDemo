using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Model.Bean;
using UnityEngine;
using UnityEngine.UI;

public class RouletteSlotsBetItem : MonoBehaviour
{

    Action<RouletteSlotsBetItem> m_onValueChanged;

    [SerializeField] GameObject m_selectedImage;
    
    public Text ScoreText;

    public Text RewardText;

    public Text CountText;

    public Button BetButton;

    public bool isSelected;

    

    private void Update()
    {
        
    }

    public void FetchData(SampleCharData data, Action<RouletteSlotsBetItem> onValueChanged)
    {
        m_onValueChanged = onValueChanged;
        RewardText.text = data.reward;
        ScoreText.text = data.score.ToString();
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
    private void ResetRouletteSlotsBetItem()
    {
        isSelected = false;
        m_selectedImage.SetActive(false);
    }
    
}
