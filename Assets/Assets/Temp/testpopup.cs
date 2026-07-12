using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class WindowsPopup : MonoBehaviour
{
    // Import the native Windows USER32 DLL
    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern int MessageBoxW(IntPtr hWnd, string text, string caption, uint type);

    // Box Styles Flags
    private const uint MB_OK = 0x00000000;
    private const uint MB_ICONINFORMATION = 0x00000040;
    private const uint MB_ICONWARNING = 0x00000030;
    private const uint MB_ICONERROR = 0x00000010;

    void Start()
    {
        // Example: Trigger the native popup when the game starts
        ShowNativeMessage("System Alert", "This is an official Windows OS popup message!", MB_OK | MB_ICONINFORMATION);
    }

    public void ShowNativeMessage(string title, string message, uint type)
    {
        // IntPtr.Zero tells Windows to treat it as an independent desktop window
        MessageBoxW(IntPtr.Zero, message, title, type);
    }
}