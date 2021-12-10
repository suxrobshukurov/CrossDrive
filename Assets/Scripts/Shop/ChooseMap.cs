using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseMap : MonoBehaviour
{
    [SerializeField] private AudioClip btnClick;


    //����� ������������ �������� ��� ����� NowMap
    public void ChooseNewMap(int numberMap)
    {
        if (PlayerPrefs.GetString("music") != "No") // ���� �� �������� ����
        {
            GetComponent<AudioSource>().clip = btnClick;
            GetComponent<AudioSource>().Play(); // �� ��������� �������� ������
        }
        PlayerPrefs.SetInt("NowMap", numberMap);
        GetComponent<CheckMaps>().whichMapSelected();
    }
}
