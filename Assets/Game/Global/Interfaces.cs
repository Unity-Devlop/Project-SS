namespace Game
{
    public interface IGameEntry
    {
        public bool initialized { get; }
        public void OnInit();
    }
}