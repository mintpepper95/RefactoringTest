using System;

namespace Refactoring.LegacyService;

public class TimeProvider : ITimeProvider {
    public DateTime Now => DateTime.Now;
}
