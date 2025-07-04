using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkinInfoDisplayer : MonoBehaviour
{
    // Kéo Image bạn muốn hiển thị skin vào đây
    public Image skinDisplayImage;


    void Start()
    {
        int targetSkinId = 0; // ID của skin chúng ta muốn lấy

        // Gọi hàm để tìm và hiển thị thông tin skin
        DisplaySkinInfo(targetSkinId);
    }

    public void DisplaySkinInfo(int id)
    {
        Debug.Log($"--- Bắt đầu tìm kiếm skin có ID: {id} ---");

        // Tìm skin trong SkinData thông qua GameplayManager
        Skin foundSkin = FindSkinInSkinData(id);

        if (foundSkin != null)
        {
            // --- 1. LẤY VÀ HIỂN THỊ ẢNH ---
            if (skinDisplayImage != null)
            {
                skinDisplayImage.sprite = foundSkin.skinSprite;
                Debug.Log($"<color=green>Đã tìm thấy và hiển thị Sprite: {foundSkin.skinSprite.name}</color>");
            }
            else
            {
                Debug.LogWarning("Chưa gán đối tượng Image để hiển thị skin.");
            }

            // --- 2. DEBUG THÔNG TIN RA CONSOLE ---
            string skinDetails = $"Skin ID: {foundSkin.skinID}, Level Unlock: {foundSkin.levelUnlock}";
            Debug.Log($"<color=cyan>Thông tin chi tiết: {skinDetails}</color>");

        }
    }

    // Hàm phụ để tìm kiếm skin trong SkinData
    private Skin FindSkinInSkinData(int id)
    {
        SkinData skinData = GameplayManager.Instance.skinData;

        // Tìm trong cả 3 danh sách
        foreach (var skin in skinData.premiumSkin)
        {
            if (skin.skinID == id) return skin;
        }
        foreach (var skin in skinData.coinSkin)
        {
            if (skin.skinID == id) return skin;
        }
        foreach (var skin in skinData.rescueSkin)
        {
            if (skin.skinID == id) return skin;
        }

        return null; // Trả về null nếu không tìm thấy
    }
}