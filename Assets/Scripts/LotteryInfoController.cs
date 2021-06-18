using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LotteryInfoController
{
    private static LotteryInfoController instance = null;
    public static LotteryInfoController SharedInstance
    {
        get
        {
            if (instance == null)
            {
                instance = new LotteryInfoController();
                instance.Init();
            }
            return instance;
        }
    }

    public string[] rewardArr = new string[] { "AA", "BB", "CC", "DD", "EE" };
    public int[] rewardInfoCountArr;

    public bool canUpdateInfo = false;

    private void Init()
    {
        rewardInfoCountArr = new int[rewardArr.Length];
        for (int i = 0; i < rewardInfoCountArr.Length; i++)
        {
            rewardInfoCountArr[i] = 0;
        }
    }

    public void setLotteryInfo(string title)
    {
        switch (title)
        {
            case "AA":
                rewardInfoCountArr[0] += 1;
                canUpdateInfo = true;
                break;
            case "BB":
                rewardInfoCountArr[1] += 1;
                canUpdateInfo = true;
                break;
            case "CC":
                rewardInfoCountArr[2] += 1;
                canUpdateInfo = true;
                break;
            case "DD":
                rewardInfoCountArr[3] += 1;
                canUpdateInfo = true;
                break;
            case "EE":
                rewardInfoCountArr[4] += 1;
                canUpdateInfo = true;
                break;
        }
    }

    ////////

    public TestPanelController testPanelController;

    private int totalItemCount = 20;
    public string[] sampleChar;
    private string[] sampleData;
    private int averageNum;

    public float width = 0;
    public float height = 0;

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
                Debug.Log("低於平均數");
                list.Add(charStr);
            }
            else
            {
                var averageCount = openWith.Values.Sum();
                //
                if (averageCount >= sampleChar.Length * averageNum && count < (averageNum + 1))
                {
                    Debug.Log("加總大於平均數*項目列表數量 且 項目數量<平均數+1");
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
                    Debug.Log("取得小於平均數列表集 重新亂數 TX = \n" + TX);

                    if (filteredList.Count > 0)
                    {
                        Debug.Log("取得小於平均數列表集 重新亂數");
                        random = UnityEngine.Random.Range(0, filteredList.Count - 1);
                        charStr = filteredList[random].Key;
                        count = filteredList[random].Value;
                    }
                    else
                    {
                        Debug.Log("取得小於平均數列表集+1 重新亂數");
                        filteredList = openWith.Where(x => x.Value < averageNum + 1).ToList();
                        random = UnityEngine.Random.Range(0, filteredList.Count - 1);
                        charStr = filteredList[random].Key;
                        count = filteredList[random].Value;
                    }

                    Debug.Log("第二次random random" + random + ", charStr = " + charStr);
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
        Debug.Log("最後的 www = \n" + www);

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
        var aspectRatio = width / height;
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
        var realWidth = Mathf.Floor(width / widthCount);
        var realHeight = Mathf.Floor(height / heightCount);
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
        var topAndBottomMargin = (height - (heightCount * realHeight)) / 2;
        var leftAndRightMargin = (width - (widthCount * realWidth)) / 2;
        //Debug.Log(string.Format("addRealSizeGrid => topAndBottomMargin:{0},leftAndRightMargin:{1}", topAndBottomMargin, leftAndRightMargin));

        //算出每一個座標點
        //友 width , height 得知範圍 -xxx - +xxx
        float[] horizontalPointArray = new float[widthCount];
        float[] verticalPointArray = new float[heightCount];
        var rangeLeft = 0 - (width / 2) + leftAndRightMargin;
        var rangeTop = 0 - (height / 2) + topAndBottomMargin;
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
        testPanelController.CallBackForInitFruitItem(sampleData, points, realHeight, realWidth);
    }

    public void StartTheLotteryAction()
    {
        testPanelController.CallBackForStartTheLottery();
    }
}
