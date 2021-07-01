using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Script.Model.Bean;
using UnityEngine;
using UnityEngine.UI;

public class RouletteSlotsController : MonoBehaviour
{
    RouletteSlotsPanel _rouletteSlotsPanel;
    RouletteSlotsInfoPanel _rouletteSlotsInfoPanel;
    RouletteSlotsPlayerPanel _playerInfoPanel;
    RouletteSlotsPlayerInfo _playerInfo;

    public Button _rouletteSlotsButton;

    private int totalItemCount = 28;
    public string[] sampleChar;
    private string[] sampleData;
    private int averageNum;

    private float _rouletteSlotsPanelW;
    private float _rouletteSlotsPanelH;

    private float _rouletteSlotsInfoPanelW;
    private float _rouletteSlotsInfoPanelH;

    private List<string> _selectedRewardList;
    private bool _canUseRouletteButton;

    private float _playerCoin; //錢
    private float _playerScore; //積分

    private void Awake()
    {
        _playerCoin = 500;
        _playerScore = 0;

        _selectedRewardList = new List<string>();
        CheckRouletteButtonStatus();

        //開始按鈕可使用狀態
        _rouletteSlotsButton.interactable = false;

        sampleChar = new string[10] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
        sampleData = new string[totalItemCount];

        averageNum = Mathf.FloorToInt((float)sampleData.Length / sampleChar.Length);

        if (GameObject.Find("/Canvas/RouletteSlotsPanel").TryGetComponent<RouletteSlotsPanel>(out RouletteSlotsPanel rouletteSlotsPanel))
        {
            _rouletteSlotsPanel = rouletteSlotsPanel;
            _rouletteSlotsPanel.InitAction(RouletteSlotsAnimationComplete);

            //取得轉盤的大小
            var groupRT = _rouletteSlotsPanel.transform.GetComponent<RectTransform>();
            _rouletteSlotsPanelW = groupRT.sizeDelta.x;
            _rouletteSlotsPanelH = groupRT.sizeDelta.y;

            InitLotteryInfoData();
        }

        if (GameObject.Find("/Canvas/RouletteSlotsInfoPanel").TryGetComponent<RouletteSlotsInfoPanel>(out RouletteSlotsInfoPanel rouletteSlotsInfoPanel))
        {
            _rouletteSlotsInfoPanel = rouletteSlotsInfoPanel;
            _rouletteSlotsInfoPanel.InitInfoAction(OnInfoValueChange);

            Debug.Log("_rouletteSlotsInfoPanel = " + _rouletteSlotsInfoPanel);
            //取得資訊欄位的大小
            var groupRT = _rouletteSlotsInfoPanel.transform.GetComponent<RectTransform>();
            _rouletteSlotsInfoPanelW = groupRT.sizeDelta.x;
            _rouletteSlotsInfoPanelH = groupRT.sizeDelta.y;

            // test
            CalculationAddInfoItem(sampleChar);
        }

        if (GameObject.Find("/Canvas/PlayerInfoPanel").TryGetComponent<RouletteSlotsPlayerPanel>(out RouletteSlotsPlayerPanel rouletteSlotsPlayerPanel))
        {
            _playerInfoPanel = rouletteSlotsPlayerPanel;
        }

        SetPlayerInfo();
    }

