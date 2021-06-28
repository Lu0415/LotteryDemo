using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LotaryInfoPanel : MonoBehaviour
{

    //抽獎資訊父物體
    public Transform lotaryInfoTran;
    private Transform[] lotaryInfoArr;
    private LotteryInfoCell[] lotaryInfoCellArr;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        //if (LotteryInfoController.SharedInstance.canUpdateInfo) {
        //    LotteryInfoController.SharedInstance.canUpdateInfo = false;
        //    for (int i = 0; i < lotaryInfoTran.childCount; i++)
        //    {
        //        lotaryInfoCellArr[i].SetItemCount(count: LotteryInfoController.SharedInstance.rewardInfoCountArr[i]);
        //        Debug.Log("LotteryInfoController.SharedInstance.rewardInfoCountArr " + i + ": " + LotteryInfoController.SharedInstance.rewardInfoCountArr[i].ToString());
        //    }
            
        //}
    }

    public void Init()
    {

        lotaryInfoArr = new Transform[lotaryInfoTran.childCount];
        lotaryInfoCellArr = new LotteryInfoCell[lotaryInfoTran.childCount];

        for (int i = 0; i < lotaryInfoTran.childCount; i++)
        {
            // 資訊
            lotaryInfoArr[i] = lotaryInfoTran.GetChild(i);
            lotaryInfoCellArr[i] = lotaryInfoArr[i].GetComponent<LotteryInfoCell>();
            //lotaryInfoCellArr[i].SetItemTitle(title: LotteryInfoController.SharedInstance.rewardArr[i]);
        }

        Debug.Log("lotaryInfoCellArr: " + lotaryInfoCellArr.Length);
        Debug.Log("");
    }
}
