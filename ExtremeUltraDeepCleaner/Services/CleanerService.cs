using System.Diagnostics;
using System.IO;
using System.ServiceProcess;

namespace ExtremeUltraDeepCleaner.Services
{
    /// <summary>
    /// Progress reporter interface for cleaning operations
    /// </summary>
    public interface IProgressReporter
    {
        void ReportProgress(int step, string status);
        void LogMessage(string message, Models.LogLevel level);
    }

    /// <summary>
    /// Core cleaning service with all 18 cleaning tasks
    /// </summary>
    public class CleanerService
    {
        private readonly IProgressReporter _progressReporter;
        private int _filesDeleted = 0;
        private long _bytesFreed = 0;

        public int FilesDeleted => _filesDeleted;
        public long BytesFreed => _bytesFreed;

        public CleanerService(IProgressReporter progressReporter)
        {
            _progressReporter = progressReporter;
        }

        /// <summary>
        /// TASK 1: Clean User Temp folder
        /// </summary>
        public async Task CleanUserTempAsync()
        {
            _progressReporter.ReportProgress(1, "Cleaning User Temp...");
            await Task.Run(async () =>
            {
                try
                {
                    string tempPath = Path.GetTempPath();
                    if (FileSystemHelper.DirectoryExistsFast(tempPath))
                    {
                        long freed = await FileSystemHelper.SafeEmptyDirectoryAsync(tempPath);
                        _bytesFreed += freed;
                        _progressReporter.LogMessage($"User Temp cleaned: {freed / (1024 * 1024)} MB freed", Models.LogLevel.Success);
                    }
                }
                catch (Exception ex)
                {
                    _progressReporter.LogMessage($"User Temp: {ex.Message}", Models.LogLevel.Warning);
                }
            });
        }

        /// <summary>
        /// TASK 2: Clean Windows Temp folder
        /// </summary>
        public async Task CleanWindowsTempAsync()
        {
            _progressReporter.ReportProgress(2, "Cleaning Windows Temp...");
            await Task.Run(async () =>
            {
                try
                {
                    string winTemp = @"C:\Windows\Temp";
                    if (FileSystemHelper.DirectoryExistsFast(winTemp))
                    {
                        long freed = await FileSystemHelper.SafeEmptyDirectoryAsync(winTemp);
                        _bytesFreed += freed;
                        _progressReporter.LogMessage($"Windows Temp cleaned: {freed / (1024 * 1024)} MB freed", Models.LogLevel.Success);
                    }
                }
                catch (Exception ex)
                {
                    _progressReporter.LogMessage($"Windows Temp: {ex.Message}", Models.LogLevel.Warning);
                }
            });
        }

        /// <summary>
        /// TASK 3: Clean Prefetch
        /// </summary>
        public async Task CleanPrefetchAsync()
        {
            _progressReporter.ReportProgress(3, "Cleaning Prefetch...");
            await Task.Run(async () =>
            {
                try
                {
                    string prefetch = @"C:\Windows\Prefetch";
                    if (FileSystemHelper.DirectoryExistsFast(prefetch))
                    {
                        long freed = await FileSystemHelper.SafeEmptyDirectoryAsync(prefetch);
                        _bytesFreed += freed;
                        _progressReporter.LogMessage($"Prefetch cleaned: {freed / (1024 * 1024)} MB freed", Models.LogLevel.Success);
                    }
                }
                catch (Exception ex)
                {
                    _progressReporter.LogMessage($"Prefetch: {ex.Message}", Models.LogLevel.Warning);
                }
            });
        }

