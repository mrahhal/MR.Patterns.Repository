namespace MR.Patterns.Repository
{
	public interface IEntity<TKey>
	{
		TKey Id { get; set; }
	}
}
