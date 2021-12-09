using UnityEngine;

public class Fading : MonoBehaviour
{
    [SerializeField] private Texture2D _fading;

    private float _fadeSpeed = 0.8f, _alpha = 1f, _fadeDir = -1; // скорость анимации, прозрачность
    private int _drawDepth = -1000; // слой

    private void OnGUI()
    {
        _alpha += _fadeDir * _fadeSpeed * Time.deltaTime; 
        _alpha = Mathf.Clamp01(_alpha); // округляем прозрачность

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, _alpha); // отрисовка
        GUI.depth = _drawDepth;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), _fading);
    }

    public float Fade(float dir)
    {
        _fadeDir = dir;
        return _fadeSpeed;
    }

}
