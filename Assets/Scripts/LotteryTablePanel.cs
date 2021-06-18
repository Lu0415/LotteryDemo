
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 跑馬燈轉盤
/// </summary>
public class LotteryTablePanel : MonoBehaviour
{
    ////單次開始抽獎結束抽獎的事件
    //private Action<bool> PlayingAction;
    
    //抽獎按鈕
    public Button drawBtn;
    ////跳過抽獎動畫
    //public Toggle jumpTgl;

    ////抽獎圖片父物體
    //public Transform rewardImgTran;

    ////轉動特效
    //public Transform eff_TurnFrame;
    ////中獎特效
    //public Transform eff_SelectFrame;
    ////抽獎圖片
    //private Transform[] rewardTransArr;
    //private LotteryCell[] rewardCellArr;
    //private string[] rewardTitleArr;

    //// 前進距離
    //private int moveCount = 0;
    //// 總格子數
    //private int randomTotalGridCount = 0;

    ////默認展示狀態
    //private bool isInitState;
    ////抽獎結束 -- 結束狀態，光環不轉
    //private bool drawEnd;
    ////中獎
    //private bool drawWinning;

    //[Header("展示狀態時間 --> 控制光環轉動初始速度")]
    //public float setrewardTime = 1f;

    //private float rewardTime;
    //private float rewardTiming = 0;
    //private float waitSeconds = 0.001f; // 0.05

    ////當前光環所在獎勵的 Index
    //private int haloIndex = 0;
    ////本次中獎ID
    //private int rewardIndex = 0;

    ////點了抽獎按鈕正在抽獎
    //private bool isOnClickPlaying;

    //public bool IsOnClickPlaying
    //{
    //    get => isOnClickPlaying;
    //    set
    //    {
    //        isOnClickPlaying = value;
    //        if (eff_TurnFrame != null)
    //        {
    //            eff_TurnFrame.gameObject.SetActive(isOnClickPlaying);
    //        }
    //    }
    //}

    //public bool DrawWinning
    //{
    //    get => drawWinning;
    //    set => drawWinning = value;
    //}

    //public bool DrawEnd
    //{
    //    get => drawEnd;
    //    set
    //    {
    //        drawEnd = value;
    //        if (eff_SelectFrame != null)
    //        {
    //            eff_SelectFrame.gameObject.SetActive(drawEnd);
    //        }
    //    }
    //}

    ///// <summary>
    ///// 注册轉盤抽獎事件
    ///// </summary>
    ///// <param name="playingAction"></param>
    //public void SetPlayingAction(Action<bool> playingAction, Action<bool> playingThreeAction)
    //{
    //    PlayingAction = playingAction;
    //}

    public void Start()
    {
        Init();
    }
    
    public void Init()
    {
        
        drawBtn.onClick.AddListener(OnClickDrawFun);
        //rewardTransArr = new Transform[rewardImgTran.childCount];
        //rewardCellArr = new LotteryCell[rewardImgTran.childCount];
        //rewardTitleArr = new string[rewardImgTran.childCount];

        //for (int i = 0; i < rewardImgTran.childCount; i++)
        //{
        //    // 水果
        //    rewardTransArr[i] = rewardImgTran.GetChild(i);
        //    rewardCellArr[i] = rewardTransArr[i].GetComponent<LotteryCell>();
        //    // for test
        //    rewardTitleArr[i] = LotteryInfoController.SharedInstance.rewardArr[UnityEngine.Random.Range(0, 4)];
        //    rewardCellArr[i].SetTitle(title: rewardTitleArr[i]);
        //}

        //// 默認展示時間
        //rewardTime = setrewardTime;
        //rewardTiming = 0;

        //DrawEnd = false;
        //DrawWinning = false;
        //IsOnClickPlaying = false;

        //RePrepare();

    }

    //public void RePrepare()
    //{
    //    if (IsOnClickPlaying)
    //    {
    //        return;
    //    }
    //    rewardTime = setrewardTime;
    //    rewardTiming = 0;

