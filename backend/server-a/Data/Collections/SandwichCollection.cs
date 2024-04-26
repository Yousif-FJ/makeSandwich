using server_a.Data.Models;

namespace server_a.Data.Collections;
public class SandwichCollection : List<Sandwich>
{
    public SandwichCollection()
    {
        AddRange(MockData());
    }

    public SandwichCollection(IEnumerable<Sandwich> collection) : base(collection)
    {
        AddRange(MockData());
    }

    public SandwichCollection(int capacity) : base(capacity)
    {
        AddRange(MockData());
    }
    private static IList<Sandwich> MockData()
    {
        return [
                new() { Id = 0, Name = "Ham and cheese", BreadType = BreadTypeEnum.Wheat},
                new() { Id = 1, Name = "Vegetarian", BreadType = BreadTypeEnum.Rye },
                new() { Id = 2, Name = "BLT", BreadType = BreadTypeEnum.Oat },
            ];
    }
}