        /// <summary>
        /// TASK 4: Clean Windows Update Cache
        /// </summary>
        public async Task CleanWindowsUpdateCacheAsync()
        {
            _progressReporter.ReportProgress(4, "Cleaning Windows Update Cache...");
            await Task.Run(async () =>
            {
                try
                {
                    // Stop Windows Update service
                    await StopServiceAsync("wuauserv");
                    await Task.Delay(1000);

                    string downloadPath = @"C:\Windows\SoftwareDistribution\Download";
                    if (FileSystemHelper.DirectoryExistsFast(downloadPath))
                    {
                        long freed = await FileSystemHelper.SafeEmptyDirectoryAsync(downloadPath);
                        _bytesFreed += freed;
                        _progressReporter.LogMessage($"Update Cache cleaned: {freed / (1024 * 1024)} MB freed", Models.LogLevel.Success);
                    }

                    // Restart Windows Update service
                    await StartServiceAsync("wuauserv");
                }
                catch (Exception ex)
                {
                    _progressReporter.LogMessage($"Update Cache: {ex.Message}", Models.LogLevel.Warning);
                }
            });
        }

        /// <summary>
        /// TASK 5: Clean Delivery Optimization Files
        /// </summary>
        public async Task CleanDeliveryOptimizationAsync()
        {
            _progressReporter.ReportProgress(5, "Cleaning Delivery Optimization...");
            await Task.Run(() =>
            {
                try
                {
                    string doPath = @"C:\ProgramData\Microsoft\Network\Downloader";
                    if (Directory.Exists(doPath))
                    {
                        foreach (var file in Directory.EnumerateFiles(doPath, "qmgr*.dat"))
                        {
                            try
                            {
                                long size = new FileInfo(file).Length;
                                File.Delete(file);
                                _bytesFreed += size;
                                _filesDeleted++;
                            }
                            catch { }
                        }
                        _progressReporter.LogMessage("Delivery Optimization cleaned", Models.LogLevel.Success);
                    }
                }
                catch (Exception ex)
                {
                    _progressReporter.LogMessage($"Delivery Optimization: {ex.Message}", Models.LogLevel.Warning);
                }
            });
        }

        /// <summary>
        /// TASK 6: Clean Error Reports
        /// </summary>
        public async Task CleanErrorReportsAsync()
        {
            _progressReporter.ReportProgress(6, "Cleaning Error Reports...");
            await Task.Run(async () =>
            {
                try
                {
                    string werPath = @"C:\ProgramData\Microsoft\Windows\WER";
                    if (FileSystemHelper.DirectoryExistsFast(werPath))
                    {
                        long freed = await FileSystemHelper.SafeEmptyDirectoryAsync(werPath);
                        _bytesFreed += freed;
                        _progressReporter.LogMessage($"Error Reports cleaned: {freed / (1024 * 1024)} MB freed", Models.LogLevel.Success);
                    }
                }
                catch (Exception ex)
                {
                    _progressReporter.LogMessage($"Error Reports: {ex.Message}", Models.LogLevel.Warning);
                }
            });
        }

        /// <summary>
        /// TASK 7: Empty Recycle Bin (all drives)
        /// </summary>
        public async Task EmptyRecycleBinAsync()
        {
            _progressReporter.ReportProgress(7, "Emptying Recycle Bins...");
            await Task.Run(async () =>
            {
                try
                {
                    char[] drives = { 'C', 'D', 'E', 'F', 'G' };
                    foreach (char drive in drives)
                    {
                        string recycleBin = $@"{drive}:\$Recycle.Bin";
                        if (FileSystemHelper.DirectoryExistsFast(recycleBin))
                        {
                            long freed = await FileSystemHelper.SafeDeleteDirectoryAsync(recycleBin);
                            _bytesFreed += freed;
                        }
                    }
                    _progressReporter.LogMessage("Recycle Bins emptied", Models.LogLevel.Success);
                }
                catch (Exception ex)
                {
                    _progressReporter.LogMessage($"Recycle Bin: {ex.Message}", Models.LogLevel.Warning);
                }
            });
        }

