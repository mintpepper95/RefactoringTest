using System;

namespace Refactoring.LegacyService;

public interface ITimeProvider {
    DateTime Now { get; }
}
