using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RouletteSlotsController : MonoBehaviour
{
    //RouletteSlotsPanel _rouletteSlotsPanel;

    private void Awake()
    {
        //if (GameObject.Find("/RouletteSlotsPanel").TryGetComponent<RouletteSlotsPanel>(out RouletteSlotsPanel rouletteSlotsPanel))
        //{
        //    _rouletteSlotsPanel = rouletteSlotsPanel;
        //}
    }

    // Start is called before the first frame update
    void Start()
    {
        //_rouletteSlotsPanel.GetComponent<RouletteSlotsPanel>().
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
        gameObject.BroadcastMessage("StartTheRouletteSlots", true, SendMessageOptions.RequireReceiver);
    }

    

    
}
