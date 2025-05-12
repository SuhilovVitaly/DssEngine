namespace DeepSpaceSaga.Common.Tools;

public class GenerationTool : IGenerationTool
{
    private Random _randomBase;

    public Random RandomBase { get; private set; } = new Random();

    public GenerationTool()
    {
        _randomBase = new Random((int)DateTime.UtcNow.Ticks);
    }

    public int GetInteger(int min = 0, int max = 0)
    {
        return _randomBase.Next(min, max);
    }

    public string GenerateCelestialObjectName()
    {
        return RandomString(4) + "-" + RandomNumber(4) + "-" + RandomNumber(3);
    }

    public double Direction()
    {
        return GetDouble(0, 359);
    }

    public double GetDouble(double minimum = 0, double maximum = 0)
    {
        return _randomBase.NextDouble() * (maximum - minimum) + minimum;
    }

    public float GetFloat(double minimum = 0, double maximum = 0)
    {
        return (float)(_randomBase.NextDouble() * (maximum - minimum) + minimum);
    }

    public float GetFloat( double maximum = 0)
    {
        return (float)(_randomBase.NextDouble() * maximum);
    }

    public int GetId()
    {
        return Math.Abs(Guid.NewGuid().GetHashCode());
    }

    public string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[_randomBase.Next(s.Length)]).ToArray());
    }

    public string RandomNumber(int length)
    {
        const string chars = "1234567890";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[_randomBase.Next(s.Length)]).ToArray());
    }
}

public interface IGenerationTool
{
    int GetInteger(int min = 0, int max = 0);
    string GenerateCelestialObjectName();
    double Direction();
    double GetDouble(double minimum = 0, double maximum = 0);
    float GetFloat(double minimum = 0, double maximum = 0);
    float GetFloat(double maximum = 0);
    int GetId();
    string RandomString(int length);
    string RandomNumber(int length);
    Random RandomBase { get; }
}

public static class IdGenerator
{
    private static int _currentId = 0; 
    private static readonly object _lock = new object(); 

    public static int GetNextId()
    {
        lock (_lock)
        {
            return ++_currentId;
        }
    }
}
