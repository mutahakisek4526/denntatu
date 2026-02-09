namespace AacV1.Services;

public static class ServiceLocator
{
    public static ScanService? ScanService { get; set; }
    public static DwellService? DwellService { get; set; }
}
