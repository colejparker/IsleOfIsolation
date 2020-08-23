using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ColorPickerUtil
{
    public class ColorPicker : MonoBehaviour
    {
        public enum Mode
        {
            HSV_H,
            HSV_S,
            HSV_V,
            RGB_R,
            RGB_G,
            RGB_B,
            Lab_L,
            Lab_a,
            Lab_b
        }

        [SerializeField] Image newColorImage;
        [SerializeField] Image currentColorImage;
        [SerializeField] InputField input_HSV_H;
        [SerializeField] InputField input_HSV_S;
        [SerializeField] InputField input_HSV_V;
        [SerializeField] InputField input_RGB_R;
        [SerializeField] InputField input_RGB_G;
        [SerializeField] InputField input_RGB_B;
        [SerializeField] InputField input_Lab_L;
        [SerializeField] InputField input_Lab_a;
        [SerializeField] InputField input_Lab_b;
        [SerializeField] InputField input_Hex;
        [SerializeField] InputField input_CMYK_C;
        [SerializeField] InputField input_CMYK_M;
        [SerializeField] InputField input_CMYK_Y;
        [SerializeField] InputField input_CMYK_K;
        [SerializeField] InputField input_Alpha;

        private bool inputFieldLock = false;
        public Mode mode;
        public PickArea pickArea;
        public PickBar pickBar;
        public Slider alphaSlider;

        private float m_alpha = 1.0f;
        private Color m_newColor;
        public Color newColor { get { return new Color(m_newColor.r, m_newColor.g, m_newColor.b, m_alpha); } }
        private ColorHSV m_newColorHSV;
        public ColorHSV newColorHSV { get { return new ColorHSV(m_newColorHSV.h, m_newColorHSV.s, m_newColorHSV.v, m_alpha); } }
        private ColorLab m_newColorLab;
        public ColorLab newColorLab { get { return new ColorLab(m_newColorLab.L, m_newColorLab.a, m_newColorLab.b, m_alpha); } }
        private ColorHex m_newColorHex;
        public ColorHex newColorHex { get { return new ColorHex(m_newColorHex.hex, m_alpha); } }
        private ColorCMYK m_newColorCMYK;
        public ColorCMYK newColorCMYK { get { return new ColorCMYK(m_newColorCMYK.c, m_newColorCMYK.m, m_newColorCMYK.y, m_newColorCMYK.k, m_alpha); } }
        private Color m_currentColor;
        public Color currentColor
        {
            set
            {
                m_currentColor = value;
                UpdateNewColor(value);
            }
            get { return m_currentColor; }
        }

        private void Awake()
        {
            currentColor = Color.black;
        }

        public void UpdateNewColor(Color color)
        {
            m_newColor = color;
            m_newColorHSV = new ColorHSV(m_newColor);
            m_newColorLab = new ColorLab(m_newColor);
            m_newColorHex = new ColorHex(m_newColor);
            m_newColorCMYK = new ColorCMYK(m_newColor);
            Refresh();
        }

        public void Refresh()
        {
            RefreshInputField();
            RefreshPicker();
        }

        public void RefreshInputField()
        {
            inputFieldLock = true;

            input_HSV_H.text = m_newColorHSV.h.ToString("0");
            input_HSV_S.text = (m_newColorHSV.s * 100.0f).ToString("0");
            input_HSV_V.text = (m_newColorHSV.v * 100.0f).ToString("0");

            input_RGB_R.text = (m_newColor.r * 255.0f).ToString("0");
            input_RGB_G.text = (m_newColor.g * 255.0f).ToString("0");
            input_RGB_B.text = (m_newColor.b * 255.0f).ToString("0");

            input_Lab_L.text = m_newColorLab.L.ToString("0");
            input_Lab_a.text = m_newColorLab.a.ToString("0");
            input_Lab_b.text = m_newColorLab.b.ToString("0");

            input_CMYK_C.text = (m_newColorCMYK.c * 100.0f).ToString("0");
            input_CMYK_M.text = (m_newColorCMYK.m * 100.0f).ToString("0");
            input_CMYK_Y.text = (m_newColorCMYK.y * 100.0f).ToString("0");
            input_CMYK_K.text = (m_newColorCMYK.k * 100.0f).ToString("0");

            input_Hex.text = m_newColorHex.hex;

            newColorImage.color = newColor;
            currentColorImage.color = currentColor;

            inputFieldLock = false;
        }

        public void RefreshAlpha()
        {
            if (inputFieldLock) return;
            m_alpha = alphaSlider.value;
            inputFieldLock = true;
            input_Alpha.text = (m_alpha * 100.0f).ToString("0");
            inputFieldLock = false;
            newColorImage.color = newColor;
        }

        public void RefreshColor()
        {
            switch (mode)
            {
                case Mode.HSV_H:
                    {
                        float h = pickBar.value * 360.0f;
                        float s = pickArea.value.x;
                        float v = pickArea.value.y;

                        m_newColorHSV = new ColorHSV(h, s, v);
                        m_newColor = m_newColorHSV.ToColor();
                        m_newColorLab = new ColorLab(m_newColor);
                    }
                    break;
                case Mode.HSV_S:
                    {
                        float h = pickArea.value.x * 360.0f;
                        float s = pickBar.value;
                        float v = pickArea.value.y;

                        m_newColorHSV = new ColorHSV(h, s, v);
                        m_newColor = m_newColorHSV.ToColor();
                        m_newColorLab = new ColorLab(m_newColor);
                    }
                    break;
                case Mode.HSV_V:
                    {
                        float h = pickArea.value.x * 360.0f;
                        float s = pickArea.value.y;
                        float v = pickBar.value;

                        m_newColorHSV = new ColorHSV(h, s, v);
                        m_newColor = m_newColorHSV.ToColor();
                        m_newColorLab = new ColorLab(m_newColor);
                    }
                    break;
                case Mode.RGB_R:
                    {
                        float r = pickBar.value;
                        float g = pickArea.value.y;
                        float b = pickArea.value.x;

                        m_newColor = new Color(r, g, b);
                        m_newColorHSV = new ColorHSV(m_newColor);
                        m_newColorLab = new ColorLab(m_newColor);
                    }
                    break;
                case Mode.RGB_G:
                    {
                        float r = pickArea.value.y;
                        float g = pickBar.value;
                        float b = pickArea.value.x;

                        m_newColor = new Color(r, g, b);
                        m_newColorHSV = new ColorHSV(m_newColor);
                        m_newColorLab = new ColorLab(m_newColor);
                    }
                    break;
                case Mode.RGB_B:
                    {
                        float r = pickArea.value.x;
                        float g = pickArea.value.y;
                        float b = pickBar.value;

                        m_newColor = new Color(r, g, b);
                        m_newColorHSV = new ColorHSV(m_newColor);
                        m_newColorLab = new ColorLab(m_newColor);
                    }
                    break;
                case Mode.Lab_L:
                    {
                        float L = pickBar.value * 100.0f;
                        float a = pickArea.value.x * 255.0f - 128.0f;
                        float b = pickArea.value.y * 255.0f - 128.0f;

                        m_newColorLab = new ColorLab(L, a, b);
                        m_newColor = m_newColorLab.ToColor();
                        m_newColorHSV = new ColorHSV(m_newColor);
                    }
                    break;
                case Mode.Lab_a:
                    {
                        float L = pickArea.value.y * 100.0f;
                        float a = pickBar.value * 255.0f - 128.0f;
                        float b = pickArea.value.x * 255.0f - 128.0f;

                        m_newColorLab = new ColorLab(L, a, b);
                        m_newColor = m_newColorLab.ToColor();
                        m_newColorHSV = new ColorHSV(m_newColor);
                    }
                    break;
                case Mode.Lab_b:
                    {
                        float L = pickArea.value.y * 100.0f;
                        float a = pickArea.value.x * 255.0f - 128.0f;
                        float b = pickBar.value * 255.0f - 128.0f;

                        m_newColorLab = new ColorLab(L, a, b);
                        m_newColor = m_newColorLab.ToColor();
                        m_newColorHSV = new ColorHSV(m_newColor);
                    }
                    break;
                default:
                    break;
            }

            m_newColorHex = new ColorHex(m_newColor);
            m_newColorCMYK = new ColorCMYK(m_newColor);

            RefreshInputField();
        }

        public void RefreshPicker()
        {
            Vector2 areaValue = new Vector2();
            float barValue = 0.0f;
            switch (mode)
            {
                case Mode.HSV_H:
                    {
                        barValue = m_newColorHSV.h / 360.0f;
                        areaValue.x = m_newColorHSV.s;
                        areaValue.y = m_newColorHSV.v;
                    }
                    break;
                case Mode.HSV_S:
                    {
                        barValue = m_newColorHSV.s;
                        areaValue.x = m_newColorHSV.h / 360.0f;
                        areaValue.y = m_newColorHSV.v;
                    }
                    break;
                case Mode.HSV_V:
                    {
                        barValue = m_newColorHSV.v;
                        areaValue.x = m_newColorHSV.h / 360.0f;
                        areaValue.y = m_newColorHSV.s;
                    }
                    break;
                case Mode.RGB_R:
                    {
                        barValue = m_newColor.r;
                        areaValue.x = m_newColor.b;
                        areaValue.y = m_newColor.g;
                    }
                    break;
                case Mode.RGB_G:
                    {
                        barValue = m_newColor.g;
                        areaValue.x = m_newColor.b;
                        areaValue.y = m_newColor.r;
                    }
                    break;
                case Mode.RGB_B:
                    {
                        barValue = m_newColor.b;
                        areaValue.x = m_newColor.r;
                        areaValue.y = m_newColor.g;
                    }
                    break;
                case Mode.Lab_L:
                    {
                        barValue = m_newColorLab.L / 100.0f;
                        areaValue.x = (m_newColorLab.a + 128.0f) / 255.0f;
                        areaValue.y = (m_newColorLab.b + 128.0f) / 255.0f;
                    }
                    break;
                case Mode.Lab_a:
                    {
                        barValue = (m_newColorLab.a + 128.0f) / 255.0f;
                        areaValue.x = (m_newColorLab.b + 128.0f) / 255.0f;
                        areaValue.y = m_newColorLab.L / 100.0f;
                    }
                    break;
                case Mode.Lab_b:
                    {
                        barValue = (m_newColorLab.b + 128.0f) / 255.0f;
                        areaValue.x = (m_newColorLab.a + 128.0f) / 255.0f;
                        areaValue.y = m_newColorLab.L / 100.0f;
                    }
                    break;
                default:
                    break;
            }
            pickArea.value = areaValue;
            pickBar.value = barValue;
            pickArea.Refresh();
            pickBar.Refresh();
        }

        public void SwitchMode(Mode mode)
        {
            this.mode = mode;
            RefreshPicker();
        }

        public void SwitchModeHSV_H(bool isOn)
        {
            if (!isOn) return;
            SwitchMode(Mode.HSV_H);
        }

        public void SwitchModeHSV_S(bool isOn)
        {
            if (!isOn) return;
            SwitchMode(Mode.HSV_S);
        }

        public void SwitchModeHSV_V(bool isOn)
        {
            if (!isOn) return;
            SwitchMode(Mode.HSV_V);
        }

        public void SwitchModeRGB_R(bool isOn)
        {
            if (!isOn) return;
            SwitchMode(Mode.RGB_R);
        }

        public void SwitchModeRGB_G(bool isOn)
        {
            if (!isOn) return;
            SwitchMode(Mode.RGB_G);
        }

        public void SwitchModeRGB_B(bool isOn)
        {
            if (!isOn) return;
            SwitchMode(Mode.RGB_B);
        }

        public void SwitchModeLab_L(bool isOn)
        {
            if (!isOn) return;
            SwitchMode(Mode.Lab_L);
        }

        public void SwitchModeLab_a(bool isOn)
        {
            if (!isOn) return;
            SwitchMode(Mode.Lab_a);
        }

        public void SwitchModeLab_b(bool isOn)
        {
            if (!isOn) return;
            SwitchMode(Mode.Lab_b);
        }

        public void InputChangedHSV_H(string str)
        {
            if (str == "" || str == "-" || inputFieldLock) return;
            m_newColorHSV.h = Mathf.Clamp(int.Parse(str), 0, 360);
            m_newColor = m_newColorHSV.ToColor();
            m_newColorLab = new ColorLab(m_newColor);
            m_newColorHex = new ColorHex(m_newColor);
            m_newColorCMYK = new ColorCMYK(m_newColor);
            RefreshInputField();
            RefreshPicker();
        }

        public void InputChangedHSV_S(string str)
        {
            if (str == "" || str == "-" || inputFieldLock) return;
            m_newColorHSV.s = Mathf.Clamp01(int.Parse(str) / 100.0f);
            m_newColor = m_newColorHSV.ToColor();
            m_newColorLab = new ColorLab(m_newColor);
            m_newColorHex = new ColorHex(m_newColor);
            m_newColorCMYK = new ColorCMYK(m_newColor);
            RefreshInputField();
            RefreshPicker();
        }

        public void InputChangedHSV_V(string str)
        {
            if (str == "" || str == "-" || inputFieldLock) return;
            m_newColorHSV.v = Mathf.Clamp01(int.Parse(str) / 100.0f);
            m_newColor = m_newColorHSV.ToColor();
            m_newColorLab = new ColorLab(m_newColor);
            m_newColorHex = new ColorHex(m_newColor);
            m_newColorCMYK = new ColorCMYK(m_newColor);
            RefreshInputField();
            RefreshPicker();
        }

        public void InputChangedRGB_R(string str)
        {
            if (str == "" || str == "-" || inputFieldLock) return;
            m_newColor.r = Mathf.Clamp01(int.Parse(str) / 255.0f);
            m_newColorHSV = new ColorHSV(m_newColor);
            m_newColorLab = new ColorLab(m_newColor);
            m_newColorHex = new ColorHex(m_newColor);
            m_newColorCMYK = new ColorCMYK(m_newColor);
            RefreshInputField();
            RefreshPicker();
        }

        public void InputChangedRGB_G(string str)
        {
            if (str == "" || str == "-" || inputFieldLock) return;
            m_newColor.g = Mathf.Clamp01(int.Parse(str) / 255.0f);
            m_newColorHSV = new ColorHSV(m_newColor);
            m_newColorLab = new ColorLab(m_newColor);
            m_newColorHex = new ColorHex(m_newColor);
            m_newColorCMYK = new ColorCMYK(m_newColor);
            RefreshInputField();
            RefreshPicker();
        }

        public void InputChangedRGB_B(string str)
        {
            if (str == "" || str == "-" || inputFieldLock) return;
            m_newColor.b = Mathf.Clamp01(int.Parse(str) / 255.0f);
            m_newColorHSV = new ColorHSV(m_newColor);
            m_newColorLab = new ColorLab(m_newColor);
            m_newColorHex = new ColorHex(m_newColor);
            m_newColorCMYK = new ColorCMYK(m_newColor);
            RefreshInputField();
            RefreshPicker();
        }

        public void InputChangedLab_L(string str)
        {
            if (str == "" || str == "-" || inputFieldLock) return;
            m_newColorLab.L = Mathf.Clamp(int.Parse(str), 0, 100);
            m_newColor = m_newColorLab.ToColor();
            m_newColorHSV = new ColorHSV(m_newColor);
            m_newColorHex = new ColorHex(m_newColor);
            m_newColorCMYK = new ColorCMYK(m_newColor);
            RefreshInputField();
            RefreshPicker();
        }

        public void InputChangedLab_a(string str)
        {
            if (str == "" || str == "-" || inputFieldLock) return;
            m_newColorLab.a = Mathf.Clamp(int.Parse(str), -128, 127);
            m_newColor = m_newColorLab.ToColor();
            m_newColorHSV = new ColorHSV(m_newColor);
            m_newColorHex = new ColorHex(m_newColor);
            m_newColorCMYK = new ColorCMYK(m_newColor);
            RefreshInputField();
            RefreshPicker();
        }

        public void InputChangedLab_b(string str)
        {
            if (str == "" || str == "-" || inputFieldLock) return;
            m_newColorLab.b = Mathf.Clamp(int.Parse(str), -128, 127);
            m_newColor = m_newColorLab.ToColor();
            m_newColorHSV = new ColorHSV(m_newColor);
            m_newColorHex = new ColorHex(m_newColor);
            m_newColorCMYK = new ColorCMYK(m_newColor);
            RefreshInputField();
        }

        public void InputChangedCMYK_C(string str)
        {
            if (str == "" || str == "-" || inputFieldLock) return;
            m_newColorCMYK.c = Mathf.Clamp01(int.Parse(str) / 100.0f);
            m_newColor = m_newColorCMYK.ToColor();
            m_newColorHSV = new ColorHSV(m_newColor);
            m_newColorLab = new ColorLab(m_newColor);
            m_newColorHex = new ColorHex(m_newColor);
            RefreshInputField();
            RefreshPicker();
        }

        public void InputChangedCMYK_M(string str)
        {
            if (str == "" || str == "-" || inputFieldLock) return;
            m_newColorCMYK.m = Mathf.Clamp01(int.Parse(str) / 100.0f);
            m_newColor = m_newColorCMYK.ToColor();
            m_newColorHSV = new ColorHSV(m_newColor);
            m_newColorLab = new ColorLab(m_newColor);
            m_newColorHex = new ColorHex(m_newColor);
            RefreshInputField();
            RefreshPicker();
        }

        public void InputChangedCMYK_Y(string str)
        {
            if (str == "" || str == "-" || inputFieldLock) return;
            m_newColorCMYK.y = Mathf.Clamp01(int.Parse(str) / 100.0f);
            m_newColor = m_newColorCMYK.ToColor();
            m_newColorHSV = new ColorHSV(m_newColor);
            m_newColorLab = new ColorLab(m_newColor);
            m_newColorHex = new ColorHex(m_newColor);
            RefreshInputField();
            RefreshPicker();
        }

        public void InputChangedCMYK_K(string str)
        {
            if (str == "" || str == "-" || inputFieldLock) return;
            m_newColorCMYK.k = Mathf.Clamp01(int.Parse(str) / 100.0f);
            m_newColor = m_newColorCMYK.ToColor();
            m_newColorHSV = new ColorHSV(m_newColor);
            m_newColorLab = new ColorLab(m_newColor);
            m_newColorHex = new ColorHex(m_newColor);
            RefreshInputField();
            RefreshPicker();
        }

        public void InputChangedHex(string str)
        {
            if (str == m_newColorHex.hex || inputFieldLock) return;
            m_newColorHex = new ColorHex(str);
            m_newColor = m_newColorHex.ToColor();
            m_newColorHSV = new ColorHSV(m_newColor);
            m_newColorLab = new ColorLab(m_newColor);
            m_newColorCMYK = new ColorCMYK(m_newColor);
            RefreshInputField();
            RefreshPicker();
        }

        public void InputChangedAlpha(string str)
        {
            if (str == "" || str == "-" || inputFieldLock) return;
            m_alpha = Mathf.Clamp01(int.Parse(str) / 100.0f);
            inputFieldLock = true;
            input_Alpha.text = (m_alpha * 100.0f).ToString("0");
            alphaSlider.value = m_alpha;
            inputFieldLock = false;
            newColorImage.color = newColor;
        }

        public void InputEndHSV_H(string str)
        {
            if (str == "" || str == "-") { str = "0"; InputChangedHSV_H(str); }
        }

        public void InputEndHSV_S(string str)
        {
            if (str == "" || str == "-") { str = "0"; InputChangedHSV_S(str); }
        }

        public void InputEndHSV_V(string str)
        {
            if (str == "" || str == "-") { str = "0"; InputChangedHSV_V(str); }
        }

        public void InputEndRGB_R(string str)
        {
            if (str == "" || str == "-") { str = "0"; InputChangedRGB_R(str); }
        }

        public void InputEndRGB_G(string str)
        {
            if (str == "" || str == "-") { str = "0"; InputChangedRGB_G(str); }
        }

        public void InputEndRGB_B(string str)
        {
            if (str == "" || str == "-") { str = "0"; InputChangedRGB_B(str); }
        }

        public void InputEndLab_L(string str)
        {
            if (str == "" || str == "-") { str = "0"; InputChangedLab_L(str); }
        }

        public void InputEndLab_a(string str)
        {
            if (str == "" || str == "-") { str = "0"; InputChangedLab_a(str); }
        }

        public void InputEndLab_b(string str)
        {
            if (str == "" || str == "-") { str = "0"; InputChangedLab_b(str); }
        }

        public void InputEndCMYK_C(string str)
        {
            if (str == "" || str == "-") { str = "0"; InputChangedCMYK_C(str); }
        }

        public void InputEndCMYK_M(string str)
        {
            if (str == "" || str == "-") { str = "0"; InputChangedCMYK_M(str); }
        }

        public void InputEndCMYK_Y(string str)
        {
            if (str == "" || str == "-") { str = "0"; InputChangedCMYK_Y(str); }
        }

        public void InputEndCMYK_K(string str)
        {
            if (str == "" || str == "-") { str = "0"; InputChangedCMYK_K(str); }
        }

        public void InputEndHex(string str)
        {
            while (str.Length < 6) str = '0' + str;
            InputChangedHex(str);
        }

        public void InputEndAlpha(string str)
        {
            if (str == "" || str == "-") { str = "0"; InputEndAlpha(str); }
        }
    }
}