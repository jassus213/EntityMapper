namespace EntityMapper.Configuration;

public readonly struct Configuration
{
    public readonly Delegate ConfigurationFunc;
    public readonly bool IsDisposable;

    public Configuration(Delegate configurationFunc, bool isDisposable = false)
    {
        ConfigurationFunc = configurationFunc;
        IsDisposable = isDisposable;
    }
}