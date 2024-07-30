using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class AudioSpriteToggleManager : MonoBehaviour
{
    public Sprite sprite1; // Assign in the inspector
    public Sprite sprite2; // Assign in the inspector

    private List<AudioSpriteToggle> toggles = new List<AudioSpriteToggle>();
    private bool isSprite1Active = true;
    private bool isAudioMuted = false;
    private float previousVolume;

    void Start()
    {
        previousVolume = AudioListener.volume;
    }

    public void RegisterToggle(AudioSpriteToggle toggle)
    {
        toggles.Add(toggle);
        toggle.UpdateSprite(isSprite1Active ? sprite1 : sprite2);
    }

    public void UnregisterToggle(AudioSpriteToggle toggle)
    {
        toggles.Remove(toggle);
    }

    public void ToggleAudioAndSprites()
    {
        // Toggle audio mute state
        isAudioMuted = !isAudioMuted;
        AudioListener.volume = isAudioMuted ? 0 : previousVolume;

        // Toggle sprite state
        isSprite1Active = !isSprite1Active;

        // Update all registered toggles
        foreach (var toggle in toggles)
        {
            toggle.UpdateSprite(isSprite1Active ? sprite1 : sprite2);
        }
    }
}

// Interface or base class for AudioSpriteToggle

