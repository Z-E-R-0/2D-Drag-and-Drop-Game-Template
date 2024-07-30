using UnityEngine;
using UnityEngine.UI;

public class AudioSpriteToggle : MonoBehaviour, IAudioSpriteToggle
{
    public Button toggleButton; // Assign the button in the inspector
    private AudioSpriteToggleManager manager;

    void Start()
    {
        // Get the manager component
        manager = FindObjectOfType<AudioSpriteToggleManager>();

        // Register this toggle with the manager
        if (manager != null)
        {
            manager.RegisterToggle(this);
        }

        // Add listener to the button
        if (toggleButton != null)
        {
            toggleButton.onClick.AddListener(OnButtonClick);
        }
    }

    void OnDestroy()
    {
        // Unregister this toggle from the manager when destroyed
        if (manager != null)
        {
            manager.UnregisterToggle(this);
        }
    }

    private void OnButtonClick()
    {
        if (manager != null)
        {
            manager.ToggleAudioAndSprites();
        }
    }

    public void UpdateSprite(Sprite newSprite)
    {
        var imageComponent = GetComponent<Image>();
        if (imageComponent != null)
        {
            imageComponent.sprite = newSprite;
        }
    }
}
