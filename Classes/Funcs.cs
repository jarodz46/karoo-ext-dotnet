using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karooext.dotnet.Classes
{
    internal class Func1<T> : Java.Lang.Object, Kotlin.Jvm.Functions.IFunction1 where T : Java.Lang.Object
    {

        private readonly Action<T> _callback;

        public Func1(Action<T> callback)
        {
            _callback = callback;
        }

        public Java.Lang.Object? Invoke(Java.Lang.Object? p0)
        {
            if (p0 != null)
                _callback?.Invoke((T)p0);
            return null; // Retourner `null` (équivalent à Kotlin's Unit)
        }
    }

    internal class Func0 : Java.Lang.Object, Kotlin.Jvm.Functions.IFunction0
    {

        private readonly Action _callback;

        public Func0(Action callback)
        {
            _callback = callback;
        }

        public Java.Lang.Object? Invoke()
        {
            _callback?.Invoke();
            return null; // Retourner `null` (équivalent à Kotlin's Unit)
        }
    }
}
