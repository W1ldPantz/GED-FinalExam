namespace DrawingSystem
{
    public interface ICanvasCommand
    {
        void Redo();
        void Undo();
    }
}