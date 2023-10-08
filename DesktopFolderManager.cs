public class DesktopFolderManager
{
    private const string DesktopsFolderName = "Desktops";
    private const string DesktopsInfoFileName = "desktops_info.txt";

    private readonly string desktopsFolderPath;
    private readonly string desktopsInfoFilePath;

    public DesktopFolderManager()
    {
        desktopsFolderPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            DesktopsFolderName
        );
        desktopsInfoFilePath = Path.Combine(desktopsFolderPath, DesktopsInfoFileName);

        if (!Directory.Exists(desktopsFolderPath))
        {
            Directory.CreateDirectory(desktopsFolderPath);
        }
    }

    public void SetDesktopFolder(Guid desktopGuid)
    {
        Dictionary<Guid, string> desktopInfo = LoadDesktopInfo();

        if (desktopInfo.ContainsKey(desktopGuid))
        {
            string folderName = desktopInfo[desktopGuid];
            string folderPath = Path.Combine(desktopsFolderPath, folderName);

            DesktopManager.SHSetKnownFolderPath(
                ref KnownFolder.Desktop,
                0,
                IntPtr.Zero,
                folderPath
            );
        }
        else
        {
            int newFolderNumber = GetNextFolderNumber();
            string newFolderName = $"Desktop_{newFolderNumber}";
            string newFolderPath = Path.Combine(desktopsFolderPath, newFolderName);

            Directory.CreateDirectory(newFolderPath);

            desktopInfo[desktopGuid] = newFolderName;
            SaveDesktopInfo(desktopInfo);

            DesktopManager.SHSetKnownFolderPath(
                ref KnownFolder.Desktop,
                0,
                IntPtr.Zero,
                newFolderPath
            );
        }
    }

    private Dictionary<Guid, string> LoadDesktopInfo()
    {
        Dictionary<Guid, string> desktopInfo = new Dictionary<Guid, string>();

        if (File.Exists(desktopsInfoFilePath))
        {
            foreach (string line in File.ReadAllLines(desktopsInfoFilePath))
            {
                string[] parts = line.Split(',');
                if (parts.Length == 2 && Guid.TryParse(parts[0], out Guid guid))
                {
                    desktopInfo[guid] = parts[1];
                }
            }
        }

        return desktopInfo;
    }

    private void SaveDesktopInfo(Dictionary<Guid, string> desktopInfo)
    {
        List<string> lines = new List<string>();
        foreach (var kvp in desktopInfo)
        {
            lines.Add($"{kvp.Key},{kvp.Value}");
        }
        File.WriteAllLines(desktopsInfoFilePath, lines);
    }

    private int GetNextFolderNumber()
    {
        int nextFolderNumber = 1;
        while (Directory.Exists(Path.Combine(desktopsFolderPath, $"Desktop_{nextFolderNumber}")))
        {
            nextFolderNumber++;
        }
        return nextFolderNumber;
    }
}