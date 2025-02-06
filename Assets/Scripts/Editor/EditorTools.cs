using UnityEngine;
using UnityEditor;
using System.Collections;
using static UnityEngine.UIElements.UxmlAttributeDescription;

namespace ClawbearGames
{
    public class EditorTools : EditorWindow
    {
        [MenuItem("Tools/Capture Screenshot To Desktop")]
        public static void CaptureScreenshot_Desktop()
        {
            string path = "C:/Users/ADMIN/Desktop/icon.png";
            ScreenCapture.CaptureScreenshot(path);
        }
    }
}

