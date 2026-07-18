using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using WallpaperPicker.Models;
using WallpaperPicker.Services;

namespace WallpaperPicker.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public ObservableCollection<Wallpaper> Wallpapers { get; } = [];
    private Process? _currentWallpaperProcess;
    private Wallpaper? _selectedWallpaper;
    public Wallpaper? SelectedWallpaper
    {
        get => _selectedWallpaper;
        set
        {
            if (SetProperty(ref _selectedWallpaper, value) && value != null)
            {
                WallpaperClicked(value);
            }
        }
    }

    public MainViewModel()
    {
        var service = new WallpaperService();

        foreach (var wallpaper in service.LoadWallpapers())
        {
            Wallpapers.Add(wallpaper);
        }
    }

    private void WallpaperClicked(Wallpaper wallpaper)
    {
        if (_currentWallpaperProcess != null && !_currentWallpaperProcess.HasExited)
        {
            _currentWallpaperProcess.Kill();
            _currentWallpaperProcess.WaitForExit();
            _currentWallpaperProcess.Dispose();
        }

        _currentWallpaperProcess = Process.Start(new ProcessStartInfo
        {
            FileName = "linux-wallpaperengine",
            Arguments = $"--screen-root DP-2 --bg {wallpaper.Id}",
            UseShellExecute = false
        });
    }
}