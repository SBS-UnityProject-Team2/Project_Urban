using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class ScriptUI : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private TMP_Text textArea;

    [Header("Text Print Settings")]
    [SerializeField] private float typingSpeed = 0.03f;
    bool isSkip = false;

    private void Update()
    {
        if (!isSkip && IsAdvanceInputPressed())
            isSkip = true;
    }

    public void Init()
    {
        isSkip = false;
        textArea.text = string.Empty;
    }

    public async UniTask PrintScript(string[] scripts)
    {
        isSkip = false;
        int length = scripts.Length;

        for (int i = 0; i < length; i++)
        {
            await PrintScript(scripts[i]);

            if (i < length - 1)
            {
                await UniTask.WaitUntil(IsAdvanceInputPressed);
                textArea.text = string.Empty;
                await UniTask.Yield();
            }
        }
    }

    public async UniTask PrintScript(string script)
    {
        isSkip = false;

        char[] charArray = script.ToCharArray();
        int charCount = script.Length;

        int idx = 0;
        while (idx < charCount)
        {
            if (isSkip)
            {
                textArea.SetCharArray(charArray, 0, charCount);

                break;
            }

            idx = GetNextIndex(charArray, idx);
            textArea.SetCharArray(charArray, 0, idx);

            await UniTask.WaitForSeconds(typingSpeed);
        }

        await UniTask.Yield();
    }

    static private bool IsAdvanceInputPressed()
    {
        return Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space);
    }

    static async public UniTask WaitClick()
    {
        await UniTask.WaitUntil(IsAdvanceInputPressed);
        await UniTask.Yield();
    }

    private int GetNextIndex(char[] charArray, int startIdx)
    {
        if (charArray[startIdx] == '<')
        {
            for (int i = startIdx + 1; i < charArray.Length; i++)
                if (charArray[i] == '>') return i + 1;
        }

        return startIdx + 1;
    }
}