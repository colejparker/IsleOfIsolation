using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace ColorPickerUtil
{
    public class PickArea : Selectable, IDragHandler, IInitializePotentialDragHandler, ICanvasElement
    {
        [SerializeField] RectTransform indicator;
        [SerializeField] ColorPicker colorPicker;
        [SerializeField] RectTransform rectTransform;
        [SerializeField] Image indicatorImage;

        Vector2 m_Offset = Vector2.zero;
        Texture2D m_texture;
        Texture2D texture
        {
            get
            {
                if (m_texture == null)
                {
                    int width = (int)Mathf.Clamp(rectTransform.rect.width * 0.25f, 0.0f, 512.0f);
                    int height = (int)Mathf.Clamp(rectTransform.rect.height * 0.25f, 0.0f, 512.0f);
                    m_texture = new Texture2D(width, height);
                    m_texture.wrapMode = TextureWrapMode.Clamp;
                    GetComponent<RawImage>().texture = m_texture;
                }
                return m_texture;
            }
        }
        Vector2 m_value = new Vector2();
        public Vector2 value
        {
            set
            {
                m_value.x = Mathf.Clamp01(value.x);
                m_value.y = Mathf.Clamp01(value.y);
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
            m_value.x = Mathf.Clamp01((localCursor.x - m_Offset.x) / rectTransform.rect.width);
            m_value.y = Mathf.Clamp01((localCursor.y - m_Offset.y) / rectTransform.rect.height);
            UpdateIndicator();

            if (colorPicker.newColorLab.L > 62.0f) indicatorImage.color = Color.black;
            else indicatorImage.color = Color.white;
            colorPicker.pickBar.Refresh();
            colorPicker.RefreshColor();
        }

        void UpdateIndicator()
        {
            Vector2 pos = new Vector2();
            pos.x = m_value.x * rectTransform.rect.width;
            pos.y = m_value.y * rectTransform.rect.height;
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
                                color.h = colorPicker.pickBar.value * 360.0f;
                                color.s = (float)x / texture.width;
                                color.v = (float)y / texture.height;
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
                                color.h = (float)x / texture.width * 360.0f;
                                color.s = colorPicker.pickBar.value;
                                color.v = (float)y / texture.height;
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
                                color.h = (float)x / texture.width * 360.0f;
                                color.s = (float)y / texture.height;
                                color.v = colorPicker.pickBar.value;
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
                                color.r = colorPicker.pickBar.value;
                                color.g = (float)y / texture.height;
                                color.b = (float)x / texture.width;
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
                                color.r = (float)y / texture.height;
                                color.g = colorPicker.pickBar.value;
                                color.b = (float)x / texture.width;
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
                                color.r = (float)x / texture.width;
                                color.g = (float)y / texture.height;
                                color.b = colorPicker.pickBar.value;
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
                                color.L = colorPicker.pickBar.value * 100.0f;
                                color.a = (float)x / texture.width * 255.0f - 128.0f;
                                color.b = (float)y / texture.height * 255.0f - 128.0f;
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
                                color.L = (float)y / texture.height * 100.0f;
                                color.a = colorPicker.pickBar.value * 255.0f - 128.0f;
                                color.b = (float)x / texture.width * 255.0f - 128.0f;
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
                                color.L = (float)y / texture.height * 100.0f;
                                color.a = (float)x / texture.width * 255.0f - 128.0f;
                                color.b = colorPicker.pickBar.value * 255.0f - 128.0f;
                                color.alpha = 1.0f;
                                texture.SetPixel(x, y, color.ToColor());
                            }
                    }
                    break;
                default:
                    break;
            }
            texture.Apply();
            if(colorPicker.newColorLab !=null)
            {
                if (colorPicker.newColorLab.L > 62.0f) indicatorImage.color = Color.black;
                else indicatorImage.color = Color.white;
            }
            
        }

        public void Rebuild(CanvasUpdate executing) { }

        public void LayoutComplete() { }

        public void GraphicUpdateComplete() { }
    }
}