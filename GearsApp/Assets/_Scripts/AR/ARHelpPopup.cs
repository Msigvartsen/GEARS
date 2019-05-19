using UnityEngine;

/// <summary>
/// Class ARHelpPopup.
/// Fade in Popup panel
/// Implements the <see cref="UnityEngine.MonoBehaviour" />
/// </summary>
/// <seealso cref="UnityEngine.MonoBehaviour" />
public class ARHelpPopup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Animator>().Play("Fade-in");
    }
}
