using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;

namespace UnityEngine.UI {
    public class SoundSlider : MonoBehaviour
    {
        public Slider sound;
        public TextMeshProUGUI soundPercent;
        // Start is called before the first frame update
        void Start()
        {
            sound.minValue = 0;
            sound.maxValue = 1;
            sound.wholeNumbers = false;
            sound.value = 1;
            
            // Add listener for value changes
            sound.onValueChanged.AddListener(OnVolumeChanged);
        }

        void OnVolumeChanged(float value)
        {
            soundPercent.text = ((int)(sound.value*100)) + "%";
            // Convert slider value (0-100) to volume range (0-1)
            float volume = value;
            PersistantGameManager.Instance.SetMusicVolume(volume);
        }

        // Update is called once per frame
        void Update()
        {
            soundPercent.text = ((int)(sound.value*100)) + "%";
        }
    }
}

