using System;
using System.Collections.Concurrent;
namespace System.Pool.Internal
{
	public sealed class ObjectPool<T>
	{
		private readonly ConcurrentBag<T> _stack = new ConcurrentBag<T>();
		private readonly Action<T> _actionOnGet;
		private readonly Action<T> _actionOnRelease;
		private readonly Func<T> _actionOnNew;
		public int Count
		{
			get;
			private set;
		}
		public ObjectPool(Action<T> actionOnGet, Action<T> actionOnRelease, Func<T> actionOnNew = null)
		{
			this._actionOnGet = actionOnGet;
			this._actionOnRelease = actionOnRelease;
			this._actionOnNew = (actionOnNew ?? new Func<T>(this.New));
		}
		public T Get()
		{
			T t;
			try
			{
				ConcurrentBag<T> stack = this._stack;
				do
				{
					if (stack.IsEmpty)
					{
						t = this._actionOnNew();
						int count = this.Count;
						this.Count = count + 1;
					}
					else
					{
						if (!stack.TryTake(out t))
						{
							t = this._actionOnNew();
							int count = this.Count;
							this.Count = count + 1;
						}
					}
				}
				while (t == null);
			}
			catch
			{
				t = this._actionOnNew();
			}
			Action<T> expr_72 = this._actionOnGet;
			if (expr_72 != null)
			{
				expr_72(t);
			}
			return t;
		}
		public void Release(T element)
		{
			if (element == null)
			{
				return;
			}
			try
			{
				ConcurrentBag<T> stack = this._stack;
				T t;
				if (stack.Count <= 0 || !stack.TryPeek(out t) || t.GetType() != element.GetType())
				{
					Action<T> expr_3A = this._actionOnRelease;
					if (expr_3A != null)
					{
						expr_3A(element);
					}
					stack.Add(element);
				}
			}
			catch
			{
			}
		}
		private T New()
		{
			return Activator.CreateInstance<T>();
		}
	}
}
