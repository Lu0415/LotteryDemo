using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LotteryInfoCell : MonoBehaviour
{
    public Text[] itemTitleText;
    public Text[] itemCountText;

    public void SetItemTitle(string title)
    {
        for (int i = 0; i < itemTitleText.Length; i++)
        {

            itemTitleText[i].text = title + ": ";
        }
    }

    public void SetItemCount(int count)
    {
        for (int i = 0; i < itemCountText.Length; i++)
        {
            itemCountText[i].text = count.ToString();
                //LotteryInfoController.SharedInstance.rewardInfoCountArr[i].ToString();
        }
        //switch (title)
        //{
        //    case "AA":
        //        ParseCountText(index: 0, count: LotteryInfoController.SharedInstance.AA);
        //        break;
        //    case "BB":
        //        ParseCountText(index: 1, count: LotteryInfoController.SharedInstance.BB);
        //        break;
        //    case "CC":
        //        ParseCountText(index: 2, count: LotteryInfoController.SharedInstance.CC);
        //        break;
        //    case "DD":
        //        ParseCountText(index: 3, count: LotteryInfoController.SharedInstance.DD);
        //        break;
        //    case "EE":
        //        ParseCountText(index: 4, count: LotteryInfoController.SharedInstance.EE);
        //        break;
        //}
    }

    private void ParseCountText(int index, int count)
    {
        
    }
}

