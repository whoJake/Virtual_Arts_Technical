using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairBehaviour : MonoBehaviour
{
    private void Update() {
        GetComponent<Image>().enabled = Cursor.lockState == CursorLockMode.Locked;
    }
}
