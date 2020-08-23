using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace ColorPickerUtil
{
    public class PickBar : Selectable, IDragHandler, IInitializePotentialDragHandler, ICanvasElement
    {
        [SerializeField] RectTransform indicator;
        [SerializeField] ColorPicker colorPicker;
        [SerializeField] RectTransform rectTransform;

        Vector2 m_Offset = Vector2.zero;
        Texture2D m_texture;
        Texture2D texture
        {
            get
            {
                if (m_texture == null)
                {
                    int height = (int)Mathf.Clamp(rectTransform.rect.height * 0.25f, 0.0f, 512.0f);
                    m_texture = new Texture2D(1, height);
                    m_texture.wrapMode = TextureWrapMode.Clamp;
                    GetComponent<RawImage>().texture = m_texture;
                }
                return m_texture;
            }
        }
        float m_value;
        public float value
        {
            set {
                m_value = Mathf.Clamp01(value);
                UpdateIndicator();
            }
            get { return m_value; }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left) return;
            UpdateDrag(eventData, eventData.pressEventCamera);
        }

        public void OnInitializePotentialDrag(PointerEventData eventData)
        {
            eventData.useDragThreshold = false;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left) return;
            base.OnPointerDown(eventData);

            m_Offset = Vector2.zero;
            if (RectTransformUtility.RectangleContainsScreenPoint(indicator, eventData.position, eventData.enterEventCamera))
            {
                Vector2 localMousePos;
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(indicator, eventData.position, eventData.pressEventCamera, out localMousePos))
                    m_Offset = localMousePos;
            }
            else
            {
                UpdateDrag(eventData, eventData.pressEventCamera);
            }
        }

        void UpdateDrag(PointerEventData eventData, Camera cam)
        {
            Vector2 localCursor;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, cam, out localCursor)) return;
            localCursor -= rectTransform.rect.position;
            m_value = Mathf.Clamp01((localCursor.y - m_Offset.y) / rectTransform.rect.height);
            UpdateIndicator();

            colorPicker.pickArea.Refresh();
            colorPicker.RefreshColor();
        }

        void UpdateIndicator()
        {
            Vector2 pos = Vector2.zero;
            pos.y = m_value * rectTransform.rect.height;
            indicator.anchoredPosition = pos;
        }

        public void Refresh()
        {
            switch (colorPicker.mode)
            {
                case ColorPicker.Mode.HSV_H:
                    {
                        for (int y = 0; y < texture.height; ++y)
                            for (int x = 0; x < texture.width; ++x)
                            {
                                ColorHSV color = new ColorHSV();
                                color.h = (float)y / texture.height * 360.0f;
                                color.s = 1.0f;
                                color.v = 1.0f;
                                color.alpha = 1.0f;
                                texture.SetPixel(x, y, color.ToColor());
                            }
                    }
                    break;
                case ColorPicker.Mode.HSV_S:
                    {
                        for (int y = 0; y < texture.height; ++y)
                            for (int x = 0; x < texture.width; ++x)
                            {
                                ColorHSV color = new ColorHSV();
                                color.h = colorPicker.pickArea.value.x * 360.0f;
                                color.s = (float)y / texture.height;
                                color.v = colorPicker.pickArea.value.y;
                                color.alpha = 1.0f;
                                texture.SetPixel(x, y, color.ToColor());
                            }
                    }
                    break;
                case ColorPicker.Mode.HSV_V:
                    {
                        for (int y = 0; y < texture.height; ++y)
                            for (int x = 0; x < texture.width; ++x)
                            {
                                ColorHSV color = new ColorHSV();
                                color.h = colorPicker.pickArea.value.x * 360.0f;
                                color.s = colorPicker.pickArea.value.y;
                                color.v = (float)y / texture.height;
                                color.alpha = 1.0f;
                                texture.SetPixel(x, y, color.ToColor());
                            }
                    }
                    break;
                case ColorPicker.Mode.RGB_R:
                    {
                        for (int y = 0; y < texture.height; ++y)
                            for (int x = 0; x < texture.width; ++x)
                            {
                                Color color = new Color();
                                color.r = (float)y / texture.height;
                                color.g = colorPicker.pickArea.value.y;
                                color.b = colorPicker.pickArea.value.x;
                                color.a = 1.0f;
                                texture.SetPixel(x, y, color);
                            }
                    }
                    break;
                case ColorPicker.Mode.RGB_G:
                    {
                        for (int y = 0; y < texture.height; ++y)
                            for (int x = 0; x < texture.width; ++x)
                            {
                                Color color = new Color();
                                color.r = colorPicker.pickArea.value.y;
                                color.g = (float)y / texture.height;
                                color.b = colorPicker.pickArea.value.x;
                                color.a = 1.0f;
                                texture.SetPixel(x, y, color);
                            }
                    }
                    break;
                case ColorPicker.Mode.RGB_B:
                    {
                        for (int y = 0; y < texture.height; ++y)
                            for (int x = 0; x < texture.width; ++x)
                            {
                                Color color = new Color();
                                color.r = colorPicker.pickArea.value.x;
                                color.g = colorPicker.pickArea.value.y;
                                color.b = (float)y / texture.height;
                                color.a = 1.0f;
                                texture.SetPixel(x, y, color);
                            }
                    }
                    break;
                case ColorPicker.Mode.Lab_L:
                    {
                        for (int y = 0; y < texture.height; ++y)
                            for (int x = 0; x < texture.width; ++x)
                            {
                                ColorLab color = new ColorLab();
                                color.L = (float)y / texture.height * 100.0f;
                                color.a = colorPicker.pickArea.value.x * 255.0f - 128.0f;
                                color.b = colorPicker.pickArea.value.y * 255.0f - 128.0f;
                                color.alpha = 1.0f;
                                texture.SetPixel(x, y, color.ToColor());
                            }
                    }
                    break;
                case ColorPicker.Mode.Lab_a:
                    {
                        for (int y = 0; y < texture.height; ++y)
                            for (int x = 0; x < texture.width; ++x)
                            {
                                ColorLab color = new ColorLab();
                                color.L = colorPicker.pickArea.value.y * 100.0f;
                                color.a = (float)y / texture.height * 255.0f - 128.0f;
                                color.b = colorPicker.pickArea.value.x * 255.0f - 128.0f;
                                color.alpha = 1.0f;
                                texture.SetPixel(x, y, color.ToColor());
                            }
                    }
                    break;
                case ColorPicker.Mode.Lab_b:
                    {
                        for (int y = 0; y < texture.height; ++y)
                            for (int x = 0; x < texture.width; ++x)
                            {
                                ColorLab color = new ColorLab();
                                color.L = colorPicker.pickArea.value.y * 100.0f;
                                color.a = colorPicker.pickArea.value.x * 255.0f - 128.0f;
                                color.b = (float)y / texture.height * 255.0f - 128.0f;
                                color.alpha = 1.0f;
                                texture.SetPixel(x, y, color.ToColor());
                            }
                    }
                    break;
                default:
                    break;
            }
            texture.Apply();
        }

        public void Rebuild(CanvasUpdate executing) { }

        public void LayoutComplete() { }

        public void GraphicUpdateComplete() { }
    }
}