using UnityEngine;

public class ControlsPopupUI : MonoBehaviour
{
    public GameObject popup;

    public void ClosePopup()
    {
        popup.SetActive(false);
    }


}
