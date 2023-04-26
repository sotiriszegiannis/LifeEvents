using Microsoft.AspNetCore.Components;

namespace WebApp
{
    public abstract class EditorBase:ComponentBase
    {
        public abstract Task<Subscriber> Save();
    }
}
