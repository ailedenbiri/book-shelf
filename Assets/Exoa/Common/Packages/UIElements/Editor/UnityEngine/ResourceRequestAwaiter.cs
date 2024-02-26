using System;
using System.Runtime.CompilerServices;
namespace UnityEngine
{
	public sealed class ResourceRequestAwaiter : INotifyCompletion
	{
		private ResourceRequest asyncOp;
		private Action continuation;
		public bool IsCompleted
		{
			get
			{
				return this.asyncOp.isDone;
			}
		}
		public ResourceRequestAwaiter(ResourceRequest asyncOp)
		{
			this.asyncOp = asyncOp;
			asyncOp.completed+=(new Action<AsyncOperation>(this.OnRequestCompleted));
		}
		public Object GetResult()
		{
			ResourceRequest expr_06 = this.asyncOp;
			if (expr_06 == null)
			{
				return null;
			}
			return expr_06.asset;
		}
		public void OnCompleted(Action continuation)
		{
			this.continuation = continuation;
		}
		private void OnRequestCompleted(AsyncOperation obj)
		{
			this.continuation();
		}
	}
}
