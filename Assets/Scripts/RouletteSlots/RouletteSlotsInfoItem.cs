using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RouletteSlotsInfoItem : MonoBehaviour
{
    public Text ScoreText;

    public Text RewardText;

    public Text CountText;


    public void FetchData(string title)
    {
        RewardText.text = title;
    }

    public void SetRewardCountValue()
    {
        Debug.Log("SetRewardValue");

        CountText.text = string.Format("{0}", int.Parse(CountText.text) + 1);
    }
}
