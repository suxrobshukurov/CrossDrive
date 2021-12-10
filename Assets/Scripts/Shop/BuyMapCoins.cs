using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyMapCoins : MonoBehaviour
{
    // ���� ��� ������ � �������� 
    [SerializeField] public GameObject _coins1000, _coins5000, _money0_99, _money1_99, _city_btn, _megapolis_btn;
    // ���� ��� ��������
    [SerializeField] private Animation _cointsText;
    // ���� ������ ����� 
    [SerializeField] private Text _coinsCount;


    //����� ��� ������� ����
    public void BuyNewMap(int needCoins)
    {
        int coins = PlayerPrefs.GetInt("Coins");
        if (coins < needCoins)
        {
            _cointsText.Play(); // ����������� �������� ���� ��� �����
        }
        else
        {
            //������� ����� 
            switch (needCoins)
            {
                case 1000:
                    PlayerPrefs.SetString("City", "Open"); // ��������� ����� � ������ ������
                    PlayerPrefs.SetInt("NowMap", 2); // �������� �������� 2 ����� NowMap
                    GetComponent<CheckMaps>().whichMapSelected(); // ������������� ����� �������� �����
                    _coins1000.SetActive(false); // ��������� ������ 1000 coins
                    _money0_99.SetActive(false); // ��������� ������ 0.99 $
                    _city_btn.SetActive(true); // �������� ������ check City
                    break;
                case 5000:
                    PlayerPrefs.SetString("Megapolis", "Open");
                    PlayerPrefs.SetInt("NowMap", 3); // �������� �������� 3 ����� NowMap
                    GetComponent<CheckMaps>().whichMapSelected(); // ������������� ����� �������� �����
                    _coins5000.SetActive(false); // ��������� ������ 5000 coins
                    _money1_99.SetActive(false); // ��������� ������ 1.99 $
                    _megapolis_btn.SetActive(true); // �������� ������ check Megapolis
                    break;
            }

            int nowCoins = coins - needCoins; // � ����� ��������� ������ ������� �����
            _coinsCount.text = nowCoins.ToString(); // �������� ������ � �������� ����
            PlayerPrefs.SetInt("Coins", nowCoins);
        }
    }
}
