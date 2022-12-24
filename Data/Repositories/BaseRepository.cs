using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text.Json;

namespace Data.Repositories;

public abstract class BaseRepository<T>
{
    private readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true
    };

    private readonly string _path;
    private Stream? _fs;


    protected BaseRepository(string path)
    {
        _path = path;
        EntitiesSubject = new BehaviorSubject<List<T>>(new List<T>());
        EntitiesSubject.OnNext(DeserializationJson());
        AsObservable = EntitiesSubject.AsObservable();
    }

    protected IObservable<List<T>> AsObservable { get; }
    private BehaviorSubject<List<T>> EntitiesSubject { get; }


    public abstract bool CompareEntities(T entity1, T entity2);

    protected void Change(T changedEntity)
    {
        var newEntities = new List<T>(EntitiesSubject.Value);
        foreach (var entity in EntitiesSubject.Value)
        {
            if (!CompareEntities(changedEntity, entity)) continue;
            newEntities.Remove(entity);
            newEntities.Add(changedEntity);
            SerializationJson(newEntities);
            break;
        }
    }

    protected void Remove(T delitingEmtity)
    {
        var newEntities = new List<T>(EntitiesSubject.Value);
        foreach (var entity in EntitiesSubject.Value)
        {
            if (!CompareEntities(delitingEmtity, entity)) continue;
            newEntities.Remove(entity);
            EntitiesSubject.OnNext(newEntities);
            SerializationJson(newEntities);
            break;
        }
    }

    private void SerializationJson(List<T>? entities)
    {
        if (entities is null) return;

        _fs = GetStream();
        JsonSerializer.Serialize(_fs, entities, _options);
        _fs.Close();
        EntitiesSubject.OnNext(entities);
    }

    protected void Append(T entity)
    {
        var newEntities = new List<T>(EntitiesSubject.Value);
        newEntities.Add(entity);
        SerializationJson(newEntities);
    }

    protected List<T> DeserializationJson()
    {
        if (EntitiesSubject.Value.Count != 0) return EntitiesSubject.Value;

        List<T>? deserialized = null;
        try
        {
            _fs = GetStream();
            deserialized = JsonSerializer.Deserialize<List<T>>(_fs, _options);
        }
        catch (Exception)
        {
            // ignored
        }
        finally
        {
            _fs?.Close();
            deserialized ??= new List<T>();
        }

        return deserialized;
    }

    private Stream GetStream()
    {
        return new FileStream
        (
            _path,
            FileMode.OpenOrCreate,
            FileAccess.ReadWrite,
            FileShare.ReadWrite,
            4096,
            FileOptions.None
        );
    }
}