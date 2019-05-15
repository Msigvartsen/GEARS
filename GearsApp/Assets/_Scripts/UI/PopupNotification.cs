using UnityEngine;

/// <summary>
/// Script to handle popup notification.
/// </summary>
public class PopupNotification : MonoBehaviour
{
    /// <summary>
    /// Display the popup with current text, before fading out after a delay.
    /// </summary>
    /// <param name="text">Text to display in popup.</param>
    /// <param name="delay">Delay before fading out.</param>
    public void ShowPopup(string text, float delay = 4)
    {
        GetComponent<Animator>().Play("Fade-in");
        GetComponentInChildren<TMPro.TextMeshProUGUI>().text = text;
        Invoke("FadeOutDelay", delay);
    }

    /// <summary>
    /// Fade out the notification panel.
    /// </summary>
    private void FadeOutDelay()
    {
        GetComponent<Animator>().Play("Fade-out");
    }
}
