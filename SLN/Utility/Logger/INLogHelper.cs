namespace SLN.Utility.Logger;

public interface INLogHelper
{
    void Trace(string msg);

    void Debug(string msg);

    void Info(string msg);

    void Warn(string msg);

    void Error(string msg);

    void Error(Exception ex);

    void Fatal(string msg);

    Dictionary<string, string> GetCorrelationId();
}