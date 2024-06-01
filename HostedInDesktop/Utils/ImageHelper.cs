using Google.Protobuf;
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

    public static async Task<ByteString> ConvertImageSourceToByteString(ImageSource imageSource)
    {
        if (imageSource is StreamImageSource streamImageSource)
        {
            using var stream = await streamImageSource.Stream(CancellationToken.None);
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            byte[] imageBytes = memoryStream.ToArray();
            return ByteString.CopyFrom(imageBytes);
        }
        else if (imageSource is FileImageSource fileImageSource)
        {
            string filePath = fileImageSource.File;
            byte[] imageBytes = await File.ReadAllBytesAsync(filePath);
            return ByteString.CopyFrom(imageBytes);
        }
        else if (imageSource is UriImageSource uriImageSource)
        {
            using var httpClient = new HttpClient();
            byte[] imageBytes = await httpClient.GetByteArrayAsync(uriImageSource.Uri);
            return ByteString.CopyFrom(imageBytes);
        }
        else
        {
            throw new NotSupportedException("Unsupported ImageSource type");
        }
    }

    public static ByteString[] ConvertPathToByteString(string path)
    {
        ByteString[] byteStringArray = null;

        if (path != null)
        {
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            ByteString byteString = ByteString.CopyFrom(bytes);
            byteStringArray = new ByteString[] { byteString };
        }

        return byteStringArray;
    }

}
