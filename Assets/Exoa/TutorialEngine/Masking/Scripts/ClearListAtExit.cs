using System;
using System.Collections.Generic;

namespace Maskable {
    struct ClearListAtExit<T> : IDisposable {
        List<T> _list;
        public ClearListAtExit(List<T> list) { _list = list; }
        public void Dispose() { _list.Clear(); }
    }
}
