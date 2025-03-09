namespace DataEncoding.Interfaces
{
    public interface ISupportsDecode<T>
    {
        int Decode(T input);
    }
}
