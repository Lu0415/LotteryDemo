using UnityEngine;

namespace Assets.Script.Model.Bean
{
    /// <summary>
    /// 
    /// </summary>
    public class RouletteSlotsData
    {
        public string[] SampleData;

        public Vector3[] PointArray;

        public float ItemW;

        public float ItemH;

        public string[] TempSampleChar;
    }

    /// <summary>
    /// 下注資訊
    /// </summary>
    public class RouletteSlotsBetData
    {
        public Vector3[] PointArray;

        public float ItemW;

        public float ItemH;

        public string[] TempSampleChar;
    }

    /// <summary>
    /// 玩家資訊
    /// </summary>
    public class RouletteSlotsPlayerInfo
    {
        public string Name;

        public float Coin;

        public float Score;
    }

}
