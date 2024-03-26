/// <summary>
/// Wrapper class to manage the data being serialized and returned.
/// </summary>
public class Data<T>
{
    public string filename {get; private set;}
    public T serializable {get; set;}
    public Data(T serializable)
    {
        this.filename = $"{typeof(T).ToString().Replace("[]", "")}.json";
        this.serializable = serializable;
    }
}
