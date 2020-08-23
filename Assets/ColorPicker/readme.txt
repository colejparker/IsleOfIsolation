-Basic usage: drag and drop ColorPicker prefab into the scene.
    -use "colorPicker.currentColor = yourCurrentColor" to set colorPicker's current color.
    -use "newColor = colorPicker.newColor" to get the picked color.
-NOTE: color picker's OK/Cancel button functions need to be manually assigned to suit your needs.

-Here is the demo scene example:
    public class Demo : MonoBehaviour
    {
        [SerializeField] ColorPicker colorPicker;
        Image currColor;

        public void OpenColorPicker(Image img)
        {
            currColor = img;
            colorPicker.currentColor = img.color;
        }

        public void PickColor()
        {
            currColor.color = colorPicker.newColor;
        }
    }

-Color conversion classes can be found in script "ColorFormat.cs", 
    -including 4 classes: ColorHSV, ColorLab, ColorHex and ColorCMYK.
    -use "FromColor" and "ToColor" functions to do the conversion.
    -example:
     ColorHSV myColor = new ColorHSV();
     myColor.FromColor(Color.white);
     Color newColor = myColor.ToColor();

-Eyedropper tool:
    -use "newColor = eyedropper.color" to get the eyedropper tool's current color.
    -Indicator: Image used to indicates eyeDropper's current color.
    -Color Picker: the color picker affacted by this EyeDropper, it will update the color picker's new color with this eyeDropper's current color. Can be null.
    -Preview: RawImage used for color preview, can be null.
    -Preview Size: number of pixels for preview image.
    -Preview Centre: Image used to indecates center of preview, can be null.
    -Preview Grid: Image used to display grid over preview area, can be null.
    -Preview Grid Line Width: width of preview grid line.
    -Cursor Icon: cursor icon for eyedropper, can be null.
    -Blocker: Transparent image used to block raycast while eyedropper is activated, can be null.