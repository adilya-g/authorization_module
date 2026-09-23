using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExampleApplication.Helpers;

public static class ValidationHelper
{
    public static bool TryValidatePeriod(DateTime? start, DateTime? end, out string? error)
    {
        if (start is null || end is null)
        {
            error = "startDate and endDate are required";
            return false;
        }
        if (start > end)
        {
            error = "startDate must be earlier than endDate";
            return false;
        }
        error = null;
        return true;
    }
}