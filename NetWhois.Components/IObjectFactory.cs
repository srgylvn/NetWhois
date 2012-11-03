namespace NetWhois.Components
{
	public interface IObjectFactory
	{
		T Instance<T>();
		T Instance<T>(object parameters);
	}
}