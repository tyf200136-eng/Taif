using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    // هذه الخانة التي تبحث عنها
    public Sprite openDoorSprite; 

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        // الحصول على مكون الـ SpriteRenderer الموجود على الباب
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.instance != null)
            {
                // التحقق من أن السكور كافٍ للفوز
                if (GameManager.instance.totalScore >= GameManager.instance.starsToWin)
                {
                    // تغيير شكل الباب إلى المفتوح
                    if (openDoorSprite != null && spriteRenderer != null)
                    {
                        spriteRenderer.sprite = openDoorSprite;
                    }

                    // استدعاء شاشة الفوز
                    GameManager.instance.WinGame();
                }
                else
                {
                    Debug.Log("الباب مغلق! اجمع المزيد من النجوم.");
                }
            }
        }
    }
}