    //    DrawEnd = false;
    //    DrawWinning = false;
    //    IsOnClickPlaying = false;
    //    if (true)
    //    {
    //        for (int i = 0; i < rewardCellArr.Length; i++)
    //        {
    //            rewardCellArr[i].ShowEff(LotteryCell.EffType.all, false);
    //        }
    //    }

    //}

    ///// <summary>
    ///// 從中獎狀態恢復到預設狀態
    ///// </summary>
    ///// <param name="index"></param>
    //public void RestoreDefault(int index = 0)
    //{
    //    index--;
    //    rewardCellArr[index].ShowEff(LotteryCell.EffType.all, false);
    //}


    void Update()
    {
        

        //if (DrawEnd || rewardCellArr == null)
        //{
        //    return;
        //}
        
        //if (!IsOnClickPlaying)
        //{
        //    return;
        //}

        //// 抽獎展示
        //rewardTiming += Time.deltaTime;
        //if (rewardTiming >= rewardTime)
        //{

        //    rewardTiming = 0;

        //    haloIndex++;
        //    moveCount++;
        //    if (haloIndex >= rewardCellArr.Length)
        //    {
        //        haloIndex = 0;
        //    }
        //    SetHaloPos(haloIndex);
        //    //Debug.Log("haloIndex: " + haloIndex.ToString());
        //}
    }

    //// 設置光環顯示位置
    //void SetHaloPos(int index)
    //{

    //    rewardCellArr[index - 1 < 0 ? rewardCellArr.Length - 1 : index - 1].ShowEff(LotteryCell.EffType.turn, false);
    //    rewardCellArr[index].ShowEff(LotteryCell.EffType.turn, true);

    //    // 中獎 && 此ID == 中獎ID
    //    Debug.Log("設置光環顯示位置 rewardTime = " + rewardTime);
    //    Debug.Log("設置光環顯示位置 moveCount = " + moveCount);
    //    if (DrawWinning && index == rewardIndex)
    //    {
    //        moveCount = 0;
    //        IsOnClickPlaying = false;
    //        DrawEnd = true;
    //        if (PlayingAction != null)
    //        {
    //            PlayingAction(false);
    //            Debug.Log("PlayingAction(false)");
    //        }

    //        //展示中獎 index
    //        LotteryInfoController.SharedInstance.setLotteryInfo(title: rewardTitleArr[index]);
    //        Debug.Log("恭喜您中獎，中獎index是：" + index);
    //    }
    //}

    
    void OnClickDrawFun()
    {

        LotteryInfoController.SharedInstance.StartTheLotteryAction();

        //Debug.Log("點擊抽獎按鈕 IsOnClickPlaying = " + IsOnClickPlaying);
        //if (!IsOnClickPlaying)
        //{
            
        //    //haloIndex = -1;
        //    //RePrepare();

        //    // 隨機抽中ID
        //    rewardIndex = UnityEngine.Random.Range(0, rewardCellArr.Length);
        //    Debug.Log("開始抽獎，本次抽獎隨機到的ID是：" + rewardIndex);

        //    var random = UnityEngine.Random.Range(3, 5);
        //    randomTotalGridCount = rewardIndex - haloIndex + (random * rewardCellArr.Length);
        //    Debug.Log("random = " + random);
        //    Debug.Log("randomTotalGridCount = " + randomTotalGridCount);

        //    IsOnClickPlaying = true;
        //    DrawEnd = false;
        //    DrawWinning = false;
        //    if (PlayingAction != null)
        //    {
        //        PlayingAction(true);
        //    }
        //    Debug.Log("點擊抽獎按鈕 jumpTgl = " + jumpTgl);
        //    if (jumpTgl != null && jumpTgl.isOn)
        //    {
        //        rewardTime = 0.02f;
        //        DrawWinning = true;
        //    }
        //    else
        //    {
        //        StartCoroutine(StartDrawAni());
        //    }
                
        //}
    }

    ///// <summary> 
    ///// 開始抽獎動畫 
    ///// 先快後慢 -- 根據需求調整時間 
    ///// </summary> 
    ///// <returns></returns> 
    //IEnumerator StartDrawAni()
    //{
    //    rewardTime = setrewardTime;

