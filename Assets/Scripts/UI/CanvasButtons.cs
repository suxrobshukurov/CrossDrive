using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;


public class CanvasButtons : MonoBehaviour
{
    [SerializeField] private Sprite _btn, _btnPressed, musicOn, musicOff; // ���� ��� �������� 

    private Image _image; // ����������

    private void Start()
    {
        _image = GetComponent<Image>(); // �������� �������� Image � ��������� ��� � ���������� _image
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
            // ��������� ������
            PlayerPrefs.SetString("music", "Yes");
            transform.GetChild(0).GetComponent<Image>().sprite = musicOn;
        }
        else
        {
            // ���������� ������
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
            StartCoroutine(LoadScene("Game")); // ��� ������� �� ������, ��������� Coroutine(LoadScene)
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
        _image.sprite = _btnPressed; // ������������ ������ ��� ������ ��� ������ ������ SetPressButton()
        transform.GetChild(0).localPosition -= new Vector3(0, 5f, 0); // ��� ������ � ��������� ���������� ���������� ����� localPosition � ������ ������������
    }
    public void SetDef�ultButton()
    {
        _image.sprite = _btn; // ������������ ������ ��� ������ ��� ������ ������ SetDef�ultButton()
        transform.GetChild(0).localPosition += new Vector3(0, 5f, 0);
    }

    IEnumerator LoadScene(string name)
    {
        float fadeTime = Camera.main.GetComponent<Fading>().Fade(1f); // ���������� � �������� Fading.cs � ������ Fade � ��������� _fadeDir
        yield return new WaitForSeconds(fadeTime); // �������� �������� �� �������� ���� �� Fading.�s �� ������ Fade
        SceneManager.LoadScene(name); // ��� ��������� �� ����� �����
    }

    // 
    private void PlayButtonSound()
    {
        if (PlayerPrefs.GetString("music") != "No") // ���� �� �������� ����
        {
            GetComponent<AudioSource>().Play(); // �� ��������� �������� ������
        }
    }
}