        /// <summary>
        /// TASK 8: Clean Thumbnail Cache
        /// </summary>
        public async Task CleanThumbnailCacheAsync()
        {
            _progressReporter.ReportProgress(8, "Cleaning Thumbnail Cache...");
            await Task.Run(() =>
            {
                try
                {
                    string explorerPath = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        @"Microsoft\Windows\Explorer");

                    if (Directory.Exists(explorerPath))
                    {
                        foreach (var file in Directory.EnumerateFiles(explorerPath, "thumbcache*"))
                        {
                            try
                            {
                                long size = new FileInfo(file).Length;
                                File.Delete(file);
                                _bytesFreed += size;
                                _filesDeleted++;
                            }
                            catch { }
                        }
                        _progressReporter.LogMessage("Thumbnail Cache cleaned", Models.LogLevel.Success);
                    }
                }
                catch (Exception ex)
                {
                    _progressReporter.LogMessage($"Thumbnail Cache: {ex.Message}", Models.LogLevel.Warning);
                }
            });
        }

        /// <summary>
        /// TASK 9: Clean Log Files
        /// </summary>
        public async Task CleanLogFilesAsync()
        {
            _progressReporter.ReportProgress(9, "Cleaning Log Files...");
            await Task.Run(() =>
            {
                try
                {
                    int count = 0;
                    long freed = 0;

                    // Only scan common log locations to avoid excessive scanning
                    string[] logPaths = {
                        @"C:\Windows\Logs",
                        @"C:\Windows\System32\LogFiles",
                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Temp")
                    };

                    foreach (var basePath in logPaths)
                    {
                        if (!Directory.Exists(basePath)) continue;

                        foreach (var file in Directory.EnumerateFiles(basePath, "*.log", SearchOption.AllDirectories))
                        {
                            try
                            {
                                long size = new FileInfo(file).Length;
                                File.Delete(file);
                                freed += size;
                                count++;
                            }
                            catch { }
                        }
                    }

                    _bytesFreed += freed;
                    _filesDeleted += count;
                    _progressReporter.LogMessage($"Log Files cleaned: {count} files, {freed / (1024 * 1024)} MB", Models.LogLevel.Success);
                }
                catch (Exception ex)
                {
                    _progressReporter.LogMessage($"Log Files: {ex.Message}", Models.LogLevel.Warning);
                }
            });
        }

        /// <summary>
        /// TASK 10: Clean Internet/Edge Cache
        /// </summary>
        public async Task CleanInternetCacheAsync()
        {
            _progressReporter.ReportProgress(10, "Cleaning Internet Cache...");
            await Task.Run(async () =>
            {
                try
                {
                    string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                    
                    string[] cachePaths = {
                        Path.Combine(localAppData, @"Microsoft\Windows\INetCache"),
                        Path.Combine(localAppData, @"Microsoft\Windows\WebCache")
                    };

                    long totalFreed = 0;
                    foreach (var cachePath in cachePaths)
                    {
                        if (FileSystemHelper.DirectoryExistsFast(cachePath))
                        {
                            long freed = await FileSystemHelper.SafeDeleteDirectoryAsync(cachePath);
                            totalFreed += freed;
                        }
                    }

                    _bytesFreed += totalFreed;
                    _progressReporter.LogMessage($"Internet Cache cleaned: {totalFreed / (1024 * 1024)} MB freed", Models.LogLevel.Success);
                }
                catch (Exception ex)
                {
                    _progressReporter.LogMessage($"Internet Cache: {ex.Message}", Models.LogLevel.Warning);
                }
            });
        }

        /// <summary>
        /// TASK 11: Clean Browser Caches
        /// </summary>
        public async Task CleanBrowserCachesAsync()
        {
            _progressReporter.ReportProgress(11, "Cleaning Browser Caches...");
            await Task.Run(async () =>
            {
                try
                {
                    string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                    string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

                    string[] browserCaches = {
                        Path.Combine(localAppData, @"Google\Chrome\User Data\Default\Cache"),
                        Path.Combine(localAppData, @"Google\Chrome\User Data\Default\Code Cache"),
                        Path.Combine(localAppData, @"BraveSoftware\Brave-Browser\User Data\Default\Cache"),
                        Path.Combine(localAppData, @"BraveSoftware\Brave-Browser\User Data\Default\Code Cache"),
                        Path.Combine(appData, @"Mozilla\Firefox\Profiles"),
                        Path.Combine(appData, @"Opera Software\Opera Stable\Cache"),
                        Path.Combine(appData, @"Opera Software\Opera GX Stable\Cache")
                    };

                    long totalFreed = 0;
                    foreach (var cachePath in browserCaches)
                    {
                        if (FileSystemHelper.DirectoryExistsFast(cachePath))
                        {
                            long freed = await FileSystemHelper.SafeDeleteDirectoryAsync(cachePath);
                            totalFreed += freed;
                        }
                    }

                    _bytesFreed += totalFreed;
                    _progressReporter.LogMessage($"Browser Caches cleaned: {totalFreed / (1024 * 1024)} MB freed", Models.LogLevel.Success);
                }
                catch (Exception ex)
                {
                    _progressReporter.LogMessage($"Browser Caches: {ex.Message}", Models.LogLevel.Warning);
                }
            });
        }

