namespace DeepSpaceSaga.Common.Implementation
{
    public class GameSessionDTO
    {
        public Guid Id { get; set; }
        public int Turn { get; set; }
        public List<int> SpaceMap { get; set; }
    }
}
