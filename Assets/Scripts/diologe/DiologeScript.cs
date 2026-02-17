using UnityEngine;
using TMPro;
using System.Collections;

public class DiologeScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private float typewriterSpeed = 0.05f; // Speed of the typewriter effect

    private Coroutine typewriterCoroutine;

    private void Awake()
    {
        if (textMeshPro == null)
        {
            textMeshPro = GetComponent<TextMeshProUGUI>();
        }
    }

    public void SetTextTypewriter(string text)
    {
        if (typewriterCoroutine != null)
        {
            StopCoroutine(typewriterCoroutine);
        }
        typewriterCoroutine = StartCoroutine(TypeText(text));
    }
    public void ClearText()
    {
        if (typewriterCoroutine != null)
        {
            StopCoroutine(typewriterCoroutine);
        }
        textMeshPro.text = string.Empty;
    }

    private IEnumerator TypeText(string text)
    {
        textMeshPro.text = string.Empty;
        foreach (char letter in text)
        {
            textMeshPro.text += letter;
            yield return new WaitForSeconds(typewriterSpeed);
        }
    }
}