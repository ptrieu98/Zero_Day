using UnityEngine;

[System.Serializable]
public class PlayerModel
{
    [Header("Chỉ số Chiến đấu & Tốc độ")]
    public float baseMoveSpeed = 5f;
    public Vector3 currentVelocity;
    public Vector3 currentLookTarget;

    [Header("Chỉ số Sinh tồn cơ bản")]
    public float maxHealth = 100f;
    public float currentHealth;
    public float maxRadiation = 100f;
    public float currentRadiation;

    [Header("Hệ thống Mang vác (Encumbrance)")]
    [Tooltip("Thể lực (Sức mạnh cơ bắp). VD: 15")]
    public float stamina = 15f; 
    
    [Tooltip("Tổng trọng lượng trang bị hiện tại trên người (kg)")]
    public float currentWeight = 0f;

    [Tooltip("Hệ số quy đổi: 1 Thể lực = bao nhiêu kg an toàn?")]
    public float weightPerStamina = 0.2f;

    [Tooltip("Vượt quá bao nhiêu kg so với mức an toàn thì tốc độ về 0?")]
    public float maxOverweightLimit = 5f;

    // Hàm tự động tính ra số kg vác an toàn (Read-only)
    public float SafeCarryCapacity => stamina * weightPerStamina;

    public void InitializeStats()
    {
        currentHealth = maxHealth;
        currentRadiation = 0f;
        // Thể lực là chỉ số cứng, không cần gán lại ở đây
    }
}