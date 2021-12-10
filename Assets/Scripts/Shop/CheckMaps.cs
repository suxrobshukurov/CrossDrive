using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckMaps : MonoBehaviour
{
    [SerializeField] public Image[] maps; // создаем массив карт 
    [SerializeField] private Sprite _selected, _notSelected;
    [NonSerialized] private BuyMapCoins _mapCoins;

    private void Start()
    {
        whichMapSelected();
        _mapCoins = GetComponent<BuyMapCoins>();
        // проверяем карты на доспуность для пользователя.
        if (PlayerPrefs.GetString("City") == "Open")
        {
            _mapCoins._coins1000.SetActive(false);
            _mapCoins._money0_99.SetActive(false);
            _mapCoins._city_btn.SetActive(true);
        }
        if(PlayerPrefs.GetString("Megapolis") == "Open")
        {
            _mapCoins._coins5000.SetActive(false);
            _mapCoins._money1_99.SetActive(false);
            _mapCoins._megapolis_btn.SetActive(true);
        }
    }

    // выбор карту 
    public void whichMapSelected()
    {
        switch (PlayerPrefs.GetInt("NowMap"))
        {
            case 2:
                maps[0].sprite = _notSelected; // устанавливаем крестик
                maps[1].sprite = _selected; // устанавливаем галочку
                maps[2].sprite = _notSelected; // устанавливаем крестик
                break;
            case 3:
                maps[0].sprite = _notSelected; // устанавливаем крестик
                maps[1].sprite = _notSelected; // устанавливаем крестик
                maps[2].sprite = _selected; // устанавливаем галочку
                break;
            default:
                maps[0].sprite = _selected; // устанавливаем галочку
                maps[1].sprite = _notSelected; // устанавливаем крестик
                maps[2].sprite = _notSelected; // устанавливаем крестик
                break;
        }
    }
}
