namespace DomainModel
{
    public interface IDomainIdentifiable<TKey>
    {
        TKey Id { get; set; }
        string Name { get; set; }
    }
}