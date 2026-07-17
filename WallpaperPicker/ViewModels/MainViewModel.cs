using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using WallpaperPicker.Models;
using WallpaperPicker.Services;

namespace WallpaperPicker.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public ObservableCollection<Wallpaper> Wallpapers { get; } = [];

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
        // linux-wallpaperengine --screen-root <MONITOR_NAME> --bg <WALLPAPER_ID>   
        Process.Start(new ProcessStartInfo
        {
            FileName = "linux-wallpaperengine",
            Arguments = $"--screen-root DP-2 --bg {wallpaper.Id}",
            UseShellExecute = false
        });
    }
}