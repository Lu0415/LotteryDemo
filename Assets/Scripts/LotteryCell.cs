using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LotteryCell : MonoBehaviour
{
    public Transform[] turnEff;
    public Transform[] seletEff;
    public Text[] titleText;

    public enum EffType
    {
        turn,
        select,
        all,
    }

    public void SetTitle(string title)
    {
        for (int i = 0; i < titleText.Length; i++)
        {

            titleText[i].text = title;
        }



    }

    public void ShowEff(EffType efftype, bool isShow)
    {

        switch (efftype)
        {
            case EffType.turn:
                for (int i = 0; i < turnEff.Length; i++)
                {

                    turnEff[i].gameObject.SetActive(isShow);
                }
                break;
            case EffType.select:
                for (int i = 0; i < turnEff.Length; i++)
                {

                    seletEff[i].gameObject.SetActive(isShow);
                }
                break;
            case EffType.all:
                for (int i = 0; i < turnEff.Length; i++)
                {
                    turnEff[i].gameObject.SetActive(isShow);
                    seletEff[i].gameObject.SetActive(isShow);
                }
                break;
            default:
                break;
        }



    }

    public void HideAllEff()
    {
        for (int i = 0; i < turnEff.Length; i++)
        {

            turnEff[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < turnEff.Length; i++)
        {

            seletEff[i].gameObject.SetActive(false);
        }
    }

  

    IEnumerator HideEffAni()
    {
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < turnEff.Length; i++)
        {

            turnEff[i].gameObject.SetActive(false);
        }
    }
}
