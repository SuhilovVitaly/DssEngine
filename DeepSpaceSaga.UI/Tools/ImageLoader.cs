namespace DeepSpaceSaga.UI.Tools;

public class ImageLoader
{
    public static Image LoadLayersTacticalImage(string filename)
    {
        if (string.IsNullOrWhiteSpace(filename))
        {
            throw new ArgumentException("Filename cannot be null, empty, or whitespace.", nameof(filename));
        }

        // Путь к файлу относительно корня проекта
        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,  "Images", "Layers", "Tactical", filename + ".png");

        // Проверяем существование файла
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("Blueprint image file not found.", filePath);
        }

        // Загружаем изображение
        return Image.FromFile(filePath);
    }

    public static Image LoadCharacterImage(string filename)
    {
        if (string.IsNullOrWhiteSpace(filename))
        {
            throw new ArgumentException("Filename cannot be null, empty, or whitespace.", nameof(filename));
        }

        // Путь к файлу относительно корня проекта
        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "Characters", filename);

        // Проверяем существование файла
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("Blueprint image file not found.", filePath);
        }

        // Загружаем изображение
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
        // Путь к файлу относительно корня проекта
        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", filename);

        // Проверяем существование файла
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("Blueprint image file not found.", filePath);
        }

        // Загружаем изображение
        return Image.FromFile(filePath);
    }

    public static Image LoadImageByName(string imageName)
    {
        if (string.IsNullOrWhiteSpace(imageName))
        {
            throw new ArgumentException("Image name cannot be null, empty, or whitespace.", nameof(imageName));
        }

        // Добавляем расширение .png если его нет
        if (!Path.HasExtension(imageName))
        {
            imageName += ".png";
        }

        // Путь к папке Images
        string imagesDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");

        // Проверяем существование папки Images
        if (!Directory.Exists(imagesDirectory))
        {
            throw new DirectoryNotFoundException($"Images directory not found: {imagesDirectory}");
        }

        // Ищем файл во всех подпапках папки Images
        string[] foundFiles = Directory.GetFiles(imagesDirectory, imageName, SearchOption.AllDirectories);

        if (foundFiles.Length == 0)
        {
            throw new FileNotFoundException($"Image file '{imageName}' not found in Images directory or its subdirectories.");
        }

        // Если найдено несколько файлов с одинаковым именем, берем первый
        if (foundFiles.Length > 1)
        {
            // Сортируем по пути для предсказуемости результата
            Array.Sort(foundFiles);
        }

        // Загружаем изображение
        return Image.FromFile(foundFiles[0]);
    }
}
