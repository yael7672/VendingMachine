using VendingMachine.Api.Domain.Exceptions;

namespace VendingMachine.Api.Domain.Entities;

public class VendingMachine
{
    private readonly object _sync = new();
    private readonly List<Shelf> _shelves = new();

    public Guid Id { get; }
    public string Name { get; }
    public string? Location { get; }
    public int MaxShelves { get; }

    public IReadOnlyCollection<Shelf> Shelves
    {
        get
        {
            lock (_sync)
            {
                return _shelves.ToArray();
            }
        }
    }

    public VendingMachine(string name, string? location, int maxShelves)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Machine name is required.", nameof(name));

        if (maxShelves <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(maxShelves), "Max shelves must be greater than zero.");

        Id = Guid.NewGuid();
        Name = name.Trim();
        Location = string.IsNullOrWhiteSpace(location) ? null : location.Trim();
        MaxShelves = maxShelves;
    }

    public Shelf AddShelf(string productType, int capacity)
    {
        // Keep capacity validation and shelf creation atomic.
        lock (_sync)
        {
            if (_shelves.Count >= MaxShelves)
                throw new BusinessRuleException(
                    "Machine has reached its maximum number of shelves.");

            var shelf = new Shelf(productType, capacity);
            _shelves.Add(shelf);
            return shelf;
        }
    }
}
