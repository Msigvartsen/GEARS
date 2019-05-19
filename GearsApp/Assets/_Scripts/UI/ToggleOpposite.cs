using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class ToggleOpposite.
/// Disable button if toggle is off
/// Implements the <see cref="UnityEngine.MonoBehaviour" />
/// </summary>
/// <seealso cref="UnityEngine.MonoBehaviour" />
public class ToggleOpposite : MonoBehaviour
{
    [SerializeField]
    private Toggle togglebutton;

    private void Start()
    {
        FlipToggle();
    }

    public void FlipToggle()
    {
        if (togglebutton.isOn)
        {
            GetComponent<Button>().interactable = false;
            GetComponent<CanvasGroup>().alpha = 0.2f;
        }
        else
        {
            GetComponent<Button>().interactable = true;
            GetComponent<CanvasGroup>().alpha = 1f;
        }

    }
}
