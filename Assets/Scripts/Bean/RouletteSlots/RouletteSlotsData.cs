using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.Model.Bean
{

    public class SampleCharData
    {
        public string reward;
        public float score;

        public SampleCharData(string reward, float score)
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
        public List<SampleCharData> SampleData;

        public Vector3[] PointArray;

        public float ItemW;

        public float ItemH;

        public List<SampleCharData> TempSampleChar;
    }

    /// <summary>
    /// 下注資訊
    /// </summary>
    public class RouletteSlotsBetData
    {
        public Vector3[] PointArray;

        public float ItemW;

        public float ItemH;

        public List<SampleCharData> TempSampleChar;
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

    /// <summary>
    /// 
    /// </summary>
    public class SampleCharDataSearch
    {
        string _reward;

        public SampleCharDataSearch(string r)
        {
            _reward = r;
        }

        public bool StartsWith(SampleCharData d)
        {
            return d.reward.StartsWith(_reward, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
