using UnityEngine;

[RequireComponent(typeof(PlayerView))]
public class PlayerController : MonoBehaviour
{
    public PlayerModel model = new PlayerModel(); 
    private PlayerView view;

    void Start()
    {
        view = GetComponent<PlayerView>();
        model.InitializeStats();
    }

    void Update()
    {
        HandleAiming();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector2 input = view.GetMovementInput();
        Vector3 moveDirection = new Vector3(input.x, 0f, input.y).normalized;

        // 1. Tính toán tốc độ thực tế bị ảnh hưởng bởi trọng lượng
        float actualSpeed = CalculateEncumbranceSpeed();

        // 2. Áp dụng tốc độ vào Model và truyền xuống View
        model.currentVelocity = moveDirection * actualSpeed;
        view.ApplyVelocity(model.currentVelocity);
    }

    // Hàm toán học xử lý Hệ thống Mang vác
    private float CalculateEncumbranceSpeed()
    {
        float safeCapacity = model.SafeCarryCapacity;

        // Trạng thái 1: Nhẹ nhàng (Weight <= SafeCapacity)
        if (model.currentWeight <= safeCapacity)
        {
            return model.baseMoveSpeed; // Chạy 100% tốc độ
        }

        // Tính toán phần trọng lượng bị dư ra
        float overweightAmount = model.currentWeight - safeCapacity;

        // Trạng thái 2: Quá nặng (Vượt qua giới hạn chịu đựng)
        if (overweightAmount >= model.maxOverweightLimit)
        {
            return 0f; // Bị đè bẹp, không thể di chuyển
        }

        // Trạng thái 3: Di chuyển chậm (Bị trừ tỷ lệ phần trăm)
        // Ví dụ: Dư 2.5kg trên tổng 5kg giới hạn -> Penalty 50% tốc độ
        float penaltyRatio = overweightAmount / model.maxOverweightLimit;
        
        return model.baseMoveSpeed * (1f - penaltyRatio);
    }

    private void HandleAiming()
    {
        Ray ray = view.GetMouseRay();
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        
        if (groundPlane.Raycast(ray, out float rayDistance))
        {
            model.currentLookTarget = ray.GetPoint(rayDistance);
            view.LookAtPoint(model.currentLookTarget);
        }
    }
}