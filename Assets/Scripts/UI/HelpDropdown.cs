using UnityEngine;

public class HelpDropdown : MonoBehaviour
{
    [SerializeField]    GameObject dropdown;

    public void Start() {
        dropdown.SetActive(false);
    }

    public void Toggle() {
        dropdown.SetActive(!dropdown.activeSelf);
    }
}
