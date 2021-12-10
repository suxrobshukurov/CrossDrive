using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;


public class CanvasButtons : MonoBehaviour
{
    [SerializeField] private Sprite _btn, _btnPressed, musicOn, musicOff; // поля для спрайтов 

    private Image _image; // переменная

    private void Start()
    {
        _image = GetComponent<Image>(); // получаем компонет Image и сохраняем его в переменной _image
        if (gameObject.name == "Music Button")
        {
            if (PlayerPrefs.GetString("music") == "No")
                transform.GetChild(0).GetComponent<Image>().sprite = musicOff;
        }
    }
    public void MusicButton()
    {
        if (PlayerPrefs.GetString("music") == "No")
        {
            // включение музыки
            PlayerPrefs.SetString("music", "Yes");
            transform.GetChild(0).GetComponent<Image>().sprite = musicOn;
        }
        else
        {
            // выключение музыки
            PlayerPrefs.SetString("music", "No");
            transform.GetChild(0).GetComponent<Image>().sprite = musicOff;
        }
        PlayButtonSound();
    }

    public void ShopScene()
    {
        StartCoroutine(LoadScene("Shop"));
        PlayButtonSound();
    }
    public void ExitShopScene()
    {
        StartCoroutine(LoadScene("Main"));
        PlayButtonSound();
    }

    public void PlayGame()
    {
        if (PlayerPrefs.GetString("First Game") == "No")
        {
            StartCoroutine(LoadScene("Game")); // при нажатии на кнопку, запускаем Coroutine(LoadScene)
        }
        else
        {

            StartCoroutine(LoadScene("Study")); 
        }
        PlayButtonSound();
    }
    
    public void RestartGames()
    {
        StartCoroutine(LoadScene("Game"));
        PlayButtonSound();
    }

    public void SetPressButton()
    {
        _image.sprite = _btnPressed; // устанавливем спрайт для кнопке при вызове метода SetPressButton()
        transform.GetChild(0).localPosition -= new Vector3(0, 5f, 0); // при работе с дочерними элемнетами обращаться через localPosition и меняем расположение
    }
    public void SetDefаultButton()
    {
        _image.sprite = _btn; // устанавливем спрайт для кнопке при вызове метода SetDefаultButton()
        transform.GetChild(0).localPosition += new Vector3(0, 5f, 0);
    }

    IEnumerator LoadScene(string name)
    {
        float fadeTime = Camera.main.GetComponent<Fading>().Fade(1f); // обращаемся к сккрипту Fading.cs к методу Fade и указываем _fadeDir
        yield return new WaitForSeconds(fadeTime); // скорость анимации мы получили тоже из Fading.сs из метода Fade
        SceneManager.LoadScene(name); // уже переходим на новую сцену
    }

    // 
    private void PlayButtonSound()
    {
        if (PlayerPrefs.GetString("music") != "No") // если не выключин звук
        {
            GetComponent<AudioSource>().Play(); // мы запуксаем звуковой эффект
        }
    }
}
