using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;
using System.Diagnostics;

string imagesFolder = Path.Combine(Environment.CurrentDirectory, "images");

WriteLine($"I will look for images in the following folder {imagesFolder} \n");

if (!Directory.Exists(imagesFolder))
{
    WriteLine($"\n Folder does not exist!");
    return;
}

IEnumerable<string> images = Directory.EnumerateFiles(imagesFolder);
foreach (var imagePath in images)
{
	if (!Path.GetFileNameWithoutExtension(imagePath).EndsWith("thumbnail"))
	{
		var thumbnailPath = Path.Combine(
			Environment.CurrentDirectory,
			"images",
			$"{Path.GetFileNameWithoutExtension(imagePath)}-thumbnail{Path.GetExtension(imagePath)}"
		);

		WriteLine(thumbnailPath);

		FontFamily fontFamily;

        if (!SystemFonts.TryGet("Arial", out fontFamily))
            throw new Exception($"Couldn't find font 'Arial'");

        var font = fontFamily.CreateFont(15f, FontStyle.Regular);

		var waterMark = $"Created at {DateTime.Now.ToString("dd/MM/yyyy HH:mm")}";

		using (var image = Image.Load(imagePath))
		{
			WriteLine($"Converting {imagePath} to {thumbnailPath} \n");

			image.Mutate(_ => _.Resize(image.Width / 10, image.Height / 10)
								.Grayscale()
								.DrawText(waterMark, font, Color.Firebrick, new PointF(image.Width - 200, image.Height - 20)));


			image.Save(thumbnailPath);
		}
	}	
}

WriteLine("Image processing finished");
if (OperatingSystem.IsWindows())
{
    Process.Start("explorer.exe", imagesFolder);
}
