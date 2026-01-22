# Extreme Ultra Deep Cleaner - PRO

## 🚀 Quick Start

### Build & Run

```powershell
cd d:\GitHub_Project\ExtremeUltraCleaner\ExtremeUltraDeepCleaner

# Build
dotnet build -c Release

# Run (MUST be as Administrator)
cd bin\Release\net8.0-windows
.\ExtremeUltraDeepCleaner.exe
```

### Create Single-File Executable

```powershell
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

Output: `bin\Release\net8.0-windows\win-x64\publish\ExtremeUltraDeepCleaner.exe`

---

## ⚡ Features

✅ **18 Async Cleaning Tasks** - All BAT logic converted to C#  
✅ **Dark Gaming UI** - Custom title bar, circular + linear progress  
✅ **MVVM Architecture** - Clean separation of concerns  
✅ **Non-Blocking UI** - `async/await` throughout  
✅ **Safety Features** - Admin check + warning dialogs  
✅ **Real-time Logging** - Live status updates  
✅ **Summary Statistics** - Files deleted, space freed, time taken  

---

## 🛡️ Safety Warnings

> **Tasks 15-17 require confirmation:**
> - Shadow Copies deletion (restore points)
> - Hibernation disable (hiberfil.sys)
> - Pagefile reset (requires reboot)

---

## 📊 Project Structure

- `ViewModels/` - MainViewModel, ViewModelBase, RelayCommand
- `Views/` - MainWindow.xaml (dark theme UI)
- `Services/` - CleanerService (18 tasks), FileSystemHelper
- `Models/` - CleaningTask, CleaningSummary, LogEntry
- `Resources/Styles/` - DarkTheme.xaml

---

## 🎯 Requirements

- Windows 10/11
- .NET 8 SDK (for building)
- Administrator privileges (runtime)

---

## 📖 Full Documentation

See [walkthrough.md](file:///C:/Users/hetti/.gemini/antigravity/brain/2ced7f95-d90b-4cab-b244-733812937bee/walkthrough.md) for complete documentation.
