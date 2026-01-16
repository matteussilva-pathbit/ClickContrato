namespace ClickContrato.Web.Services.Storage;

public interface IKeyValueStorage
{
    ValueTask SetAsync(string key, string value);
    ValueTask<string?> GetAsync(string key);
    ValueTask RemoveAsync(string key);
}


