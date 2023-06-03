using UnityEngine;

public abstract class EditablePrimitive : MonoBehaviour 
{
    protected bool isValid = true;
    public bool IsValid { get { return isValid; } }
    private int enteredColliderCount;

    public Color color;

    private bool isSelected;
    public bool isMoving;

    public Material validMaterial;
    public Material invalidMaterial;
    public Material selectedMaterial;

    public abstract void PlaceOnSurface(Vector3 point, Vector3 normal, bool keepRotation);
    public abstract void Scale(Vector3 newScale, Axis changedAxis, bool dir);

    public void Select() {
        isSelected = true;
        GetComponent<Collider>().isTrigger = true;
        UpdateMaterial();
    }

    public void Deselect() {
        isSelected = false;
        GetComponent<Collider>().isTrigger = false;
        UpdateMaterial();
    }

    public void Rotate(float dir, float speed) {
        transform.rotation *= Quaternion.Euler(0, speed * dir * Time.deltaTime, 0);
    }

    public void SetColor(Color _color) {
        color = _color;
        validMaterial.SetColor("_ObjectColor", color);
        invalidMaterial.SetColor("_ObjectColor", color);
        selectedMaterial.SetColor("_ObjectColor", color);
    }

    public void SetMaterials(Material validMat, Material invalidMat, Material selectedMat) {
        validMaterial = new Material(validMat);
        invalidMaterial = new Material(invalidMat);
        selectedMaterial = new Material(selectedMat);
    }

    protected void UpdateMaterial() {
        Material activeMaterial;
        if (isSelected) {
            activeMaterial = isValid ? selectedMaterial : invalidMaterial;
        } else {
            activeMaterial = isValid ? validMaterial : invalidMaterial;
        }

        GetComponent<MeshRenderer>().sharedMaterial = activeMaterial;
    }

    public void CopyFrom(EditablePrimitive other) {
        SetMaterials(other.validMaterial, other.invalidMaterial, other.selectedMaterial);
        SetColor(other.color);
    }

    private void OnTriggerEnter(Collider other) {
        if (!isSelected) return;
        if (other.CompareTag("Ignore Validity"))
            return;

        enteredColliderCount++;
        isValid = false;
        UpdateMaterial();
    }

    private void OnTriggerExit(Collider other) {
        if (!isSelected) return;

        if (other.CompareTag("Ignore Validity"))
            return;

        enteredColliderCount--;
        if(enteredColliderCount == 0) {
            isValid = true;
        }
        UpdateMaterial();
    }

}
