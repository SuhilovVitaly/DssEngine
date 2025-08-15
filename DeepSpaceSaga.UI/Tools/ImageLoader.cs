namespace DeepSpaceSaga.UI.Tools;

public class ImageLoader
{
    public static Image LoadLayersTacticalImage(string filename)
    {
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
}
