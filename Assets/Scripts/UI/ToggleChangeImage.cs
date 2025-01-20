using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ToggleChangeImage : MonoBehaviour
{
    public Toggle toggle; 
    public Image checkmarkImage; 
    public Sprite onSprite; // Sprite khi bật
    public Sprite offSprite; // Sprite khi tắt

    // Start is called before the first frame update
    void Start()
    {
        if (toggle != null && checkmarkImage != null)
        {
            toggle.onValueChanged.AddListener(OnToggleValueChanged);
            // Đặt hình ảnh ban đầu
            UpdateCheckmark(toggle.isOn);
        }

        void OnToggleValueChanged(bool isOn)
        {
            UpdateCheckmark(isOn);
        }

        void UpdateCheckmark(bool isOn)
        {
            // Thay đổi hình ảnh Checkmark dựa vào trạng thái
            checkmarkImage.sprite = isOn ? onSprite : offSprite;
        }
    }
}
