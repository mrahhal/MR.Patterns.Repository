namespace Basic.Services
{
	public interface IMath
	{
		int Add(int x, int y);
	}

	public class DefaultMath : IMath
	{
		public virtual int Add(int x, int y) => x + y;
	}
}