    /// <summary>
    /// 使用者資訊
    /// </summary>
    void SetPlayerInfo()
    {
        _playerInfo = new RouletteSlotsPlayerInfo();
        _playerInfo.Name = "Test User";
        _playerInfo.Coin = _playerCoin;
        _playerInfo.Score = _playerScore;

        gameObject.BroadcastMessage("ResetPlayerInfo", _playerInfo, SendMessageOptions.RequireReceiver);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 點擊按鈕開始轉動
    /// </summary>
    public void RouletteButtonPressed()
    {
        if (_canUseRouletteButton)
        {
            gameObject.BroadcastMessage("StartTheRouletteSlots", true, SendMessageOptions.RequireReceiver);
        }
    }

    /// <summary>
    /// 檢查開始按鈕狀態
    /// </summary>
    private void CheckRouletteButtonStatus()
    {
        if (_selectedRewardList.Count == 0)
        {
            _canUseRouletteButton = false;
        }
        else
        {
            _canUseRouletteButton = true;
        }
    }

    /// <summary>
    /// 計算顯示資料
    /// </summary>
    public void InitLotteryInfoData()
    {
        
        List<string> list = new List<string>();

        IDictionary<string, int> openWith = new Dictionary<string, int>();
        openWith = sampleChar.ToArray().ToDictionary(listItem => listItem, listItemCount => 0);

        for (int i = 0; i < sampleData.Length; i++)
        {
            var random = UnityEngine.Random.Range(0, sampleChar.Length - 1);

            var charStr = sampleChar[random];
            //Debug.Log(" random" + random + ", charStr = " + charStr);
            var count = openWith[charStr];
            //Debug.Log("count = " + count);

            if (count < averageNum)
            {
                //低於平均數
                //Debug.Log("低於平均數");
                list.Add(charStr);
            }
            else
            {
                var averageCount = openWith.Values.Sum();
                //
                if (averageCount >= sampleChar.Length * averageNum && count < (averageNum + 1))
                {
                    //加總大於平均數*項目列表數量 且 項目數量<平均數+1
                    //Debug.Log("加總大於平均數*項目列表數量 且 項目數量<平均數+1");
                    list.Add(charStr);
                }
                else
                {
                    var filteredList = openWith.Where(x => x.Value < averageNum).ToList();
                    string TX = "";
                    foreach (var item in filteredList)
                    {
                        TX += String.Format("item.Key = {0}, item.Value = {1} \n", item.Key, item.Value);
                    }
                    //Debug.Log("取得小於平均數列表集 重新亂數 TX = \n" + TX);

                    if (filteredList.Count > 0)
                    {
                        //取得小於平均數列表集 重新亂數
                        //Debug.Log("取得小於平均數列表集 重新亂數");
                        random = UnityEngine.Random.Range(0, filteredList.Count - 1);
                        charStr = filteredList[random].Key;
                        count = filteredList[random].Value;
                    }
                    else
                    {
                        //取得小於平均數列表集+1 重新亂數
                        //Debug.Log("取得小於平均數列表集+1 重新亂數");
                        filteredList = openWith.Where(x => x.Value < averageNum + 1).ToList();
                        random = UnityEngine.Random.Range(0, filteredList.Count - 1);
                        charStr = filteredList[random].Key;
                        count = filteredList[random].Value;
                    }

                    //Debug.Log("第二次random random" + random + ", charStr = " + charStr);
                    list.Add(charStr);
                }
            }

            openWith[charStr] = ++count;
        }

        //test
        string www = "";
        foreach (var item in openWith)
        {
            www += String.Format("item.Key = {0}, item.Value = {1} \n", item.Key, item.Value);
        }
        //Debug.Log("最後的 www = \n" + www);

        sampleData = list.ToArray();

        //test
        string sss = "";
        foreach (var item in sampleData)
        {
            sss += item;

        }
        Debug.Log("最後的sampleData = " + sss);

        // 計算可以置放多少格子
        CalculationAddGrid(totalItemCount + 4);

    }



    /// <summary>
    /// 計算可以置放多少格子
    /// </summary>
    void CalculationAddGrid(int cellCount)
    {
        //寬高比
        var aspectRatio = _rouletteSlotsPanelW / _rouletteSlotsPanelH;
        //Debug.Log(string.Format("aspectRatio:{0}", aspectRatio));
        //佔比
        var widthPercentage = 1;
        var heightPercentage = 1 / aspectRatio;
        //Debug.Log(string.Format("widthPercentage:{0},heightPercentage:{1}", widthPercentage, heightPercentage));
        //分配格數
        var widthCount = Mathf.RoundToInt(cellCount / 2 / (widthPercentage + heightPercentage) * widthPercentage);
        var heightCount = Mathf.RoundToInt(cellCount / 2 / (widthPercentage + heightPercentage) * heightPercentage);
        //Debug.Log(string.Format("widthCount:{0},heightCount:{1}", widthCount, heightCount));

        //子項目真實寬高 (無條件捨去) 避免一起時超過範圍
        var realWidth = Mathf.Floor(_rouletteSlotsPanelW / widthCount);
        var realHeight = Mathf.Floor(_rouletteSlotsPanelH / heightCount);
        //Debug.Log(string.Format("realWidth:{0},realHeight:{1}", realWidth, realHeight));
        addRealSizeGrid(realWidth, realHeight, widthCount, heightCount);
    }

    /// <summary>
    /// 新增真實格子 非正方矩形
    /// </summary>
    /// <param name="realWidth"></param>
    /// <param name="realHeight"></param>
    /// <param name="widthCount"></param>
    /// <param name="heightCount"></param>
    void addRealSizeGrid(float realWidth, float realHeight, int widthCount, int heightCount)
    {
        var topAndBottomMargin = (_rouletteSlotsPanelH - (heightCount * realHeight)) / 2;
        var leftAndRightMargin = (_rouletteSlotsPanelW - (widthCount * realWidth)) / 2;
        //Debug.Log(string.Format("addRealSizeGrid => topAndBottomMargin:{0},leftAndRightMargin:{1}", topAndBottomMargin, leftAndRightMargin));

        //算出每一個座標點
        //友 width , height 得知範圍 -xxx - +xxx
        float[] horizontalPointArray = new float[widthCount];
        float[] verticalPointArray = new float[heightCount];
        var rangeLeft = 0 - (_rouletteSlotsPanelW / 2) + leftAndRightMargin;
        var rangeTop = 0 - (_rouletteSlotsPanelH / 2) + topAndBottomMargin;
        for (int i = 0; i < widthCount; i++)
        {
            horizontalPointArray[i] = rangeLeft + (realWidth * (i + 0.5f));
            //Debug.Log(string.Format("addRealSizeGrid => horizontalPointArray[{0}] :{1}", i, horizontalPointArray[i]));
        }
        for (int i = 0; i < heightCount; i++)
        {
            verticalPointArray[i] = rangeTop + (realHeight * (i + 0.5f));
            //Debug.Log(string.Format("addRealSizeGrid => verticalPointArray[{0}] :{1}", i, verticalPointArray[i]));
        }

        //Debug.Log(string.Format("horizontalPointArray length {0},verticalPointArray length {1}", horizontalPointArray.Length, verticalPointArray.Length));

        Vector3[] points = new Vector3[(widthCount + heightCount) * 2 - 4];
        var horIndex = 0;
        var verIndex = verticalPointArray.Length - 1;
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = new Vector3(horizontalPointArray[horIndex], verticalPointArray[verIndex], 0);
            //Debug.Log(string.Format("i: {0},horIndex {1},verIndex {2} points{3}:{4}", i, horIndex, verIndex, i, points[i]));
            if (i / (widthCount + heightCount - 2) == 0)
            {
                if (horIndex < (horizontalPointArray.Length - 1))
                {
                    horIndex += 1;
                }
                else if (horIndex == (horizontalPointArray.Length - 1) && verIndex > 0)
                {
                    verIndex -= 1;
                }
            }
            else
            {
                if (horIndex > 0)
                {
                    horIndex -= 1;
                }
                else if (horIndex == 0 && verIndex < (verticalPointArray.Length - 1))
                {
                    verIndex += 1;
                }
            }
        }

        //InitRouletteSlotsItem
        RouletteSlotsData data = new RouletteSlotsData();
        data.SampleData = sampleData;
        data.PointArray = points;
        data.ItemW = realWidth;
        data.ItemH = realHeight;
        data.TempSampleChar = sampleChar;

        gameObject.BroadcastMessage("InitRouletteSlotsItem", data, SendMessageOptions.RequireReceiver);
        
    }

