using System;
using System.Globalization;
using System.IO;
using System.Linq;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using MetadataExtractor.Formats.QuickTime;
using Directory = System.IO.Directory;

namespace SimpleExifSorter
{
    class Program
    {

        private const string PicturePath = @"\\Desktop-bth3i9b\2016-10-25";
        private const string VideoPath = @"\\Desktop-bth3i9b\2016-10-25";

        private const string PictureDestinationPath = @"\\DESKTOP-BTH3I9B\TempPic";
        private const string VideoDestinationPath = @"\\DESKTOP-BTH3I9B\TempVid";

        private static void Main()
        {
            var allVideos = Directory.EnumerateFiles(VideoPath)
                .Where(file => file.ToLower().EndsWith(".mov")) // file.ToLower().EndsWith(".avi") ||
                .ToList();
            foreach (var videoFile in allVideos)
            {
                HandleFile(videoFile, true, VideoDestinationPath);
            }

            var allPictures = Directory.EnumerateFiles(PicturePath)
                .Where(file => file.ToLower().EndsWith(".jpg"))
                .ToList();

            foreach (var picture in allPictures)
            {
                HandleFile(picture, false, PictureDestinationPath);
            }
        }

        private static void HandleFile(string file, bool isVideo, string destinationPath)
        {
            var date = isVideo ? GetVideoCreatedDate(file) : GetImageCreatedDate(file);
            var path = date.HasValue
                ? Path.Combine(destinationPath, date.Value.Year.ToString(),
                    date.Value.Month + "-" + date.Value.ToString("MMMM", CultureInfo.InvariantCulture))
                : Path.Combine(PictureDestinationPath, "no-date");

            CopyFile(path, file.Substring(file.LastIndexOf('\\') + 1), file);
            Console.WriteLine(file + "\t" + date);
        }
        private static void CopyFile(string copyPath, string fileName, string sourceFile)
        {
            if (!Directory.Exists(copyPath))
            {
                Directory.CreateDirectory(copyPath);
            }
            var newFile = Path.Combine(copyPath, fileName);
            if (!File.Exists(newFile))
            {
                File.Copy(sourceFile, newFile, true);
            }
            Console.WriteLine(sourceFile + " => " + Path.Combine(copyPath, fileName));

        }

        private static DateTime? GetVideoCreatedDate(string file)
        {
            var directories = ImageMetadataReader.ReadMetadata(file);
            var header = directories.OfType<QuickTimeMovieHeaderDirectory>().FirstOrDefault();
            return header?.GetDateTime(QuickTimeMovieHeaderDirectory.TagCreated);
        }

        private static DateTime? GetImageCreatedDate(string file)
        {
            var directories = ImageMetadataReader.ReadMetadata(file);
            var subIfdDirectory = directories.OfType<ExifSubIfdDirectory>().FirstOrDefault();
            return subIfdDirectory?.GetDateTime(ExifDirectoryBase.TagDateTimeOriginal);
        }

        //private static DateTime? GetJpgCreatedDate2(string file)
        //{
        //    using (Image image = new Bitmap(file))
        //    {
        //        var propertyItem = image.PropertyItems.FirstOrDefault(a => a.Id.ToString("x") == "9004");
        //        if (propertyItem?.Value == null)
        //        {
        //            return null;
        //        }
        //        var originalDateString = Encoding.UTF8.GetString(propertyItem.Value);
        //        originalDateString = originalDateString.Remove(originalDateString.Length - 1);
        //        DateTime originalDate;
        //        try
        //        {
        //            originalDate = DateTime.ParseExact(originalDateString, "yyyy:MM:dd HH:mm:ss", null);
        //        }
        //        catch (FormatException)
        //        {
        //            originalDate = File.GetCreationTime(file);
        //        }

        //        return originalDate;
        //    }
        //}

        //const int BytesToRead = sizeof(long);

        //static bool FilesAreEqual(FileInfo first, FileInfo second)
        //{
        //    if (first.Length != second.Length)
        //        return false;

        //    var iterations = (int)Math.Ceiling((double)first.Length / BytesToRead);

        //    using (var fs1 = first.OpenRead())
        //    using (var fs2 = second.OpenRead())
        //    {
        //        var one = new byte[BytesToRead];
        //        var two = new byte[BytesToRead];

        //        for (var i = 0; i < iterations; i++)
        //        {
        //            fs1.Read(one, 0, BytesToRead);
        //            fs2.Read(two, 0, BytesToRead);

        //            if (BitConverter.ToInt64(one, 0) != BitConverter.ToInt64(two, 0))
        //                return false;
        //        }
        //    }
        //    return true;
        //}
    }
}