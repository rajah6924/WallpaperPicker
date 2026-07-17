using System;
using System.Collections.Generic;
using System.IO;
using WallpaperPicker.Models;

namespace WallpaperPicker.Services;

public class WallpaperService
{
    public List<Wallpaper> LoadWallpapers()
    {
        const string rootPath = "/home/rajah_6924/.local/share/Steam/steamapps/workshop/content/431960/";
        List<Wallpaper> wallpaperList = new();
        
        if (!Directory.Exists(rootPath))
        {
            Console.WriteLine("Root directory not found");
            return wallpaperList;
        }
        string[] subFolders = Directory.GetDirectories(rootPath);

        foreach (string folder in subFolders)
        {
            try
            {
                string id = Path.GetFileNameWithoutExtension(folder);
                string imagePath = Path.Combine(folder, "preview.jpg");
                if (!File.Exists(imagePath))
                {
                    imagePath = Path.ChangeExtension(imagePath, ".gif");
                    if (!File.Exists(imagePath))
                    {
                        imagePath =
                            "/home/rajah_6924/RiderProjects/WallpaperPicker/WallpaperPicker/Assets/warningEmoji.svg";
                    }
                }
                Wallpaper wallpaper = new(id, imagePath);
                wallpaperList.Add(wallpaper);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        return wallpaperList;
    }
}