using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RouletteSlotsInfoPanel : MonoBehaviour
{

    public Text ScoreText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetScoreValue(float score)
    {
        ScoreText.text = string.Format("SCORE: {0}", score);
    }
}
