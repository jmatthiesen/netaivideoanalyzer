using System.Runtime.InteropServices;

public class SystemInfo
{
    public string OSDescription { get; set; }
    public string OSArchitecture { get; set; }
    public string ProcessArchitecture { get; set; }
    public string FrameworkDescription { get; set; }
    public string CPUInfo { get; set; }
    public string MemoryInfo { get; set; }

    public static async Task<SystemInfo> GetSystemInfoAsync()
    {
        // get system information, like operating system, processor, etc.
        var osDescription = RuntimeInformation.OSDescription;
        var osArchitecture = RuntimeInformation.OSArchitecture;
        var processArchitecture = RuntimeInformation.ProcessArchitecture;
        var frameworkDescription = RuntimeInformation.FrameworkDescription;

        var cpuInfo = string.Empty;
        var memInfo = string.Empty;
        // validate if running in Linux operating systems
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            cpuInfo = await File.ReadAllTextAsync("/proc/cpuinfo");
            memInfo = await File.ReadAllTextAsync("/proc/meminfo");
        }
        else
        {
            // get cpu and memory information for windows operating systems
            cpuInfo = "Not available in Windows";
            memInfo = "Not available in Windows";
        }

        return new SystemInfo
        {
            OSDescription = osDescription,
            OSArchitecture = osArchitecture.ToString(),
            ProcessArchitecture = processArchitecture.ToString(),
            FrameworkDescription = frameworkDescription,
            CPUInfo = cpuInfo,
            MemoryInfo = memInfo
        };
    }
}