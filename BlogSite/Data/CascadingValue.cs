namespace BlogSite.Data
{
	public class CascadingObject<T>
	{
		T? _value;
		public T? Value
		{
			get
			{
				return _value;
			}
			set
			{
				_value = value;
				ValueChanged?.Invoke();
			}
		}

		public event Action? ValueChanged;
	}
}
