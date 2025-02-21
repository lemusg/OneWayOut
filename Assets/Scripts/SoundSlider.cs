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
            sound.maxValue = 100;
            sound.wholeNumbers = true;
            sound.value = 100;
        }

        // Update is called once per frame
        void Update()
        {
            soundPercent.text = sound.value + "%";
        }
    }
}