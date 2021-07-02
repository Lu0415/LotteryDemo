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

    public void InitAction(float score, float panelW, float panelH)
    {

        RectTransform rt = this.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f); //錨點最小
        rt.anchorMax = new Vector2(0.5f, 0.5f); //錨點最大
        rt.pivot = new Vector2(0.5f, 0.5f); //軸心
        rt.localScale = new Vector2(1.0f, 1.0f); //縮放
        rt.sizeDelta = new Vector2(panelW, panelH); //長寬

        ScoreText.text = string.Format("SCORE: {0}", score);
    }

    private void SetScoreValue(float score)
    {
        ScoreText.text = string.Format("SCORE: {0}", score);
    }
}
