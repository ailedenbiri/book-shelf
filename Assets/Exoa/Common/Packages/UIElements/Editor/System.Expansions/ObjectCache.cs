using System;
using System.Collections.Concurrent;
namespace System.Pool.Internal
{
	public sealed class ObjectCache<TKey, TValue>
	{
		private readonly ConcurrentDictionary<TKey, TValue> _stack = new ConcurrentDictionary<TKey, TValue>();
		private readonly Func<TKey, TValue> _actionOnNew;
		private readonly Action<TValue> _actionOnGet;
		public int Count
		{
			get
			{
				return this._stack.Count;
			}
		}
		public ObjectCache(Func<TKey, TValue> actionOnNew, Action<TValue> actionOnGet = null)
		{
			if (actionOnNew == null)
			{
				throw new NullReferenceException();
			}
			this._actionOnGet = actionOnGet;
			this._actionOnNew = actionOnNew;
		}
		public TValue Get(TKey key)
		{
			TValue tValue;
			try
			{
				ConcurrentDictionary<TKey, TValue> stack = this._stack;
				if (stack.IsEmpty)
				{
					tValue = this._actionOnNew(key);
					if (tValue != null)
					{
						stack.TryAdd(key, tValue);
					}
				}
				else
				{
					if (!stack.TryGetValue(key, out tValue))
					{
						tValue = this._actionOnNew(key);
						if (tValue != null)
						{
							stack.TryAdd(key, tValue);
						}
					}
				}
				if (tValue == null)
				{
					TValue comparisonValue = tValue;
					tValue = this._actionOnNew(key);
					stack.TryUpdate(key, tValue, comparisonValue);
				}
			}
			catch
			{
				tValue = this._actionOnNew(key);
			}
			Action<TValue> expr_91 = this._actionOnGet;
			if (expr_91 != null)
			{
				expr_91(tValue);
			}
			return tValue;
		}
	}
}
