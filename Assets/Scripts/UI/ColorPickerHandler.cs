using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ColorPickerHandler : MonoBehaviour
{
    [SerializeField]
    private RectTransform boundsReference;
    [SerializeField]
    private UserEditingController user;
    [SerializeField]
    private Texture2D texReference;
    [SerializeField]
    private GameObject pickerExtension;

    [SerializeField]
    private Image colourDisplay;

    private Rect bounds;

    private bool extended;

    private void Start() {
        //I was having a lot of trouble with finding these world space corners so thankyou https://forum.unity.com/threads/solved-getting-position-and-size-of-recttransform-in-screen-coordinates.816888/
        Vector3[] corners = new Vector3[4];
        boundsReference.GetWorldCorners(corners);
        bounds = new Rect();
        bounds.xMin = corners[0].x;
        bounds.yMin = corners[0].y;
        bounds.xMax = corners[2].x;
        bounds.yMax = corners[2].y;

        SetExtended(false);
    }

    private Vector2 GetUV(Vector2 mousePosition) {
        float u = Mathf.InverseLerp(bounds.xMin, bounds.xMax, mousePosition.x);
        float v = Mathf.InverseLerp(bounds.yMin, bounds.yMax, mousePosition.y);
        return new Vector2(u, v);
    }

    private Vector2Int UVToTextureCoordinate(Vector2 uv) {
        Vector2Int result = new Vector2Int();
        result.x = Mathf.RoundToInt(uv.x * texReference.width);
        result.y = Mathf.RoundToInt(uv.y * texReference.height);
        return result;
    }

    public void ToggleExtended() {
        SetExtended(!extended);
    }

    public void SetExtended(bool val) {
        extended = val;
        pickerExtension.SetActive(extended);
        GetComponentInChildren<Button>().enabled = !extended;
    }

    public void SetColor(Color c) {
        user.ChangeColor(c);
        colourDisplay.color = c;
    }

    private void Update() {
        if(Cursor.lockState == CursorLockMode.Locked) {
            SetExtended(false);
            return;
        }
        if (!extended) return;

        Vector2 mousePosition = (Vector2)Input.mousePosition;

        //If clicked outside bounds
        if (Input.GetMouseButtonDown(0)) {
            if (mousePosition.x < bounds.xMin - 5 || mousePosition.x > bounds.xMax + 5 || mousePosition.y < bounds.yMin - 5 || mousePosition.y > bounds.yMax + 5) {
                SetExtended(false);
                return;
            }
        }

        Vector2 uv = GetUV(mousePosition);
        //Mouse held
        if (Input.GetMouseButton(0)) {
            Vector2Int sample = UVToTextureCoordinate(uv);
            Color pixelColor = texReference.GetPixel(sample.x, sample.y);
            SetColor(pixelColor);
        }
    }
}
