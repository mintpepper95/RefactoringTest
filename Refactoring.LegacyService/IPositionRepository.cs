using System.Threading.Tasks;

namespace Refactoring.LegacyService;
public interface IPositionRepository {
    Task<Position> GetByIdAsync(int id);
}

