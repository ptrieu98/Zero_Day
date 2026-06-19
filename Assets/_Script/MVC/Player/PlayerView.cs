using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerView : MonoBehaviour
{
    private Rigidbody rb;
    private Camera mainCam;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        mainCam = Camera.main;
    }

    // 1. CHỨC NĂNG LẤY ĐẦU VÀO (Input)
    public Vector2 GetMovementInput()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    public Ray GetMouseRay()
    {
        return mainCam.ScreenPointToRay(Input.mousePosition);
    }

    // 2. CHỨC NĂNG THỰC THI (Output)
    public void ApplyVelocity(Vector3 velocity)
    {
        rb.linearVelocity = velocity;
    }

    public void LookAtPoint(Vector3 point)
{
    // Cân bằng chiều cao Y
    Vector3 flatPoint = new Vector3(point.x, transform.position.y, point.z);
    
    // Chỉ thực hiện xoay nếu khoảng cách giữa chuột và tâm nhân vật lớn hơn 0.1 unit
    // Điều này ngăn lỗi xoay mòng mòng khi rê chuột trúng người nhân vật
    if (Vector3.Distance(transform.position, flatPoint) > 0.1f)
    {
        transform.LookAt(flatPoint);
    }
}
}