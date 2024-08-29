using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components;
using System.Diagnostics;

namespace RenderingTest.Client.Components;

public abstract class TimedComponentBase : IComponent, IHandleEvent, IHandleAfterRender
{
    protected RenderFragment componentRenderFragment;
    private RenderHandle _renderHandle;
    protected bool initialized;
    protected bool hasNeverRendered = true;
    protected bool hasPendingQueuedRender;
    private bool _hasCalledOnAfterRender;

    protected virtual string ComponentName => GetType().Name;

    public TimedComponentBase()
    {
        componentRenderFragment = builder =>
        {
            Console.WriteLine($"{ComponentName} UI started a render at {GetTime()}");
            var timer = Stopwatch.StartNew();
            hasPendingQueuedRender = false;
            hasNeverRendered = false;
            BuildComponent(builder);
            Console.WriteLine($"{ComponentName} rendered in {timer.Elapsed}");
        };
    }

    protected virtual void BuildComponent(RenderTreeBuilder builder)
        => BuildRenderTree(builder);

    protected virtual void BuildRenderTree(RenderTreeBuilder builder) { }

    protected virtual void OnInitialized() { }

    protected virtual Task OnInitializedAsync() => Task.CompletedTask;

    protected virtual void OnParametersSet() { }

    protected virtual Task OnParametersSetAsync() => Task.CompletedTask;

    protected void StateHasChanged()
        => _renderHandle.Dispatcher.InvokeAsync(Render);

    internal protected void Render()
    {
        if (hasPendingQueuedRender)
            return;

        if (hasNeverRendered || ShouldRender() || _renderHandle.IsRenderingOnMetadataUpdate)
        {
            hasPendingQueuedRender = true;

            try
            {
                _renderHandle.Render(componentRenderFragment);
            }
            catch
            {
                hasPendingQueuedRender = false;
                throw;
            }
        }
    }

    protected virtual bool ShouldRender() => true;

    protected virtual void OnAfterRender(bool firstRender) { }

    protected virtual Task OnAfterRenderAsync(bool firstRender) => Task.CompletedTask;

    protected Task InvokeAsync(Action workItem)
        => _renderHandle.Dispatcher.InvokeAsync(workItem);

    protected Task InvokeAsync(Func<Task> workItem)
        => _renderHandle.Dispatcher.InvokeAsync(workItem);

    void IComponent.Attach(RenderHandle renderHandle)
    {
        if (_renderHandle.IsInitialized)
            throw new InvalidOperationException($"The render handle is already set. Cannot initialize a {nameof(ComponentBase)} more than once.");

        _renderHandle = renderHandle;
    }

    public virtual async Task SetParametersAsync(ParameterView parameters)
    {
        Console.WriteLine($"{ComponentName} UI started a lifecycle at {GetTime()}");
        var timer = Stopwatch.StartNew();
        parameters.SetParameterProperties(this);

        if (!initialized)
        {
            initialized = true;

            await RunInitAndSetParametersAsync();
        }
        else
            await CallOnParametersSetAsync();

        Console.WriteLine($"{ComponentName} lifefcyled in {timer.Elapsed}");
    }

    private async Task RunInitAndSetParametersAsync()
    {
        OnInitialized();

        var task = OnInitializedAsync();

        if (task.Status != TaskStatus.RanToCompletion && task.Status != TaskStatus.Canceled)
        {
            Render();

            try
            {
                await task;
            }
            catch // avoiding exception filters for AOT runtime support
            {
                if (!task.IsCanceled)
                    throw;
            }
        }

        await CallOnParametersSetAsync();
    }

    private Task CallOnParametersSetAsync()
    {
        OnParametersSet();

        var task = OnParametersSetAsync();
        var shouldAwaitTask = task.Status != TaskStatus.RanToCompletion &&
            task.Status != TaskStatus.Canceled;

        Render();

        return shouldAwaitTask ?
            CallStateHasChangedOnAsyncCompletion(task) :
            Task.CompletedTask;
    }

    private async Task CallStateHasChangedOnAsyncCompletion(Task task)
    {
        try
        {
            await task;
        }
        catch
        {
            if (task.IsCanceled)
                return;

            throw;
        }
        Render();
    }

    async Task IHandleEvent.HandleEventAsync(EventCallbackWorkItem callback, object? arg)
    {
        Console.WriteLine($"{ComponentName} UI started an event at {GetTime()}");
        var timer = Stopwatch.StartNew();

        var task = callback.InvokeAsync(arg);
        var shouldAwaitTask = task.Status != TaskStatus.RanToCompletion && task.Status != TaskStatus.Canceled;

        Render();

        if (shouldAwaitTask)
            await CallStateHasChangedOnAsyncCompletion(task);

        Console.WriteLine($"{ComponentName} UI evented in {timer.Elapsed}");
    }

    Task IHandleAfterRender.OnAfterRenderAsync()
    {
        var firstRender = !_hasCalledOnAfterRender;
        _hasCalledOnAfterRender |= true;

        OnAfterRender(firstRender);

        return OnAfterRenderAsync(firstRender);
    }

    private string GetTime()
    {
        Math.DivRem(DateTime.Now.Ticks, TimeSpan.TicksPerMillisecond, out long millisecondparts);
        var mills = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        Math.DivRem(mills, 1000, out long millseconds);
        return $" {DateTime.Now.ToLongTimeString()}:{millseconds}:{millisecondparts}";
    }
}