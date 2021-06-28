using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class RouletteSlotsPanel : MonoBehaviour
{
    public Transform panelTransform;
    public GameObject rouletteItem;
    GameObject newRouletteItem;

    //單次開始抽獎結束抽獎的事件
    private Action<bool> PlayingAction;

    // 抽獎圖片
    private Transform[] rewardTransArr;
    private RouletteSlotsItem[] rewardCellArr;

    // 展示狀態時間 --> 控制光環轉動初始速度
    private float setrewardTime = 0.5f;

    private float rewardTime;
    private float rewardTiming = 0;
    private float waitSeconds = 0.001f; // 0.05

    // 當前光環所在獎勵的 Index
    private int haloIndex = 0;
    // 本次中獎ID
    private int rewardIndex = 0;
    // 前進距離
    private int moveCount = 0;
    // 總格子數
    private int randomTotalGridCount = 0;

    // 抽獎結束 -- 結束狀態，光環不轉
    private bool drawEnd;
    // 中獎
    private bool drawWinning;

    // 轉動特效
    private Transform eff_TurnFrame;
    // 中獎特效
    private Transform eff_SelectFrame;

    private float panelW;
    private float panelH;

    private int totalItemCount = 20;
    public string[] sampleChar;
    private string[] sampleData;
    private int averageNum;

    //public float width = 0;
    //public float height = 0;

    // 點了抽獎按鈕正在抽獎
    private bool isOnClickPlaying;

    public bool IsOnClickPlaying
    {
        get => isOnClickPlaying;
        set
        {
            isOnClickPlaying = value;
            if (eff_TurnFrame != null)
            {
                eff_TurnFrame.gameObject.SetActive(isOnClickPlaying);
            }
        }
    }

    public bool DrawWinning
    {
        get => drawWinning;
        set => drawWinning = value;
    }

    public bool DrawEnd
    {
        get => drawEnd;
        set
        {
            drawEnd = value;
            if (eff_SelectFrame != null)
            {
                eff_SelectFrame.gameObject.SetActive(drawEnd);
            }
        }
    }

    private void Awake()
    {
        var groupRT = transform.GetComponent<RectTransform>();
        if (groupRT != null)
        {
            panelW = groupRT.sizeDelta.x;
            panelH = groupRT.sizeDelta.y;
            //Debug.Log(string.Format("panelW:{0},panelH{1}", panelW, panelH));
            //LotteryInfoController.SharedInstance.testPanelController = this;

            //LotteryInfoController.SharedInstance.width = panelW;
            //LotteryInfoController.SharedInstance.height = panelH;

            // 計算顯示資料
            InitLotteryInfoData();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(string.Format("panelW:{0},panelH{1}", panelW, panelH));
    }

    // 計算顯示資料
    public void InitLotteryInfoData()
    {
        sampleChar = new string[10] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
        sampleData = new string[totalItemCount];

        averageNum = Mathf.FloorToInt((float)sampleData.Length / sampleChar.Length);

        List<string> list = new List<string>();

        IDictionary<string, int> openWith = new Dictionary<string, int>();
        openWith = sampleChar.ToArray().ToDictionary(listItem => listItem, listItemCount => 0);

        for (int i = 0; i < sampleData.Length; i++)
        {
            var random = UnityEngine.Random.Range(0, sampleChar.Length - 1);

            var charStr = sampleChar[random];
            Debug.Log(" random" + random + ", charStr = " + charStr);
            var count = openWith[charStr];
            Debug.Log("count = " + count);

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
        var aspectRatio = panelW / panelH;
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
        var realWidth = Mathf.Floor(panelW / widthCount);
        var realHeight = Mathf.Floor(panelH / heightCount);
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
        var topAndBottomMargin = (panelH - (heightCount * realHeight)) / 2;
        var leftAndRightMargin = (panelW - (widthCount * realWidth)) / 2;
        //Debug.Log(string.Format("addRealSizeGrid => topAndBottomMargin:{0},leftAndRightMargin:{1}", topAndBottomMargin, leftAndRightMargin));
        
        //算出每一個座標點
        //友 width , height 得知範圍 -xxx - +xxx
        float[] horizontalPointArray = new float[widthCount];
        float[] verticalPointArray = new float[heightCount];
        var rangeLeft = 0 - (panelW / 2) + leftAndRightMargin;
        var rangeTop = 0 - (panelH / 2) + topAndBottomMargin;
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

        //
        InitRouletteItem(sampleData, points, realHeight, realWidth);
    }

    /// <summary>
    /// 計算好畫面及項目後回傳
    /// </summary>
    /// <param name="dataArray"></param>
    /// <param name="pointArray"></param>
    /// <param name="itemH"></param>
    /// <param name="itemW"></param>
    public void InitRouletteItem(string[] dataArray, Vector3[] pointArray, float itemH, float itemW)
    {
        rewardTransArr = new Transform[pointArray.Length];
        rewardCellArr = new RouletteSlotsItem[pointArray.Length];
        
        var index = 0;
        foreach (var point in pointArray)
        {

            newRouletteItem = Instantiate(rouletteItem);
            newRouletteItem.name = dataArray[index];
            newRouletteItem.transform.SetParent(panelTransform);
            newRouletteItem.SetActive(true);

            RectTransform rt = newRouletteItem.GetComponent<RectTransform>();

            rt.anchorMin = new Vector2(0.5f, 0.5f); //錨點最小
            rt.anchorMax = new Vector2(0.5f, 0.5f); //錨點最大
            rt.pivot = new Vector2(0.5f, 0.5f); //軸心
            rt.localScale = new Vector2(1.0f, 1.0f); //縮放
            rt.sizeDelta = new Vector2(itemW, itemH); //長寬
            rt.localPosition = point;
            
            // 項目
            rewardTransArr[index] = transform.GetChild(index);
            rewardCellArr[index] = rewardTransArr[index].GetComponent<RouletteSlotsItem>();
            rewardCellArr[index].SetTitle(title: dataArray[index] + ", " + index.ToString());

            index += 1;
        }

        // 默認展示時間
        rewardTime = setrewardTime;
        rewardTiming = 0;

        DrawEnd = false;
        DrawWinning = false;
        IsOnClickPlaying = false;

        RePrepare();

    }

    /// <summary>
    /// 回復初始狀態
    /// </summary>
    public void RePrepare()
    {
        if (IsOnClickPlaying)
        {
            return;
        }
        rewardTime = setrewardTime;
        rewardTiming = 0;

        DrawEnd = false;
        DrawWinning = false;
        IsOnClickPlaying = false;
        if (true)
        {
            for (int i = 0; i < rewardCellArr.Length; i++)
            {
                rewardCellArr[i].ShowEff(RouletteSlotsItem.EffType.all, false);
            }
        }
    }

    /// <summary>
    /// 從中獎狀態恢復到預設狀態
    /// </summary>
    /// <param name="index"></param>
    public void RestoreDefault(int index = 0)
    {
        index--;
        rewardCellArr[index].ShowEff(RouletteSlotsItem.EffType.all, false);
    }

    void Update()
    {
        if (DrawEnd || rewardCellArr == null)
        {
            return;
        }

        if (!IsOnClickPlaying)
        {
            return;
        }

        // 抽獎展示
        rewardTiming += Time.deltaTime;
        if (rewardTiming >= rewardTime)
        {
            rewardTiming = 0;

            haloIndex++;
            moveCount++;
            if (haloIndex >= rewardCellArr.Length)
            {
                haloIndex = 0;
            }
            SetHaloPos(haloIndex);
            //Debug.Log("haloIndex: " + haloIndex.ToString());
        }
    }

    /// <summary>
    /// 設置光環顯示位置
    /// </summary>
    void SetHaloPos(int index)
    {

        rewardCellArr[index - 1 < 0 ? rewardCellArr.Length - 1 : index - 1].ShowEff(RouletteSlotsItem.EffType.turn, false);
        rewardCellArr[index].ShowEff(RouletteSlotsItem.EffType.turn, true);

        // 中獎 && 此ID == 中獎ID
        //Debug.Log("設置光環顯示位置 rewardTime = " + rewardTime);
        //Debug.Log("設置光環顯示位置 moveCount = " + moveCount);
        if (DrawWinning && index == rewardIndex)
        {
            moveCount = 0;
            IsOnClickPlaying = false;
            DrawEnd = true;
            if (PlayingAction != null)
            {
                PlayingAction(false);
                Debug.Log("PlayingAction(false)");
            }

            //展示中獎 index
            //LotteryInfoController.SharedInstance.setLotteryInfo(title: rewardTitleArr[index]);
            Debug.Log("恭喜您中獎，中獎index是：" + index);
        }
    }

    /// <summary> 
    /// 開始執行抽獎
    /// </summary> 
    private void StartTheRouletteSlots()
    {
        Debug.Log("點擊抽獎按鈕 IsOnClickPlaying = " + IsOnClickPlaying);
        if (!IsOnClickPlaying)
        {
            var reward = sampleChar[UnityEngine.Random.Range(0, sampleChar.Length - 1)];
            Debug.Log("點擊抽獎按鈕 reward：" + reward);
            var rewardIndexList = rewardCellArr.Select((elem, index) => new { elem, index }).Where(x => x.elem.titleText[0].text.Contains(reward)).Select((elem, index) => elem.index).ToList();


            //Debug.Log("點擊抽獎按鈕 rewardIndexList：" + item.index + ", length : " + item.elem.titleText.Length);

            // 隨機抽中ID
            rewardIndex = rewardIndexList[UnityEngine.Random.Range(0, rewardIndexList.Count - 1)];

            Debug.Log("開始抽獎，本次抽獎隨機到的ID是：" + rewardIndex);

            var random = UnityEngine.Random.Range(3, 5);
            randomTotalGridCount = rewardIndex - haloIndex + (random * rewardCellArr.Length);
            Debug.Log("random = " + random);
            Debug.Log("randomTotalGridCount = " + randomTotalGridCount);


            IsOnClickPlaying = true;
            DrawEnd = false;
            DrawWinning = false;

            if (PlayingAction != null)
            {
                PlayingAction(true);
            }

            StartCoroutine(StartDrawAni());

        }
    }

    /// <summary> 
    /// 開始抽獎動畫 
    /// 先快後慢 -- 根據需求調整時間 
    /// </summary> 
    /// <returns></returns> 
    IEnumerator StartDrawAni()
    {
        rewardTime = setrewardTime;

        int[] stepsValue = new int[4] { 10, 75, 95, 100 };
        int[] steps = new int[4];

        // 12 70 14 4 
        for (int i = 0; i < steps.Length; i++)
        {
            Debug.Log("stepsValue[i] = " + stepsValue[i]);
            steps[i] = (int)((float)randomTotalGridCount / 100 * stepsValue[i]);
            Debug.Log("steps[i] = " + steps[i]);
        }


        Debug.Log("進入 第一區 ");
        // 第一區 
        waitSeconds = (rewardTime - 0f) / steps[0];
        do
        {
            yield return new WaitForSeconds(rewardTime);
            if (rewardTime > 0f)
            {
                rewardTime -= waitSeconds;
            }
            else
            {
                rewardTime = 0f;
            }
            Debug.Log("moveCount = " + moveCount);
        } while (moveCount >= 0 && moveCount < steps[0]);

        Debug.Log("進入 第二區 ");
        // 第二區
        do
        {
            rewardTime = 0f;
            yield return new WaitForSeconds(rewardTime);
        } while (moveCount >= steps[0] && moveCount < steps[1]);

        Debug.Log("進入 第三區 ");
        // 第三區
        waitSeconds = (0.5f - 0f) / (steps[2] - steps[1]);
        do
        {
            if (rewardTime <= 0.5f)
            {
                rewardTime += waitSeconds;
            }
            else
            {
                rewardTime = 0.5f;
            }
            yield return new WaitForSeconds(rewardTime);
        } while (moveCount >= steps[1] && moveCount < steps[2]);
        DrawWinning = true;

        Debug.Log("進入 第四區 ");
        // 第四區
        waitSeconds = (0.8f - 0.5f) / (steps[3] - steps[2]);
        do
        {
            if (rewardTime <= 0.8f)
            {
                rewardTime += waitSeconds;
            }
            else
            {
                rewardTime = 0.8f;
            }
            yield return new WaitForSeconds(rewardTime);
        } while (moveCount >= steps[2] && moveCount < steps[3] - 1);

    }
}
