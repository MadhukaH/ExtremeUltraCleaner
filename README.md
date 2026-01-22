# 🚀 ExtremeUltraCleaner

**ExtremeUltraCleaner** is a high-performance Windows system cleaning utility built with **C# (.NET 8)** and **WPF**, designed for **power users, gamers, and developers** who want deep cleanup with maximum speed and safety.

> ⚡ Fast • 🛡️ Safe • 🎨 Modern UI • 🧠 Smart Performance

---

## ✨ Features

* 🔥 **Extreme Deep System Cleanup**
* 🎨 **Modern Dark UI (WPF / Fluent Style)**
* ⚡ **Async & Multi-Threaded Performance**
* 🛡️ **Administrator-Protected Operations**
* 📊 **Live Progress & Status Logging**
* 🧹 **Safe Cleanup with Smart Checks**
* 🎮 **Gaming & SSD Friendly**

---

## 🧹 Cleaning Capabilities

ExtremeUltraCleaner safely performs the following operations:

1. User & Windows Temp cleanup
2. Prefetch file cleanup
3. Windows Update cache removal
4. Delivery Optimization cleanup
5. Windows Error Reports removal
6. Recycle Bin cleanup (all drives)
7. Thumbnail cache cleanup
8. Browser cache cleanup (Chrome, Edge, Firefox, Brave, Opera*)
9. Log file cleanup (safe mode)
10. DNS cache flush
11. Windows.old removal (if exists)
12. Installer patch cache cleanup
13. Shadow copy & restore point removal (with warning)
14. Hibernation disable
15. Pagefile reset (recreated after reboot)
16. Silent Disk Cleanup execution

> ⚠️ Dangerous operations always require user confirmation.

---

## 🧠 Performance Design

* Uses **`async / await`** to prevent UI freezing
* Background execution for heavy disk operations
* Parallel execution where safe
* Smart path detection (skip if not exists)
* Minimal disk traversal
* Optimized file enumeration

---

## 🧱 Architecture

* **Framework:** .NET 8
* **UI:** WPF (XAML)
* **Pattern:** MVVM
* **Language:** C#
* **Execution:** Native (no BAT files)

```
ExtremeUltraCleaner/
│
├── UI/
│   ├── MainWindow.xaml
│   └── MainWindow.xaml.cs
│
├── ViewModels/
│   └── MainViewModel.cs
│
├── Services/
│   └── CleanerService.cs
│
├── Helpers/
│   └── AdminHelper.cs
│
├── App.xaml
└── ExtremeUltraCleaner.csproj
```

---

## 🔐 Administrator Requirement

This application **must run as Administrator** to perform deep system cleaning.

Administrator privileges are enforced via:

* Application manifest
* Runtime permission checks

---

## 🛡️ Safety Notes

* ❌ No registry cleaning (intentional)
* ❌ No random file deletion
* ✅ Only known system locations
* ✅ Auto-skip on permission errors
* ✅ Graceful error handling (no crashes)

---

## 📦 Build Instructions

1. Open project in **Visual Studio 2022+**
2. Select **Release | x64**
3. Build solution
4. Run as **Administrator**

---

## 📌 Requirements

* Windows 10 / 11
* .NET 8 Runtime
* Administrator access

---

## 📜 Disclaimer

This software performs **advanced system operations**.
Use at your own risk. The developer is **not responsible** for data loss or system instability caused by misuse.

---

## ⭐ Contribution

Pull requests, optimizations, and UI improvements are welcome.
Please follow clean code and MVVM standards.

---

## 📄 License

MIT License – free to use, modify, and distribute.

---