        /// <summary>
        /// TASK 12: Flush DNS Cache
        /// </summary>
        public async Task FlushDnsCacheAsync()
        {
            _progressReporter.ReportProgress(12, "Flushing DNS Cache...");
            await Task.Run(() =>
            {
                try
                {
                    var psi = new ProcessStartInfo
                    {
                        FileName = "ipconfig",
                        Arguments = "/flushdns",
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        RedirectStandardOutput = true
                    };

                    var process = Process.Start(psi);
                    process?.WaitForExit();
                    
                    _progressReporter.LogMessage("DNS Cache flushed", Models.LogLevel.Success);
                }
                catch (Exception ex)
                {
                    _progressReporter.LogMessage($"DNS Flush: {ex.Message}", Models.LogLevel.Warning);
                }
            });
        }

        /// <summary>
        /// TASK 13: Clean Windows.old
        /// </summary>
        public async Task CleanWindowsOldAsync()
        {
            _progressReporter.ReportProgress(13, "Cleaning Windows.old...");
            await Task.Run(async () =>
            {
                try
                {
                    string windowsOld = @"C:\Windows.old";
                    if (FileSystemHelper.DirectoryExistsFast(windowsOld))
                    {
                        long freed = await FileSystemHelper.SafeDeleteDirectoryAsync(windowsOld);
                        _bytesFreed += freed;
                        _progressReporter.LogMessage($"Windows.old deleted: {freed / (1024 * 1024 * 1024)} GB freed", Models.LogLevel.Success);
                    }
                    else
                    {
                        _progressReporter.LogMessage("Windows.old not found (already clean)", Models.LogLevel.Info);
                    }
                }
                catch (Exception ex)
                {
                    _progressReporter.LogMessage($"Windows.old: {ex.Message}", Models.LogLevel.Warning);
                }
            });
        }

        /// <summary>
        /// TASK 14: Clean Installer Cache
        /// </summary>
        public async Task CleanInstallerCacheAsync()
        {
            _progressReporter.ReportProgress(14, "Cleaning Installer Cache...");
            await Task.Run(async () =>
            {
                try
                {
                    string installerCache = @"C:\Windows\Installer\$PatchCache$";
                    if (FileSystemHelper.DirectoryExistsFast(installerCache))
                    {
                        long freed = await FileSystemHelper.SafeDeleteDirectoryAsync(installerCache);
                        _bytesFreed += freed;
                        _progressReporter.LogMessage($"Installer Cache cleaned: {freed / (1024 * 1024)} MB freed", Models.LogLevel.Success);
                    }
                }
                catch (Exception ex)
                {
                    _progressReporter.LogMessage($"Installer Cache: {ex.Message}", Models.LogLevel.Warning);
                }
            });
        }

        /// <summary>
        /// TASK 15: Delete Shadow Copies/Restore Points ⚠️ DANGEROUS
        /// </summary>
        public async Task DeleteShadowCopiesAsync()
        {
            _progressReporter.ReportProgress(15, "Deleting Shadow Copies...");
            await Task.Run(() =>
            {
                try
                {
                    var psi = new ProcessStartInfo
                    {
                        FileName = "vssadmin",
                        Arguments = "delete shadows /for=C: /all /quiet",
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        Verb = "runas"
                    };

                    var process = Process.Start(psi);
                    process?.WaitForExit();
                    
                    _progressReporter.LogMessage("Shadow Copies deleted", Models.LogLevel.Success);
                }
                catch (Exception ex)
                {
                    _progressReporter.LogMessage($"Shadow Copies: {ex.Message}", Models.LogLevel.Error);
                }
            });
        }