    //    int[] stepsValue = new int[4] { 10, 75, 95, 100 };
    //    int[] steps = new int[4];

    //    // 12 70 14 4 
    //    for (int i = 0; i < steps.Length; i++)
    //    {
    //        Debug.Log("stepsValue[i] = " + stepsValue[i]);
    //        steps[i] = (int)((float)randomTotalGridCount / 100 * stepsValue[i]);
    //        Debug.Log("steps[i] = " + steps[i]);
    //    }


    //    Debug.Log("進入 第一區 ");
    //    // 第一區 
    //    waitSeconds = (rewardTime - 0f) / steps[0];
    //    do
    //    {
    //        yield return new WaitForSeconds(rewardTime);
    //        if (rewardTime > 0f)
    //        {
    //            rewardTime -= waitSeconds;
    //        }
    //        else
    //        {
    //            rewardTime = 0f;
    //        }
    //        Debug.Log("moveCount = " + moveCount);
    //    } while (moveCount >= 0 && moveCount < steps[0]);

    //    Debug.Log("進入 第二區 ");
    //    // 第二區
    //    do
    //    {
    //        rewardTime = 0f;
    //        yield return new WaitForSeconds(rewardTime);
    //    } while (moveCount >= steps[0] && moveCount < steps[1]);

    //    Debug.Log("進入 第三區 ");
    //    // 第三區
    //    waitSeconds = (0.5f - 0f) / (steps[2] - steps[1]);
    //    do
    //    {
    //        if (rewardTime <= 0.5f)
    //        {
    //            rewardTime += waitSeconds;
    //        }
    //        else
    //        {
    //            rewardTime = 0.5f;
    //        }
    //        yield return new WaitForSeconds(rewardTime);
    //    } while (moveCount >= steps[1] && moveCount < steps[2]);
    //    DrawWinning = true;

    //    Debug.Log("進入 第四區 ");
    //    // 第四區
    //    waitSeconds = (0.8f - 0.5f) / (steps[3] - steps[2]);
    //    do
    //    {
    //        if (rewardTime <= 0.8f)
    //        {
    //            rewardTime += waitSeconds;
    //        }
    //        else
    //        {
    //            rewardTime = 0.8f;
    //        }
    //        yield return new WaitForSeconds(rewardTime);
    //    } while (moveCount >= steps[2] && moveCount < steps[3] - 1);

    //}

    ///// <summary>
    ///// 開始抽獎動畫
    ///// 先快後慢 -- 根據需求調整時間
    ///// </summary>
    ///// <returns></returns>
    //IEnumerator StartDrawAni()
    //{
    //    rewardTime = setrewardTime;

    //    // 加速 //切20份 (1/0.05) = 20-1; 0-19
    //    for (int i = 0; i < setrewardTime / waitSeconds - 1; i++)
    //    {
    //        yield return new WaitForSeconds(waitSeconds);
    //        rewardTime -= waitSeconds; //1趨近於0
    //    }
    //    Debug.Log("rewardTime 111 = " + rewardTime);

    //    yield return new WaitForSeconds(2f);
    //    // 減速 //切17份 (1/0.05) = 20-4; 0-16
    //    for (int i = 0; i < setrewardTime / waitSeconds - 4; i++) // -4
    //    {
    //        yield return new WaitForSeconds(waitSeconds);
    //        rewardTime += 0.003f; //0.02
    //    }
    //    Debug.Log("rewardTime 222 = " + rewardTime);
    //    yield return new WaitForSeconds(2f);
    //    // 減速 1 - rewardTime
    //    for (int i = 0; i < (1 - rewardTime) / (waitSeconds * 2) - 4; i++) // -4
    //    {
    //        yield return new WaitForSeconds(waitSeconds * 2);
    //        rewardTime += (waitSeconds * 2); //0.02
    //    }
    //    Debug.Log("rewardTime 333 = " + rewardTime);
    //    yield return new WaitForSeconds(0.5f);//0.5
    //    DrawWinning = true;
    //}

    public void OnDestroy()
    {
        Debug.Log("OnDestroy()");
    }

}

