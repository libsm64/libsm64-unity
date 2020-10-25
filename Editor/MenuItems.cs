using UnityEngine;
using UnityEditor;

namespace LibSM64.Editor
{
    static public class MenuItems
    {
        [MenuItem("GameObject/Create SM64/Mario")] // , false, -100)]
        static void menu_hello()
        {
            Debug.Log("Hello");
        }
    }
}
