using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseMap : MonoBehaviour
{
    // [SerializeField] private AudioClip btnClick;


    //метод устанавлвает значение для ключа NowMap
    public void ChooseNewMap(int numberMap)
    {
        PlayerPrefs.SetInt("NowMap", numberMap);
        GetComponent<CheckMaps>().whichMapSelected();
    }
}
