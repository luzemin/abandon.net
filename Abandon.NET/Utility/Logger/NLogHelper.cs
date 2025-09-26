using CorrelationId.Abstractions;
using NLog;

namespace Abandon.NET.Utility.Logger;

public sealed class NLogHelper : INLogHelper
{
    private static NLog.Logger _logger = LogManager.GetCurrentClassLogger();
    private readonly ICorrelationContextAccessor _correlationContext;

    public NLogHelper(ICorrelationContextAccessor correlationContext)
    {
        this._correlationContext = correlationContext;
    }

    /// <summary>获取CorrelationId</summary>
    /// <returns></returns>
    public Dictionary<string, string> GetCorrelationId()
    {
        return new Dictionary<string, string>()
        {
            {
                "X-Correlation-Id",
                this._correlationContext.CorrelationContext.CorrelationId
            }
        };
    }

    private string GetNewMsg(string Msg)
    {
        return ((this._correlationContext.CorrelationContext != null ? $"【{this._correlationContext.CorrelationContext.CorrelationId}】" : "") ?? "") + Msg;
    }

    public void Trace(string msg) => NLogHelper._logger.Trace(this.GetNewMsg(msg));

    public void Debug(string msg) => NLogHelper._logger.Debug(this.GetNewMsg(msg));

    public void Info(string msg) => NLogHelper._logger.Info(this.GetNewMsg(msg));

    public void Warn(string msg) => NLogHelper._logger.Warn(this.GetNewMsg(msg));

    public void Error(string msg) => NLogHelper._logger.Error(this.GetNewMsg(msg));

    public void Error(Exception ex) => NLogHelper._logger.Error(this.GetNewMsg(ex.ToString()));

    public void Fatal(string msg) => NLogHelper._logger.Fatal(this.GetNewMsg(msg));
}