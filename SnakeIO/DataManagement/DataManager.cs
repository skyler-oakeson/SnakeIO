using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

/// <summary>
/// Class to manage the reading and writing of serializable data given.
/// Takes a Data<T> and loads from the filename generated.
/// </summary>
public class DataManager
{
    public bool saving {get; private set;}
    public bool loading {get; private set;}

    /// <summary>
    /// Returns an unwrapped loaded T. If failed returns unwrapped old T (Whatever was passed in as serializable to Data).
    /// </summary>
    public T Load<T>(T data)
    {
        Data<T> wrapper = new Data<T>(data);
        lock (this)
        {
            if (!this.loading)
            {
                this.loading = true;
                var result = FinalizeLoad(wrapper);
                result.Wait();
                return wrapper.serializable;
            }
        }
        return wrapper.serializable;
    }

    private async Task FinalizeLoad<T>(Data<T> data)
    {
        await Task.Run(() =>
        {
            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                try
                {
                    if (storage.FileExists(data.filename))
                    {
                        using (IsolatedStorageFileStream fs = storage.OpenFile(data.filename, FileMode.Open))
                        {
                            if (fs != null)
                            {
                                DataContractJsonSerializer mySerializer = new DataContractJsonSerializer(typeof(T));
                                T obj = (T)mySerializer.ReadObject(fs);
                                data.serializable = obj;
                            }
                        }
                    }
                }
                catch (IsolatedStorageException e)
                {
                    Console.WriteLine($"ERROR LOADING: {e}");
                }
            }

            this.loading = false;
        });
    }

    /// <summary>
    /// Saves to filename generated in Data<T>. Saves serializable to filename.
    /// </summary>
    public void Save<T>(T data)
    {
        Data<T> wrapper = new Data<T>(data);
        lock (this)
        {
            if (!this.saving)
            {
                this.saving = true;
                FinalizeSave(wrapper);
            }
        }
    }

    private async Task FinalizeSave<T>(Data<T> data)
    {
        await Task.Run(() =>
        {
            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                try
                {
                    using (IsolatedStorageFileStream fs = storage.OpenFile(data.filename, FileMode.Create))
                    {
                        if (fs != null)
                        {
                            DataContractJsonSerializer mySerializer = new DataContractJsonSerializer(typeof(T));
                            mySerializer.WriteObject(fs, data.serializable);
                        }
                    }
                }
                catch (IsolatedStorageException e)
                {
                    Console.WriteLine($"ERROR SAVING: {e}");
                }
            }

            this.saving = false;
        });
    }
}
