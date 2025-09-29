using CorrelationId.Abstractions;
using NLog;

namespace Abandon.NET.Utility.Logger;

public sealed class NLogHelper : INLogHelper
{
    private static NLog.Logger _logger = LogManager.GetCurrentClassLogger();

    public void Trace(string msg) => _logger.Trace(msg);

    public void Debug(string msg) => _logger.Debug(msg);

    public void Info(string msg) => _logger.Info(msg);

    public void Warn(string msg) => _logger.Warn(msg);

    public void Error(string msg) => _logger.Error(msg);

    public void Error(Exception ex) => _logger.Error(ex.ToString());

    public void Fatal(string msg) => _logger.Fatal(msg);
}