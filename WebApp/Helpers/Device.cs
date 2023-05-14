using Microsoft.JSInterop;
using System.Reflection;

namespace WebApp
{
    public class Device
    {
        public IJSRuntime JsRuntime { get; set; }
        public Device(IJSRuntime jsRuntime)
        {
            JsRuntime=jsRuntime;
        }
        public async Task<bool> IsMobile()
        {
            return await JsRuntime.InvokeAsync<bool>("IsMobile");
        }
    }
}
