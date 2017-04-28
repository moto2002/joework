using UnityEngine;
using System.Collections;

/// <summary>
/// 当颜色透明到一定程度，第二次还没按下，重新使startFadingTime = 0
/// 就需要重新按二次才能退出了，在这过程中，第二次按下了，那么程序就退出了。
/// </summary>
public class ClickTwiceToQuitApp : MonoBehaviour
{

    public UnityEngine.UI.Text tipTextBox;
    public string tip = "Click again to quit app";
    public float fadingSpeed = 1;
    private bool fading;
    private float startFadingTime;
    private Color originalColor;
    private Color transparentColor;


    void Start()
    {
        originalColor = tipTextBox.color;
        transparentColor = originalColor;
        transparentColor.a = 0;
        tipTextBox.text = tip;
        tipTextBox.color = transparentColor;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (startFadingTime == 0)
            {
                tipTextBox.color = originalColor;
                startFadingTime = Time.time;
                fading = true;
            }
            else
            {
                Debug.Log("Quit Ok");
                Application.Quit();
            }
        }

        if (fading)
        {
            tipTextBox.color = Color.Lerp(originalColor, transparentColor, (Time.time - startFadingTime) * fadingSpeed);//颜色以线性速度透明掉

            if (tipTextBox.color.a < 2.0 / 255)
            {
                tipTextBox.color = transparentColor;
                startFadingTime = 0;
                fading = false;
            }
        }
    }
}
