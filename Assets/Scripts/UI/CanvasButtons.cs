using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;


public class CanvasButtons : MonoBehaviour
{
    [SerializeField] private Sprite _btn, _btnPressed; // ���� ��� �������� 

    private Image _image; // ����������

    private void Awake()
    {
        _image = GetComponent<Image>(); // �������� �������� Image � ��������� ��� � ���������� _image
    }

    public void PlayGame()
    {
        StartCoroutine(LoadScene("Game")); // ��� ������� �� ������, ��������� Coroutine(LoadScene)
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
}
