namespace PBScriptNew.Models;

public class AuditSettings
{
    public string AuditFilter { get; set; } = "and (SUBSTRING(name,3,1) = '_')";
    public string AuditExclude { get; set; } = "and name NOT IN ('NUMBERS', 'FW_ASYNC_SCHEDULER', 'FW_PROCESSING_DETAILS')";
}