        /// <summary>
        /// TASK 16: Disable Hibernation ⚠️ DANGEROUS
        /// </summary>
        public async Task DisableHibernationAsync()
        {
            _progressReporter.ReportProgress(16, "Disabling Hibernation...");
            await Task.Run(() =>
            {
                try
                {
                    var psi = new ProcessStartInfo
                    {
                        FileName = "powercfg",
                        Arguments = "-h off",
                        CreateNoWindow = true,
                        UseShellExecute = false
                    };

                    var process = Process.Start(psi);
                    process?.WaitForExit();
                    
                    _progressReporter.LogMessage("Hibernation disabled (hiberfil.sys deleted)", Models.LogLevel.Success);
                }
                catch (Exception ex)
                {
                    _progressReporter.LogMessage($"Hibernation: {ex.Message}", Models.LogLevel.Error);
                }
            });
        }

        /// <summary>
        /// TASK 17: Reset Pagefile ⚠️ DANGEROUS
        /// </summary>
        public async Task ResetPagefileAsync()
        {
            _progressReporter.ReportProgress(17, "Resetting Pagefile...");
            await Task.Run(() =>
            {
                try
                {
                    var psi = new ProcessStartInfo
                    {
                        FileName = "wmic",
                        Arguments = @"pagefileset where name=""C:\\pagefile.sys"" delete",
                        CreateNoWindow = true,
                        UseShellExecute = false
                    };

                    var process = Process.Start(psi);
                    process?.WaitForExit();
                    
                    _progressReporter.LogMessage("Pagefile reset (will recreate on reboot)", Models.LogLevel.Success);
                }
                catch (Exception ex)
                {
                    _progressReporter.LogMessage($"Pagefile: {ex.Message}", Models.LogLevel.Error);
                }
            });
        }

        /// <summary>
        /// TASK 18: Run Disk Cleanup
        /// </summary>
        public async Task RunDiskCleanupAsync()
        {
            _progressReporter.ReportProgress(18, "Running Disk Cleanup...");
            await Task.Run(() =>
            {
                try
                {
                    var psi = new ProcessStartInfo
                    {
                        FileName = "cleanmgr",
                        Arguments = "/sagerun:1",
                        CreateNoWindow = true,
                        UseShellExecute = false
                    };

                    var process = Process.Start(psi);
                    // Don't wait for cleanmgr as it may take a while
                    
                    _progressReporter.LogMessage("Disk Cleanup launched", Models.LogLevel.Success);
                }
                catch (Exception ex)
                {
                    _progressReporter.LogMessage($"Disk Cleanup: {ex.Message}", Models.LogLevel.Warning);
                }
            });
        }

        #region Helper Methods

        /// <summary>
        /// Safely stops a Windows service
        /// </summary>
        private async Task<bool> StopServiceAsync(string serviceName)
        {
            return await Task.Run(() =>
            {
                try
                {
                    using var service = new ServiceController(serviceName);
                    if (service.Status == ServiceControllerStatus.Running)
                    {
                        service.Stop();
                        service.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(30));
                        return true;
                    }
                    return false;
                }
                catch
                {
                    return false;
                }
            });
        }

        /// <summary>
        /// Safely starts a Windows service
        /// </summary>
        private async Task<bool> StartServiceAsync(string serviceName)
        {
            return await Task.Run(() =>
            {
                try
                {
                    using var service = new ServiceController(serviceName);
                    if (service.Status == ServiceControllerStatus.Stopped)
                    {
                        service.Start();
                        service.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(30));
                        return true;
                    }
                    return false;
                }
                catch
                {
                    return false;
                }
            });
        }

        #endregion
    }
}
