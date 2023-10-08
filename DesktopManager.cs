using System.Runtime.InteropServices;

public static class DesktopManager
{
    [DllImport("shell32.dll")]
    public static extern int SHSetKnownFolderPath(ref Guid folderId, uint flags, IntPtr token, [MarshalAs(UnmanagedType.LPWStr)] string path);

    public static int GetCurrentVirtualDesktop()
    {
        return VirtualDesktop.GetCurrentDesktopNumber();
    }

    public static void SubscribeVirtualDesktopChange(Action<int> desktopChangedCallback)
    {
        int currentDesktop = GetCurrentVirtualDesktop();

        while (true)
        {
            int newDesktop = GetCurrentVirtualDesktop();
            if (newDesktop != currentDesktop)
            {
                currentDesktop = newDesktop;
                desktopChangedCallback(currentDesktop);
            }
            System.Threading.Thread.Sleep(1000);
        }
    }
}