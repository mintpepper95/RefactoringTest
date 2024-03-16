namespace Refactoring.LegacyService;

public class Position {
    public int Id { get; private set; }
    public string Name { get; private set; }
    // I've updated Status from Enum to string. As it doesn't really make any sense to have an enum with only 1 value.
    public string Status { get; private set; }

    public Position(int id, string name, string status) {
        Id = id;
        Name = name;
        Status = status;
    }
}
