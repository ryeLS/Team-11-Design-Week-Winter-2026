using UnityEngine;

public class ControlsPopupUI : MonoBehaviour
{
    public GameObject popup;

    void start()
    {
        Time.timeScale = 0f;
    }
    public void ClosePopup()
    {
        popup.SetActive(false);
        Time.timeScale = 1f;
    }


}
