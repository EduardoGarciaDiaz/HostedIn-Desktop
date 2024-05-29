using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostedInDesktop.Utils;

public static class ImageHelper
{
    public static ImageSource ConvertToImageSource(byte[] imageBytes)
    {
        if (imageBytes == null || imageBytes.Length == 0)
        {
            return null;
        }

        return ImageSource.FromStream(() => new MemoryStream(imageBytes));
    }
    public static Image ConvertToImage(byte[] imageBytes)
    {
        if (imageBytes == null || imageBytes.Length == 0)
        {
            return null;
        }

        Image image = new Image();

        using (MemoryStream stream = new MemoryStream(imageBytes))
        {
            image.Source = ImageSource.FromStream(() => stream);
        }

        return image;
    }

    public static async Task<string> SaveVideoToFileAsync(byte[] videoBytes)
    {
        string fileName = $"video_{Guid.NewGuid()}.mp4";
        string filePath = Path.Combine(FileSystem.CacheDirectory, fileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        {
            await fileStream.WriteAsync(videoBytes, 0, videoBytes.Length);
        }

        return filePath;
    }
}
