using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ChangePanel
/// Simple class that turns swaps two panels. Run script on a button/via script
/// and set newPanel equal the panel you want to show, and previousPanel to the panel you want to hide.
/// </summary>
public class ChangePanel : MonoBehaviour
{
    [SerializeField]
    private GameObject newPanel;
    [SerializeField]
    private GameObject previousPanel;

    /// <summary>
    /// Swaps the two panels Active State.
    /// </summary>
    public void SetNewActivePanel()
    {
        newPanel.SetActive(true);
        previousPanel.SetActive(false);
    }
}

