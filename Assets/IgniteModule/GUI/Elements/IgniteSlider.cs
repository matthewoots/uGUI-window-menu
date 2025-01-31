using UnityEngine;
using UnityEngine.UI;
using IgniteModule.GUICore;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;
using System.Collections;

namespace IgniteModule
{
    public class IgniteSlider : IgniteGUIElement, IPointerClickHandler
    {
        [SerializeField] Slider slider = null;
        [SerializeField] RectTransform handleAreaRect = null;
        [SerializeField] Image backgroundImage = null;
        [SerializeField] Image handleImage = null;
        [SerializeField] Text sliderValueText = null;
        [SerializeField] HorizontalLayoutGroup horizontalLayoutGroup = null;

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            onSelected.Invoke();
        }

        public void SetSize(float width, float height)
        {
            horizontalLayoutGroup.padding = new RectOffset(0, (int)height, 0, 0);
            this.RectTransform.SetSizeDelta(width, height);
            handleAreaRect.SetSizeDelta(y: height);
            handleImage.rectTransform.SetSizeDelta(x: height);
        }

        public static IgniteSlider Create(Action<float> onValueChanged, float minValue = 0f, float maxValue = 1f, bool wholeNumbers = false, UnityEvent<float> valueChangeEvent = null, float initialValue = 0f)
        {
            var instance = Instantiate(Resources.Load<GameObject>("IgniteGUI/Slider")).GetComponent<IgniteSlider>();

            instance.SetSize(IgniteGUISettings.ElementWidth, IgniteGUISettings.ElementHeight);
            instance.backgroundImage.color = IgniteGUISettings.SliderBackgroundColor;
            instance.handleImage.color = IgniteGUISettings.SliderHandleColor;
            instance.sliderValueText.font = IgniteGUISettings.Font;
            instance.sliderValueText.fontSize = IgniteGUISettings.FontSize;
            instance.sliderValueText.resizeTextMaxSize = IgniteGUISettings.FontSize;
            instance.sliderValueText.color = IgniteGUISettings.FontColor;
            instance.slider.minValue = minValue;
            instance.slider.maxValue = maxValue;
            instance.slider.wholeNumbers = wholeNumbers;
            instance.slider.onValueChanged.AddListener(new UnityAction<float>(onValueChanged));
            instance.slider.onValueChanged.AddListener(v => instance.sliderValueText.text = v.ToString("F"));
            instance.slider.value = initialValue;

            if (valueChangeEvent != null)
            {
                valueChangeEvent.AddListener(v =>
                {
                    if (instance != null)
                    {
                        instance.slider.value = v;
                    }
                });
            }

            return instance;
        }

