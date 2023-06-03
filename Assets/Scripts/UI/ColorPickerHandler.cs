using UnityEngine;
using UnityEngine.UI;

public class ColorPickerHandler : MonoBehaviour
{
    [SerializeField]    private RectTransform pickerRect;
    [SerializeField]    private UserEditingController user;
    [SerializeField]    private Texture2D textureReference;
    [SerializeField]    private GameObject pickerFoldout;
    [SerializeField]    private Image colourPreview;

    private Rect pickerBounds;
    private bool isExtended;

    private void Start() {
        //I was having a lot of trouble with finding these world space corners so thankyou https://forum.unity.com/threads/solved-getting-position-and-size-of-recttransform-in-screen-coordinates.816888/
        Vector3[] corners = new Vector3[4];
        pickerRect.GetWorldCorners(corners);
        pickerBounds = new Rect();
        pickerBounds.xMin = corners[0].x;
        pickerBounds.yMin = corners[0].y;
        pickerBounds.xMax = corners[2].x;
        pickerBounds.yMax = corners[2].y;

        SetExtended(false);
    }

    private void Update() {
        if (Cursor.lockState == CursorLockMode.Locked) {
            SetExtended(false);
            return;
        }
        if (!isExtended) return;

        Vector2 mousePosition = (Vector2)Input.mousePosition;

        //If clicked outside bounds
        if (Input.GetMouseButtonDown(0)) {
            if (mousePosition.x < pickerBounds.xMin - 5 || mousePosition.x > pickerBounds.xMax + 5 ||
                mousePosition.y < pickerBounds.yMin - 5 || mousePosition.y > pickerBounds.yMax + 5) 
            {
                SetExtended(false);
                return;
            }
        }

        Vector2 uv = GetUV(mousePosition);
        //Mouse held
        if (Input.GetMouseButton(0)) {
            Vector2Int sample = UVToTextureCoordinate(uv);
            Color pixelColor = textureReference.GetPixel(sample.x, sample.y);
            SetColor(pixelColor);
        }
    }

    public void ToggleExtended() {
        SetExtended(!isExtended);
    }

    public void SetExtended(bool val) {
        isExtended = val;
        pickerFoldout.SetActive(isExtended);
        GetComponentInChildren<Button>().enabled = !isExtended;
    }

    public void SetColor(Color c) {
        user.ChangeColor(c);
        colourPreview.color = c;
    }

    private Vector2 GetUV(Vector2 mousePosition) {
        float u = Mathf.InverseLerp(pickerBounds.xMin, pickerBounds.xMax, mousePosition.x);
        float v = Mathf.InverseLerp(pickerBounds.yMin, pickerBounds.yMax, mousePosition.y);
        return new Vector2(u, v);
    }

    private Vector2Int UVToTextureCoordinate(Vector2 uv) {
        Vector2Int result = new Vector2Int();
        result.x = Mathf.RoundToInt(uv.x * textureReference.width);
        result.y = Mathf.RoundToInt(uv.y * textureReference.height);
        return result;
    }
}
