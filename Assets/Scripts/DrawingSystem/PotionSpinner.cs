using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DrawingSystem
{
    public class PotionSpinner : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private TextMeshProUGUI textMeshProUGUI;
        [SerializeField] private Slider slider;

        private void Awake()
        {
            slider.onValueChanged.AddListener(val =>
            {
                speed = val;
                textMeshProUGUI.text = "Spin Speed: " + speed.ToString("F1");
            });

            slider.value = 30;
        }

        // Update is called once per frame
        void LateUpdate()
        {
            transform.Rotate( Vector3.down * (speed * Time.deltaTime), Space.Self);
        }

    }
}