        public class ValueChangeEvent : UnityEvent<float>
        {
        }
    }

    public static partial class IIgniteGUIGroupExtensions
    {
        // slider
        public static IIgniteGUIGroup AddSlider(this IIgniteGUIGroup group, Action<float> onValueChanged, float minValue = 0f, float maxValue = 1f, bool wholeNumbers = false, UnityEvent<float> valueChangeEvent = null, float initialValue = 0f)
        {
            return group.Add(IgniteSlider.Create(onValueChanged, minValue, maxValue, wholeNumbers, valueChangeEvent, initialValue));
        }

        // label, slider
        public static IIgniteGUIGroup AddSlider(this IIgniteGUIGroup group, string label, Action<float> onValueChanged, float minValue = 0f, float maxValue = 1f, bool wholeNumbers = false, UnityEvent<float> valueChangeEvent = null, float initialValue = 0f)
        {
            return group.Add(IgniteHorizontalGroup.Create().AddLabel(label).Add(IgniteSlider.Create(onValueChanged, minValue, maxValue, wholeNumbers, valueChangeEvent, initialValue)) as IgniteHorizontalGroup);
        }

        // slider, label
        public static IIgniteGUIGroup AddSlider(this IIgniteGUIGroup group, Action<float> onValueChanged, string label, float minValue = 0f, float maxValue = 1f, bool wholeNumbers = false, UnityEvent<float> valueChangeEvent = null, float initialValue = 0f)
        {
            return group.Add(IgniteHorizontalGroup.Create().Add(IgniteSlider.Create(onValueChanged, minValue, maxValue, wholeNumbers, valueChangeEvent, initialValue)).AddLabel(label) as IgniteHorizontalGroup);
        }

        // monitoring slider
        public static IIgniteGUIGroup AddMonitoringSlider(this IIgniteGUIGroup group, Func<float> monitor, float minValue = 0f, float maxValue = 1f)
        {
            var valueChangeEvent = new IgniteSlider.ValueChangeEvent();
            var slider = IgniteSlider.Create(v => { }, minValue, maxValue, valueChangeEvent: valueChangeEvent);
            slider.StartCoroutine(MonitoringCoroutine(valueChangeEvent, monitor));
            return group.Add(slider);
        }

        // label, monitoring slider
        public static IIgniteGUIGroup AddMonitoringSlider(this IIgniteGUIGroup group, string label, Func<float> monitor, float minValue, float maxValue)
        {
            var valueChangeEvent = new IgniteSlider.ValueChangeEvent();
            var slider = IgniteSlider.Create(v => { }, minValue, maxValue, valueChangeEvent: valueChangeEvent);
            slider.StartCoroutine(MonitoringCoroutine(valueChangeEvent, monitor));
            return group.Add(IgniteHorizontalGroup.Create().AddLabel(label).Add(slider) as IgniteHorizontalGroup);
        }

        // monitoring slider, label
        public static IIgniteGUIGroup AddMonitoringSlider(this IIgniteGUIGroup group, Func<float> monitor, string label, float minValue = 0f, float maxValue = 1f)
        {
            var valueChangeEvent = new IgniteSlider.ValueChangeEvent();
            var slider = IgniteSlider.Create(v => { }, minValue, maxValue, valueChangeEvent: valueChangeEvent);
            slider.StartCoroutine(MonitoringCoroutine(valueChangeEvent, monitor));
            return group.Add(IgniteHorizontalGroup.Create().Add(slider).AddLabel(label) as IgniteHorizontalGroup);
        }

        // operable monitoring slider
        public static IIgniteGUIGroup AddOperableMonitoringSlider(this IIgniteGUIGroup group, Func<float> monitor, Action<float> onValueChanged, float minValue = 0f, float maxValue = 1f)
        {
            var valueChangeEvent = new IgniteSlider.ValueChangeEvent();
            var slider = IgniteSlider.Create(onValueChanged, minValue, maxValue, valueChangeEvent: valueChangeEvent);
            slider.StartCoroutine(MonitoringCoroutine(valueChangeEvent, monitor));
            return group.Add(slider);
        }

        // label, operable monitoring slider
        public static IIgniteGUIGroup AddOperableMonitoringSlider(this IIgniteGUIGroup group, string label, Func<float> monitor, Action<float> onValueChanged, float minValue = 0f, float maxValue = 1f)
        {
            var valueChangeEvent = new IgniteSlider.ValueChangeEvent();
            var slider = IgniteSlider.Create(onValueChanged, minValue, maxValue, valueChangeEvent: valueChangeEvent);
            slider.StartCoroutine(MonitoringCoroutine(valueChangeEvent, monitor));
            return group.Add(IgniteHorizontalGroup.Create().AddLabel(label).Add(slider) as IgniteHorizontalGroup);
        }

        // operable monitoring slider, label
        public static IIgniteGUIGroup AddOperableMonitoringSlider(this IIgniteGUIGroup group, Func<float> monitor, Action<float> onValueChanged, string label, float minValue = 0f, float maxValue = 1f)
        {
            var valueChangeEvent = new IgniteSlider.ValueChangeEvent();
            var slider = IgniteSlider.Create(onValueChanged, minValue, maxValue, valueChangeEvent: valueChangeEvent);
            slider.StartCoroutine(MonitoringCoroutine(valueChangeEvent, monitor));
            return group.Add(IgniteHorizontalGroup.Create().Add(slider).AddLabel(label) as IgniteHorizontalGroup);
        }

        static IEnumerator MonitoringCoroutine(UnityEvent<float> valueChangeEvent, Func<float> monitor)
        {
            while (true)
            {
                valueChangeEvent.Invoke(monitor());
                yield return null;
            }
        }
    }
}