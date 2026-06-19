using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Mục tiêu")]
    public Transform player;

    [Header("Góc nhìn & Vị trí (Duckov Style)")]
    [Tooltip("Độ cao camera - Cần đặt cao kết hợp với FOV thấp")]
    public float cameraHeight = 18f; 
    
    [Tooltip("Độ lùi camera để bù trừ cho góc nghiêng")]
    public float cameraOffsetZ = -10f; 
    
    [Tooltip("Góc nghiêng 60-65 độ giúp thấy rõ không gian 3D hơn")]
    public float cameraAngleX = 60f; 

    [Header("Cơ chế Rướn (Look-ahead)")]
    public float smoothSpeed = 10f;
    [Range(0f, 1f)] public float lookAheadFactor = 0.25f;
    
    [Tooltip("Giới hạn khoảng cách camera được phép rướn xa khỏi nhân vật")]
    public float maxLookDistance = 6f; 

    private Camera mainCam;

    void Start()
    {
        mainCam = GetComponent<Camera>();
        
        // Ép FOV hẹp lại để giảm méo hình, tạo cảm giác chiến thuật
        mainCam.fieldOfView = 40f; 
    }

    void FixedUpdate()
    {
        if (player == null) return;

        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        
        if (groundPlane.Raycast(ray, out float rayDistance))
        {
            Vector3 mouseWorldPosition = ray.GetPoint(rayDistance);
            
            // 1. Tính toán khoảng cách rướn từ Nhân vật tới Chuột
            Vector3 lookOffset = (mouseWorldPosition - player.position) * lookAheadFactor;
            
            // 2. GIỚI HẠN (CLAMP): Đảm bảo camera không bao giờ chạy quá xa nhân vật
            lookOffset = Vector3.ClampMagnitude(lookOffset, maxLookDistance);

            // 3. Cộng dồn vị trí nhân vật + khoảng rướn + độ cao + độ lùi
            Vector3 targetPosition = player.position + lookOffset;
            targetPosition.y += cameraHeight;
            targetPosition.z += cameraOffsetZ;

            // 4. Nội suy mượt mà
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        }
        
        // Giữ cố định góc xoay
        transform.rotation = Quaternion.Euler(cameraAngleX, 0f, 0f);
    }
}