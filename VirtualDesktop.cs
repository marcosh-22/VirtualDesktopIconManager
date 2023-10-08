using System.Runtime.InteropServices;

public static class VirtualDesktop
{
    [DllImport("VirtualDesktopAccessor.dll")]
    public static extern int GetCurrentDesktopNumber();

    [DllImport("VirtualDesktopAccessor.dll")]
    public static extern int GetDesktopCount();

    [DllImport("VirtualDesktopAccessor.dll")]
    public static extern Guid GetDesktopIdByNumber(int number);

    [DllImport("VirtualDesktopAccessor.dll")]
    public static extern int GetDesktopNumberById(Guid desktop_id);
}
