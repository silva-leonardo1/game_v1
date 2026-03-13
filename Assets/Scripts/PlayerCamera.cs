using UnityEngine;

public class PlayerCamera: MonoBehaviour
{
    [Header("Sens")]
    public float sensX, sensY;

    [Header("Reference")]
    public Transform orient, holder;
    private float rotationX, rotationY;
    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void Update()
    {
        float horizontal = Mathf.Clamp(Input.GetAxisRaw("Mouse X") * sensX, -10f, 10f);
        float vertical = Mathf.Clamp(Input.GetAxisRaw("Mouse Y") * sensY, -10f, 10f);
        rotationX -= vertical;
        rotationY += horizontal;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);
        orient.rotation = Quaternion.Euler(0, rotationY, 0);
        holder.rotation = Quaternion.Euler(rotationX, rotationY, 0);
    }
}