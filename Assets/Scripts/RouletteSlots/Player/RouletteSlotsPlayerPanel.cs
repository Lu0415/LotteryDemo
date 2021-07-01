using System.Collections;
using System.Collections.Generic;
using Assets.Script.Model.Bean;
using UnityEngine;
using UnityEngine.UI;

public class RouletteSlotsPlayerPanel : MonoBehaviour
{

    public Text _playerName;
    public Text _playerCoin;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="info"></param>
    private void ResetPlayerInfo(RouletteSlotsPlayerInfo info)
    {
        _playerName.text = "暱稱: " + info.Name;
        _playerCoin.text = "金幣: " + info.Coin;
    }
}
