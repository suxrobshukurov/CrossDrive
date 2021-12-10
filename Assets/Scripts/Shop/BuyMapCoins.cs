using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyMapCoins : MonoBehaviour
{
    // поля для кнопок в магазине 
    [SerializeField] public GameObject _coins1000, _coins5000, _money0_99, _money1_99, _city_btn, _megapolis_btn;
    // поле для анимации
    [SerializeField] private Animation _cointsText;
    // поле вывода счета 
    [SerializeField] private Text _coinsCount;


    //метод для покупки карт
    public void BuyNewMap(int needCoins)
    {
        int coins = PlayerPrefs.GetInt("Coins");
        if (coins < needCoins)
        {
            _cointsText.Play(); // пригривание анимации если нет денег
        }
        else
        {
            //Покупка карты 
            switch (needCoins)
            {
                case 1000:
                    PlayerPrefs.SetString("City", "Open"); // открываем карту с нужным ключом
                    PlayerPrefs.SetInt("NowMap", 2); // передаем значение 2 ключу NowMap
                    GetComponent<CheckMaps>().whichMapSelected(); // устанавливаем карту выбраную карту
                    _coins1000.SetActive(false); // выключить кнопку 1000 coins
                    _money0_99.SetActive(false); // выключить кнопку 0.99 $
                    _city_btn.SetActive(true); // включаем кнопку check City
                    break;
                case 5000:
                    PlayerPrefs.SetString("Megapolis", "Open");
                    PlayerPrefs.SetInt("NowMap", 3); // передаем значение 3 ключу NowMap
                    GetComponent<CheckMaps>().whichMapSelected(); // устанавливаем карту выбраную карту
                    _coins5000.SetActive(false); // выключить кнопку 5000 coins
                    _money1_99.SetActive(false); // выключить кнопку 1.99 $
                    _megapolis_btn.SetActive(true); // включаем кнопку check Megapolis
                    break;
            }

            int nowCoins = coins - needCoins; // в новой переменой храним остаток суммы
            _coinsCount.text = nowCoins.ToString(); // передаем статус в указаное поле
            PlayerPrefs.SetInt("Coins", nowCoins);
        }
    }
}
