using System;
using UnityEngine;

public class IsNoAds : MonoBehaviour {
    private void Start() {

        //PlayerPrefs.DeleteKey("NoAds");
        //PlayerPrefs.DeleteKey("City");
        //PlayerPrefs.DeleteKey("Megapolis");
        //PlayerPrefs.SetInt("Coins", 135);

        if (PlayerPrefs.GetString("NoAds") == "yes")
            Destroy(gameObject);
    }
    
}
