using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Assets.Script.Model.Bean;

public class RouletteSlotsPanel : MonoBehaviour
{
    RouletteSlotsInfoPanel _rouletteSlotsInfoPanel;

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

    private string[] _tempSampleChar;
    private string[] _dataArray;

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
        if (GameObject.Find("/Canvas/RouletteSlotsInfoPanel").TryGetComponent<RouletteSlotsInfoPanel>(out RouletteSlotsInfoPanel rouletteSlotsInfoPanel))
        {
            _rouletteSlotsInfoPanel = rouletteSlotsInfoPanel;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(string.Format("panelW:{0},panelH{1}", panelW, panelH));

    }

    /// <summary>
    /// 計算好畫面及項目後回傳
    /// </summary>
    /// <param name="data"></param>
    private void InitRouletteSlotsItem(RouletteSlotsData data)
    {

        _dataArray = data.SampleData;
        var pointArray = data.PointArray;
        var itemW = data.ItemW;
        var itemH = data.ItemH;
        _tempSampleChar = data.TempSampleChar;

        rewardTransArr = new Transform[pointArray.Length];
        rewardCellArr = new RouletteSlotsItem[pointArray.Length];

        var index = 0;
        foreach (var point in pointArray)
        {

            newRouletteItem = Instantiate(rouletteItem);
            newRouletteItem.name = _dataArray[index];
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
            rewardCellArr[index].SetTitle(title: _dataArray[index] + ", " + index.ToString());

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
            _rouletteSlotsInfoPanel.SetRewardCount(_dataArray[index]);
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
            var reward = _tempSampleChar[UnityEngine.Random.Range(0, _tempSampleChar.Length - 1)];
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
