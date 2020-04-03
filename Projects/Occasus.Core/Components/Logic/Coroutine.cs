using System;
using System.Collections;

namespace Occasus.Core.Components.Logic
{
    public class Coroutine : ICoroutine
    {
        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public object Current
        {
            get { throw new NotImplementedException(); }
        }

        public bool MoveNext()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
