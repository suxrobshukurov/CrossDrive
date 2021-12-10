using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseMap : MonoBehaviour
{
    [SerializeField] private AudioClip btnClick;


    //метод устанавлвает значение для ключа NowMap
    public void ChooseNewMap(int numberMap)
    {
        if (PlayerPrefs.GetString("music") != "No") // если не выключин звук
        {
            GetComponent<AudioSource>().clip = btnClick;
            GetComponent<AudioSource>().Play(); // мы запуксаем звуковой эффект
        }
        PlayerPrefs.SetInt("NowMap", numberMap);
        GetComponent<CheckMaps>().whichMapSelected();
    }
}
