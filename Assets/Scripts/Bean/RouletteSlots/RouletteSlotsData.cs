using UnityEngine;

namespace Assets.Script.Model.Bean
{

    public class SampleData
    {
        public string reward;
        public float score;

        public SampleData(string reward, float score)
        {
            this.reward = reward;
            this.score = score;
        }
    }

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
