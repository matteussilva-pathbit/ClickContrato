using Microsoft.JSInterop;

namespace ClickContrato.Web.Services.Storage;

public sealed class BrowserLocalStorage : IKeyValueStorage
{
    private readonly IJSRuntime _js;
    public BrowserLocalStorage(IJSRuntime js) => _js = js;

    public ValueTask SetAsync(string key, string value) =>
        _js.InvokeVoidAsync("ccStorage.set", key, value);

    public ValueTask<string?> GetAsync(string key) =>
        _js.InvokeAsync<string?>("ccStorage.get", key);

    public ValueTask RemoveAsync(string key) =>
        _js.InvokeVoidAsync("ccStorage.remove", key);
}


