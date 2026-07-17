using Avalonia.Media.Imaging;

namespace WallpaperPicker.Models;

public class Wallpaper
{
    public string Id { get; set; }
    public Bitmap Image { get; set; }

    public Wallpaper(string id, string imagePath)
    {
        Id = id;
        Image = new Bitmap(imagePath);
    }
}