using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ColorPickerUtil
{
    public class Eyedropper : MonoBehaviour
    {
        [SerializeField] Image indicator;
        [SerializeField] ColorPicker colorPicker;
        [SerializeField] RawImage preview;
        [SerializeField] [RangeAttribute(1, 10)] int previewSize;
        [SerializeField] RectTransform previewCenter;
        [SerializeField] RectTransform previewGrid;
        [SerializeField] [RangeAttribute(1, 10)] int previewGridLineWidth;
        [SerializeField] Texture2D cursorIcon;
        [SerializeField] RectTransform blocker;

        Color m_color;
        public Color color
        {
            set
            {
                m_color = value;
                if (indicator != null) indicator.color = m_color;
            }
            get { return m_color; }
        }

        Texture2D colorSample;

        private void Awake()
        {
            if (preview == null)
            {
                colorSample = new Texture2D(1, 1, TextureFormat.RGB24, false);
            }
            else
            {
                int size = previewSize * 2 + 1;
                colorSample = new Texture2D(size, size, TextureFormat.RGB24, false);
                colorSample.filterMode = FilterMode.Point;
                preview.texture = colorSample;

                if (previewGrid != null)
                {
                    previewGrid.SetParent(preview.transform);
                    previewGrid.sizeDelta = Vector2.zero;
                    previewGrid.anchoredPosition = Vector2.zero;
                    previewGrid.gameObject.SetActive(true);
                }

                if (previewCenter != null)
                {
                    previewCenter.SetParent(preview.transform);
                    previewCenter.sizeDelta = new Vector2(preview.rectTransform.rect.width / size, preview.rectTransform.rect.height / size);
                    previewCenter.anchoredPosition = Vector2.zero;
                    previewCenter.gameObject.SetActive(true);
                }
            }
        }

        public void EnableEyedropper()
        {
            if (blocker != null)
            {
                blocker.parent = transform.root;
                blocker.gameObject.SetActive(true);
                blocker.anchorMin = Vector2.zero;
                blocker.anchorMax = Vector2.one;
                blocker.anchoredPosition = Vector2.zero;
                blocker.sizeDelta = Vector2.zero;
            }
            if (preview != null && previewGrid != null)
            {
                previewGrid.GetComponent<Image>().material.SetFloat("_NumCell", previewSize * 2 + 1);
                previewGrid.GetComponent<Image>().material.SetFloat("_LineWidth", previewGridLineWidth / 10.0f);
            }
            if (cursorIcon != null) Cursor.SetCursor(cursorIcon, new Vector2(15f, 120f), CursorMode.Auto);
            StartCoroutine(UpdateColor());
        }

        public void DisableEyedropper()
        {
            if (blocker != null)
            {
                blocker.parent = transform;
                blocker.gameObject.SetActive(false);
            }
            if (colorPicker != null) colorPicker.UpdateNewColor(indicator.color);
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }

        private IEnumerator UpdateColor()
        {
            while (true)
            {
                if (Input.GetKey(KeyCode.Escape))
                {
                    if (colorPicker != null) colorPicker.Refresh();
                    DisableEyedropper();
                    break;
                }

                if (Input.GetMouseButton(0) || Input.touchCount == 1)
                {
                    DisableEyedropper();
                    break;
                }

                yield return new WaitForEndOfFrame();

                if (Input.mousePosition.x > previewSize && 
                    Input.mousePosition.y > previewSize &&
                    Input.mousePosition.x < Screen.width - previewSize - 1 && 
                    Input.mousePosition.y < Screen.height - previewSize - 1)
                {
                    Rect rect = new Rect(Input.mousePosition.x - previewSize, Input.mousePosition.y - previewSize, previewSize * 2 + 1, previewSize * 2 + 1);
                    colorSample.ReadPixels(rect, 0, 0, false);
                    colorSample.Apply();
                    color = colorSample.GetPixel(previewSize, previewSize);
                }
            }
            yield return null;
        }
    }
}