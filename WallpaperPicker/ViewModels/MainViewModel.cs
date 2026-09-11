using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WallpaperPicker.Models;
using WallpaperPicker.Services;

namespace WallpaperPicker.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public ObservableCollection<Wallpaper> Wallpapers { get; } = [];
    private Process? _currentWallpaperProcess;
    private Wallpaper? _selectedWallpaper;
    public IRelayCommand StopWallpaperCommand { get; }

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
        StopWallpaperCommand = new RelayCommand(StopWallpaper);

        var service = new WallpaperService();

        foreach (var wallpaper in service.LoadWallpapers())
        {
            Wallpapers.Add(wallpaper);
        }
    }

    private void WallpaperClicked(Wallpaper wallpaper)
    {
        StopWallpaper();

        _currentWallpaperProcess = Process.Start(new ProcessStartInfo
        {
            FileName = "linux-wallpaperengine",
            Arguments = $"--screen-root DP-2 --bg {wallpaper.Id}",
            UseShellExecute = false
        });
    }

    private void StopWallpaper()
    {
        if (_currentWallpaperProcess is not { } process)
        {
            return;
        }

        if (!process.HasExited)
        {
            process.Kill();
            process.WaitForExit();
        }

        process.Dispose();
        _currentWallpaperProcess = null;
    }
}