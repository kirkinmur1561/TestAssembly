namespace Common
{
    public interface IModel
    {
        string Name { get; }
        string Description { get; }
        int Exe();
    }
}