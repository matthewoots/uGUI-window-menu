﻿using UnityEngine;
using UnityEngine.UI;
using ATweening;
using System;
using UnityEngine.EventSystems;

namespace IgniteModule.GUICore
{
    public class IgniteWindowHeader : GUIMonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Toggle toggle = null;
        [SerializeField] private Text headerName = null;
        [SerializeField] private Button killButton = null;
        [SerializeField] private RectTransform arrowImageRect = null;
        [SerializeField] private RectTransform toggleRect = null;
        [SerializeField] private RectTransform killButtonRect = null;
        [SerializeField] private HorizontalLayoutGroup headerLayoutGroup = null;

        int clickCount;
        float lastTime;

        public void SetToggleValue(bool isOn)
        {
            toggle.isOn = isOn;
        }

        public void SetKillButtonActive(bool isActive)
        {
            killButton.gameObject.SetActive(isActive);
        }

        public void SetHeight(float height)
        {
            this.RectTransform.SetSizeDelta(y: height);
            toggleRect.SetSizeDelta(height, height);
            killButtonRect.SetSizeDelta(height, height);
            var rectOffset = new RectOffset((int)height, (int)height, 0, 0);
            headerLayoutGroup.padding = rectOffset;
        }

        public void SetName(string name)
        {
            headerName.text = name;
        }

        public void OnToggleValueChanged(Action<bool> onValueChanged)
        {
            toggle.onValueChanged.AddListener(v => onValueChanged(v));
        }

        public void OnClickKillButton(Action onClick)
        {
            killButton.onClick.AddListener(() => onClick());
        }

        void Start()
        {
            headerName.font = IgniteGUISettings.Font;
            headerName.fontSize = IgniteGUISettings.FontSize;
            SetHeight(IgniteGUISettings.ElementHeight);

            toggle.onValueChanged.AddListener(v =>
            {
                if (v)
                {
                    toggle.enabled = false;
                    arrowImageRect.DoLocalRotate(new Vector3(0f, 0f, -90f), 0.3f)
                            .OnComplete(() => toggle.enabled = true)
                            .SetRelative();
                }
                else
                {
                    toggle.enabled = false;
                    arrowImageRect.DoLocalRotate(new Vector3(0f, 0f, 90f), 0.3f)
                            .OnComplete(() => toggle.enabled = true)
                            .SetRelative();
                }
            });
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            clickCount++;
            var currentTime = eventData.clickTime;
            if (Mathf.Abs(currentTime - lastTime) >= 0.75f)
            {
                clickCount = 1;
            }

            lastTime = currentTime;

            if (clickCount >= 2)
            {
                toggle.isOn = !toggle.isOn;
                clickCount = 0;
            }
        }
    }
}