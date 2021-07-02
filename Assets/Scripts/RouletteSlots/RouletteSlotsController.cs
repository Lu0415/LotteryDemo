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
    RouletteSlotsBetPanel _rouletteSlotsBetPanel;
    RouletteSlotsPlayerPanel _playerInfoPanel;
    RouletteSlotsInfoPanel _rouletteSlotsInfoPanel;
    RouletteSlotsPlayerInfo _playerInfo;

    public Button _rouletteSlotsButton;

    private int _totalItemCount = 28;
    public List<SampleCharData> sampleChar;
    //public string[] sampleChar;
    private List<SampleCharData> sampleData;
    private int averageNum;

    private float _rouletteSlotsPanelW;
    private float _rouletteSlotsPanelH;

    private float _rouletteSlotsBetPanelW;
    private float _rouletteSlotsBetPanelH;

    private List<SampleCharData> _selectedRewardList;
    private bool _canUseRouletteButton;

    private float _playerCoin; //錢
    private float _playerScore; //積分

    private void Awake()
    {
        _playerCoin = 500;
        _playerScore = 0;

        _selectedRewardList = new List<SampleCharData>();
        CheckRouletteButtonStatus();

        //開始按鈕可使用狀態
        _rouletteSlotsButton.interactable = false;

        //sampleChar = new string[10] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
        sampleChar = new List<SampleCharData>
        {
            new SampleCharData("A", 10),new SampleCharData("B", 20),new SampleCharData("C", 30),new SampleCharData("D", 40),new SampleCharData("E", 50),
            new SampleCharData("F", 10),new SampleCharData("G", 20),new SampleCharData("H", 30),new SampleCharData("I", 40),new SampleCharData("J", 50)
        };
        sampleData = new List<SampleCharData>();

        averageNum = Mathf.FloorToInt((float)_totalItemCount / sampleChar.Count);

        if (GameObject.Find("/Canvas/RouletteSlotsPanel").TryGetComponent<RouletteSlotsPanel>(out RouletteSlotsPanel rouletteSlotsPanel))
        {
            _rouletteSlotsPanel = rouletteSlotsPanel;
            _rouletteSlotsPanel.InitAction(RouletteSlotsAnimationComplete);

            //取得轉盤的大小
            var groupRT = _rouletteSlotsPanel.transform.GetComponent<RectTransform>();
            _rouletteSlotsPanelW = groupRT.sizeDelta.x;
            _rouletteSlotsPanelH = groupRT.sizeDelta.y;

            InitLotteryBetData();
        }

        if (GameObject.Find("/Canvas/RouletteSlotsBetPanel").TryGetComponent<RouletteSlotsBetPanel>(out RouletteSlotsBetPanel rouletteSlotsBetPanel))
        {
            _rouletteSlotsBetPanel = rouletteSlotsBetPanel;
            _rouletteSlotsBetPanel.InitBetAction(OnBetValueChange);

            Debug.Log("_rouletteSlotsBetPanel = " + _rouletteSlotsBetPanel);
            //取得資訊欄位的大小
            var groupRT = _rouletteSlotsBetPanel.transform.GetComponent<RectTransform>();
            _rouletteSlotsBetPanelW = groupRT.sizeDelta.x;
            _rouletteSlotsBetPanelH = groupRT.sizeDelta.y;

            // test
            CalculationAddBetItem(sampleChar);
        }

        if (GameObject.Find("/Canvas/PlayerInfoPanel").TryGetComponent<RouletteSlotsPlayerPanel>(out RouletteSlotsPlayerPanel rouletteSlotsPlayerPanel))
        {
            _playerInfoPanel = rouletteSlotsPlayerPanel;
        }

        if (GameObject.Find("/Canvas/RouletteSlotsInfoPanel").TryGetComponent<RouletteSlotsInfoPanel>(out RouletteSlotsInfoPanel rouletteSlotsInfoPanel))
        {
            _rouletteSlotsInfoPanel = rouletteSlotsInfoPanel;
            _rouletteSlotsInfoPanel.InitAction(_playerScore);


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
            _rouletteSlotsButton.interactable = false;
        }
        else
        {
            _canUseRouletteButton = true;
            _rouletteSlotsButton.interactable = true;
        }
    }

    /// <summary>
    /// 計算顯示資料
    /// </summary>
    public void InitLotteryBetData()
    {

        List<SampleCharData> list = new List<SampleCharData>();
        
        IDictionary<string, int> openWith = new Dictionary<string, int>();

        foreach (var item in sampleChar)
        {
            openWith.Add(item.reward, 0);
        }

        Debug.Log(string.Format("計算顯示資料 averageNum = {0}", averageNum));

        for (int i = 0; i < _totalItemCount; i++)
        {
            var random = UnityEngine.Random.Range(0, sampleChar.Count - 1);

            var charStr = sampleChar[random].reward;
            
            var count = openWith[charStr];

            Debug.Log(string.Format("計算顯示資料 random = {0}, charStr = {1}, count = {2}", random, charStr, count));
            

            if (count < averageNum)
            {
                //低於平均數
                Debug.Log("計算顯示資料 低於平均數");
                //Debug.Log(string.Format("計算顯示資料 sampleChar[random] = {0}", sampleChar[random].reward));
                //list.Add(sampleChar[random]);
            }
            else
            {
                var averageCount = openWith.Values.Sum();
                Debug.Log(string.Format("計算顯示資料 averageCount = {0}", averageCount));
                //
                if (averageCount >= sampleChar.Count * averageNum && count < (averageNum + 1))
                {
                    //加總大於平均數*項目列表數量 且 項目數量<平均數+1
                    Debug.Log("計算顯示資料 加總大於平均數*項目列表數量 且 項目數量<平均數+1");
                    //Debug.Log(string.Format("計算顯示資料 sampleChar[random] = {0}", sampleChar[random].reward));
                    //list.Add(sampleChar[random]);
                }
                else
                {
                    var filteredList = openWith.Where(x => x.Value < averageNum).ToList();
                    string TX = "";
                    foreach (var item in filteredList)
                    {
                        TX += String.Format("item.Key = {0}, item.Value = {1} \n", item.Key, item.Value);
                    }
                    Debug.Log("計算顯示資料 取得小於平均數列表集 重新亂數 TX = \n" + TX);

                    if (filteredList.Count > 0)
                    {
                        //取得小於平均數列表集 重新亂數
                        Debug.Log("計算顯示資料 取得小於平均數列表集 重新亂數");
                        random = UnityEngine.Random.Range(0, filteredList.Count - 1);
                        charStr = filteredList[random].Key;
                        count = filteredList[random].Value;
                    }
                    else
                    {
                        //取得小於平均數列表集+1 重新亂數
                        Debug.Log("計算顯示資料 取得小於平均數列表集+1 重新亂數");
                        filteredList = openWith.Where(x => x.Value < averageNum + 1).ToList();
                        random = UnityEngine.Random.Range(0, filteredList.Count - 1);
                        charStr = filteredList[random].Key;
                        count = filteredList[random].Value;
                    }

                    Debug.Log("計算顯示資料 第二次random random" + random + ", charStr = " + charStr);
                    //Debug.Log(string.Format("計算顯示資料 sampleChar[random] = {0}", sampleChar[random].reward));
                    //list.Add(sampleChar[random]);
                }
            }
            
            var search = new SampleCharDataSearch(charStr);
            list.Add(sampleChar[sampleChar.FindIndex(search.StartsWith)]);

            openWith[charStr] = ++count;
        }

        //test
        string www = "";
        foreach (var item in openWith)
        {
            www += String.Format("item.Key = {0}, item.Value = {1} \n", item.Key, item.Value);
        }
        Debug.Log("計算顯示資料 最後的 www = \n" + www);

        Debug.Log("計算顯示資料 最後的 list.Count = " + list.Count);
        sampleData = list;

        //test
        string sss = "";
        foreach (var item in sampleData)
        {
            sss += item.reward;

        }
        Debug.Log("計算顯示資料 最後的sampleData = " + sss);

        // 計算可以置放多少格子
        CalculationAddGrid(_totalItemCount + 4);

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
    private void CalculationAddBetItem(List<SampleCharData> sampleArray)
    {
        var widthCount = sampleArray.Count;
        Debug.Log("CalculationAddBetItem sampleArray.Length = " + sampleArray.Count);
        Debug.Log("CalculationAddBetItem widthCount = " + widthCount);
        //子項目真實寬高 (無條件捨去) 避免一起時超過範圍
        var realWidth = Mathf.Floor(_rouletteSlotsBetPanelW / widthCount);
        var realHeight = _rouletteSlotsBetPanelH;

        var leftAndRightMargin = (_rouletteSlotsBetPanelW - (widthCount * realWidth)) / 2;

        //算出每一個座標點
        Vector3[] points = new Vector3[widthCount];
        var rangeLeft = 0 - (_rouletteSlotsBetPanelW / 2) + leftAndRightMargin;
        for (int i = 0; i < widthCount; i++)
        {
            points[i] = new Vector3(rangeLeft + (realWidth * (i + 0.5f)), 0, 0);
            Debug.Log(string.Format("CalculationAddBetItem => horizontalPointArray[{0}] :{1}", i, points[i]));
        }

        RouletteSlotsBetData data = new RouletteSlotsBetData();
        data.PointArray = points;
        data.ItemW = realWidth;
        data.ItemH = realHeight;
        data.TempSampleChar = sampleChar;

        gameObject.BroadcastMessage("InitRouletteSlotsBetItem", data, SendMessageOptions.RequireReceiver);
    }

    /// <summary>
    /// 壓注後回傳
    /// </summary>
    /// <param name="index"></param>
    /// <param name="isSelected"></param>
    void OnBetValueChange(SampleCharData reward, bool isSelected)
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

        CheckRouletteButtonStatus();
        gameObject.BroadcastMessage("SetSelectedReward", _selectedRewardList, SendMessageOptions.RequireReceiver);
    }

    /// <summary>
    /// 旋轉動畫結束
    /// </summary>
    /// <param name="complete"></param>
    void RouletteSlotsAnimationComplete(bool isWinning, SampleCharData winningItem)
    {
        var m_score = 0f;
        foreach (var item in _selectedRewardList)
        {
            if (item == winningItem)
            {
                //中獎的
                m_score += (item.score * 10);
            }
            else
            {
                //沒中的
                m_score += (item.score);
            }
        }
        _playerScore += m_score;

        //顯示 score
        gameObject.BroadcastMessage("SetScoreValue", _playerScore, SendMessageOptions.RequireReceiver);

        //還原壓注按鈕
        gameObject.BroadcastMessage("ResetRouletteSlotsBetItem", true, SendMessageOptions.RequireReceiver);
        _selectedRewardList.Clear();
        CheckRouletteButtonStatus();

    }

}
