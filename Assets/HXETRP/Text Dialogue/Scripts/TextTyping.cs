using UnityEngine;
using TMPro;
using System.Collections;
using System.Text.RegularExpressions;

public class TextTyping : MonoBehaviour
{
    public TextMeshProUGUI textUI;
    [TextArea] public string[] scripts;

    private int nowText = -1;
    private int lastText = 0;

    private bool isTyping = false;
    private bool skipTyping = false;

    private void Start()
    {
        lastText = scripts.Length;
        StartTyping();
    }

    public void StartTyping()
    {
        if (!isTyping && nowText + 1 < lastText)
        {
            nowText++;
            StartCoroutine(Typing(scripts[nowText]));
        }
    }
            

    private void Update()
    {
        // ���콺 Ŭ�� �� Ÿ���� ��ŵ ��û
        if (isTyping && Input.GetMouseButtonDown(0))
        {
            skipTyping = true;
        }
        // Ÿ������ ���� �� Ŭ�� �� ���� ���� ���
        else if (!isTyping && Input.GetMouseButtonDown(0))
        {
            StartTyping();
        }
    }

    IEnumerator Typing(string rawText)
    {
        isTyping = true;
        skipTyping = false;

        // TMP �±� ���� (ex. <color=#FF0000>)
        string displayText = Regex.Replace(rawText, "<.*?>", "");
        textUI.text = "";

        for (int i = 0; i < displayText.Length; i++)
        {
            if (skipTyping)
            {
                textUI.text = displayText;
                break;
            }

            textUI.text += displayText[i];
            yield return new WaitForSeconds(0.05f); // ���ڴ� ���� �ð�
        }

        isTyping = false;
    }
}