    /// <summary>
    /// 
    /// </summary>
    private void CalculationAddInfoItem(string[] sampleArray)
    {
        var widthCount = sampleArray.Length;
        Debug.Log("CalculationAddInfoItem sampleArray.Length = " + sampleArray.Length);
        Debug.Log("CalculationAddInfoItem widthCount = " + widthCount);
        //子項目真實寬高 (無條件捨去) 避免一起時超過範圍
        var realWidth = Mathf.Floor(_rouletteSlotsInfoPanelW / widthCount);
        var realHeight = _rouletteSlotsInfoPanelH;

        var leftAndRightMargin = (_rouletteSlotsInfoPanelW - (widthCount * realWidth)) / 2;

        //算出每一個座標點
        Vector3[] points = new Vector3[widthCount];
        var rangeLeft = 0 - (_rouletteSlotsInfoPanelW / 2) + leftAndRightMargin;
        for (int i = 0; i < widthCount; i++)
        {
            points[i] = new Vector3(rangeLeft + (realWidth * (i + 0.5f)), 0, 0);
            Debug.Log(string.Format("CalculationAddInfoItem => horizontalPointArray[{0}] :{1}", i, points[i]));
        }

        RouletteSlotsInfoData data = new RouletteSlotsInfoData();
        data.PointArray = points;
        data.ItemW = realWidth;
        data.ItemH = realHeight;
        data.TempSampleChar = sampleChar;

        gameObject.BroadcastMessage("InitRouletteSlotsInfoItem", data, SendMessageOptions.RequireReceiver);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="index"></param>
    /// <param name="score"></param>
    void OnItemValueChange(int index, bool isSelected)
    {

        Debug.Log("index: " + index + ", isSelected: " + isSelected);
    }

    /// <summary>
    /// 壓注後回傳
    /// </summary>
    /// <param name="index"></param>
    /// <param name="isSelected"></param>
    void OnInfoValueChange(string reward, bool isSelected)
    {
        Debug.Log("reward: " + reward + ", isSelected: " + isSelected);

        if (isSelected)
        {
            _selectedRewardList.Add(reward);
        }
        else
        {
            _selectedRewardList.Remove(reward);
        }

        if (_selectedRewardList.Count == 0)
        {
            _rouletteSlotsButton.interactable = false;
        }
        else
        {
            _rouletteSlotsButton.interactable = true;
        }
        

        CheckRouletteButtonStatus();
        gameObject.BroadcastMessage("SetSelectedReward", _selectedRewardList, SendMessageOptions.RequireReceiver);
    }

    /// <summary>
    /// 旋轉動畫結束
    /// </summary>
    /// <param name="complete"></param>
    void RouletteSlotsAnimationComplete(bool complete)
    {
        if (complete)
        {
            //還原壓注按鈕
            gameObject.BroadcastMessage("ResetRouletteSlotsInfoItem", true, SendMessageOptions.RequireReceiver);
        }
    }

}
