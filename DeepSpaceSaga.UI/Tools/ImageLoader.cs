namespace DeepSpaceSaga.UI.Tools;

public class ImageLoader
{
    public static Image LoadLayersTacticalImage(string filename)
    {
        if (string.IsNullOrWhiteSpace(filename))
        {
            throw new ArgumentException("Filename cannot be null, empty, or whitespace.", nameof(filename));
        }

        // Path to file relative to project root
        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,  "Images", "Layers", "Tactical", filename + ".png");

        // Check if file exists
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("Blueprint image file not found.", filePath);
        }

        // Load the image
        return Image.FromFile(filePath);
    }

    public static Image LoadCharacterImage(string filename)
    {
        if (string.IsNullOrWhiteSpace(filename))
        {
            throw new ArgumentException("Filename cannot be null, empty, or whitespace.", nameof(filename));
        }

        // Path to file relative to project root
        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "Characters", filename);

        // Check if file exists
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("Blueprint image file not found.", filePath);
        }

        // Load the image
        return Image.FromFile(filePath);
    }

    public static Image LoadItemImage(string filename)
    {
        if (string.IsNullOrWhiteSpace(filename))
        {
            throw new ArgumentException("Filename cannot be null, empty, or whitespace.", nameof(filename));
        }

        filename = filename.Replace(".", "/");
        filename = filename + ".png";
        // Path to file relative to project root
        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", filename);

        // Check if file exists
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("Blueprint image file not found.", filePath);
        }

        // Load the image
        return Image.FromFile(filePath);
    }

    public static Image LoadImageByName(string imageName)
    {
        if (string.IsNullOrWhiteSpace(imageName))
        {
            throw new ArgumentException("Image name cannot be null, empty, or whitespace.", nameof(imageName));
        }

        // Add .png extension if not present
        if (!Path.HasExtension(imageName))
        {
            imageName += ".png";
        }

        // Path to Images directory
        string imagesDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");

        // Check if Images directory exists
        if (!Directory.Exists(imagesDirectory))
        {
            throw new DirectoryNotFoundException($"Images directory not found: {imagesDirectory}");
        }

        // Search for file in all subdirectories of Images folder
        string[] foundFiles = Directory.GetFiles(imagesDirectory, imageName, SearchOption.AllDirectories);

        if (foundFiles.Length == 0)
        {
            throw new FileNotFoundException($"Image file '{imageName}' not found in Images directory or its subdirectories.");
        }

        // If multiple files with same name found, take the first one
        if (foundFiles.Length > 1)
        {
            // Sort by path for predictable result
            Array.Sort(foundFiles);
        }

        // Load the image
        return Image.FromFile(foundFiles[0]);
    }
}
