using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;


public class CanvasButtons : MonoBehaviour
{
    [SerializeField] private Sprite _btn, _btnPressed; // поля для спрайтов 

    private Image _image; // переменная

    private void Awake()
    {
        _image = GetComponent<Image>(); // получаем компонет Image и сохраняем его в переменной _image
    }

    public void PlayGame()
    {
        StartCoroutine(LoadScene("Game")); // при нажатии на кнопку, запускаем Coroutine(LoadScene)
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
}
