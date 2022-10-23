using UnityEngine;

public class PeachColor : MonoBehaviour
{
    // Peach made me do it!
    private static string[] Colors = new string[] { "#c51111", "#123ed1", "#117f2d", "#ed54ba", "#ef7d0d", "#f5f557", "#3f474e", "#d650f0", "#6b2fbb", "#71491e", "#34fedc", "#50ef39" };

    private void Start()
    {
        if (ColorUtility.TryParseHtmlString(Colors[Random.Range(0, Colors.Length)], out var color)) {
            foreach(var renderer in GetComponentsInChildren<Renderer>()) {
                renderer.material.color = color;
            }
        }
    }
}
