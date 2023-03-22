namespace Services.Utils
{
    public interface IModelStateWrapper
    {
        void AddError(string key, string errorMessage);
        bool IsValid { get; }

        Dictionary<string, string> Errors { get; }
    }
}
