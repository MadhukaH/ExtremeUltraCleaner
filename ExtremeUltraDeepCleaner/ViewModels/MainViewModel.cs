using ExtremeUltraDeepCleaner.Models;
using ExtremeUltraDeepCleaner.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace ExtremeUltraDeepCleaner.ViewModels
{
    /// <summary>
    /// Main ViewModel for the application
    /// </summary>
    public class MainViewModel : ViewModelBase, IProgressReporter
    {
        #region Private Fields

        private int _currentStep;
        private double _progressPercentage;
        private string _currentStatus = "Ready to clean";
        private bool _isCleaningInProgress;
        private bool _showSummary;
        private CleaningSummary _summary = new();
        private readonly CleanerService _cleanerService;

        #endregion

        #region Public Properties

        /// <summary>
        /// Current step number (1-18)
        /// </summary>
        public int CurrentStep
        {
            get => _currentStep;
            set => SetProperty(ref _currentStep, value);
        }

        /// <summary>
        /// Progress percentage (0-100)
        /// </summary>
        public double ProgressPercentage
        {
            get => _progressPercentage;
            set => SetProperty(ref _progressPercentage, value);
        }

        /// <summary>
        /// Current status message
        /// </summary>
        public string CurrentStatus
        {
            get => _currentStatus;
            set => SetProperty(ref _currentStatus, value);
        }

        /// <summary>
        /// Whether cleaning is currently in progress
        /// </summary>
        public bool IsCleaningInProgress
        {
            get => _isCleaningInProgress;
            set => SetProperty(ref _isCleaningInProgress, value);
        }

        /// <summary>
        /// Whether to show the summary panel
        /// </summary>
        public bool ShowSummary
        {
            get => _showSummary;
            set => SetProperty(ref _showSummary, value);
        }

        /// <summary>
        /// Cleaning summary statistics
        /// </summary>
        public CleaningSummary Summary
        {
            get => _summary;
            set => SetProperty(ref _summary, value);
        }

        /// <summary>
        /// Collection of log messages
        /// </summary>
        public ObservableCollection<LogEntry> LogMessages { get; } = new();

        /// <summary>
        /// Command to start cleaning process
        /// </summary>
        public ICommand StartCleaningCommand { get; }

        #endregion

        #region Constructor

        public MainViewModel()
        {
            _cleanerService = new CleanerService(this);
            StartCleaningCommand = new RelayCommand(
                execute: async () => await ExecuteCleaningAsync(),
                canExecute: () => !IsCleaningInProgress
            );
        }

        #endregion

        #region Command Handlers

        /// <summary>
        /// Executes the full cleaning process
        /// </summary>
        private async Task ExecuteCleaningAsync()
        {
            try
            {
                IsCleaningInProgress = true;
                ShowSummary = false;
                LogMessages.Clear();
                CurrentStep = 0;
                ProgressPercentage = 0;

                var stopwatch = Stopwatch.StartNew();
                LogMessage("🚀 Starting Extreme Ultra Deep Cleaning...", LogLevel.Info);

                // Kill Explorer for better file access
                LogMessage("Stopping Windows Explorer...", LogLevel.Info);
                await FileSystemHelper.KillExplorerAsync();

                // Execute all 18 tasks sequentially
                await _cleanerService.CleanUserTempAsync();
                await _cleanerService.CleanWindowsTempAsync();
                await _cleanerService.CleanPrefetchAsync();
                await _cleanerService.CleanWindowsUpdateCacheAsync();
                await _cleanerService.CleanDeliveryOptimizationAsync();
                await _cleanerService.CleanErrorReportsAsync();
                await _cleanerService.EmptyRecycleBinAsync();
                await _cleanerService.CleanThumbnailCacheAsync();
                await _cleanerService.CleanLogFilesAsync();
                await _cleanerService.CleanInternetCacheAsync();
                await _cleanerService.CleanBrowserCachesAsync();
                await _cleanerService.FlushDnsCacheAsync();
                await _cleanerService.CleanWindowsOldAsync();
                await _cleanerService.CleanInstallerCacheAsync();

                // Dangerous tasks - show warnings
                if (await ShowWarningDialog("Delete Shadow Copies/Restore Points?",
                    "⚠️ This will delete all system restore points.\nYou won't be able to restore your system to a previous state.\n\nContinue?"))
                {
                    await _cleanerService.DeleteShadowCopiesAsync();
                }
                else
                {
                    LogMessage("Shadow Copies deletion skipped by user", LogLevel.Warning);
                    CurrentStep = 15;
                    ProgressPercentage = (15.0 / 18.0) * 100;
                }

                if (await ShowWarningDialog("Disable Hibernation?",
                    "⚠️ This will disable hibernation and delete hiberfil.sys.\nYou won't be able to use hibernate mode.\n\nContinue?"))
                {
                    await _cleanerService.DisableHibernationAsync();
                }
                else
                {
                    LogMessage("Hibernation disable skipped by user", LogLevel.Warning);
                    CurrentStep = 16;
                    ProgressPercentage = (16.0 / 18.0) * 100;
                }

                if (await ShowWarningDialog("Reset Pagefile?",
                    "⚠️ This will delete and recreate the pagefile on next reboot.\nYour system may need to restart.\n\nContinue?"))
                {
                    await _cleanerService.ResetPagefileAsync();
                }
                else
                {
                    LogMessage("Pagefile reset skipped by user", LogLevel.Warning);
                    CurrentStep = 17;
                    ProgressPercentage = (17.0 / 18.0) * 100;
                }

                await _cleanerService.RunDiskCleanupAsync();

                // Restart Explorer
                LogMessage("Restarting Windows Explorer...", LogLevel.Info);
                await FileSystemHelper.StartExplorerAsync();

                stopwatch.Stop();

                // Show summary
                Summary = new CleaningSummary
                {
                    TotalFilesDeleted = _cleanerService.FilesDeleted,
                    TotalBytesFreed = _cleanerService.BytesFreed,
                    TimeTaken = stopwatch.Elapsed
                };

                CurrentStatus = "✅ Cleaning Completed!";
                ProgressPercentage = 100;
                ShowSummary = true;

                LogMessage($"✅ COMPLETE! Freed {Summary.SpaceFreedGB}, took {Summary.TimeTakenFormatted}", LogLevel.Success);
            }
            catch (Exception ex)
            {
                LogMessage($"❌ Error: {ex.Message}", LogLevel.Error);
            }
            finally
            {
                IsCleaningInProgress = false;
                
                // Ensure Explorer is running
                await FileSystemHelper.StartExplorerAsync();
            }
        }

        /// <summary>
        /// Shows a warning dialog and returns user's choice
        /// </summary>
        private async Task<bool> ShowWarningDialog(string title, string message)
        {
            return await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                var result = MessageBox.Show(
                    message,
                    title,
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning,
                    MessageBoxResult.No);

                return result == MessageBoxResult.Yes;
            });
        }

        #endregion

        #region IProgressReporter Implementation

        /// <summary>
        /// Reports progress updates from CleanerService
        /// </summary>
        public void ReportProgress(int step, string status)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                CurrentStep = step;
                CurrentStatus = status;
                ProgressPercentage = (step / 18.0) * 100;
            });
        }

        /// <summary>
        /// Logs a message to the UI
        /// </summary>
        public void LogMessage(string message, LogLevel level)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                LogMessages.Add(new LogEntry
                {
                    Timestamp = DateTime.Now,
                    Message = message,
                    Level = level
                });
            });
        }

        #endregion
    }
}
