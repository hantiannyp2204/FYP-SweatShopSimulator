public interface ISubscribeEvents<T>
{
    public void SubcribeEvents(T action);
    public void UnsubcribeEvents(T action);
}
