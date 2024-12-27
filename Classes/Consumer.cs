using Android.Runtime;
using IO.Hammerhead.Karooext;
using IO.Hammerhead.Karooext.Aidl;
using IO.Hammerhead.Karooext.Internal;
using IO.Hammerhead.Karooext.Models;
using Kotlinx.Serialization.Json;
using KotlinX.Serialization;
using KotlinX.Serialization.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karooextlink.Classes
{
    public class Consumer<T> : IHandler.Stub where T : Java.Lang.Object
    {
        Action<string>? onError;
        Action? onComplete;
        Action<T>? onEvent;

        public Consumer(Action<string>? onError, Action? onComplete, Action<T> onEvent)
        {
            this.onError = onError;
            this.onComplete = onComplete;
            this.onEvent = onEvent;
        }

        public override void OnComplete()
        {
            onComplete?.Invoke();
        }

        public override void OnError(string? p0)
        {
            if (p0 != null)
                onError?.Invoke(p0);
        }


        public override void OnNext(Bundle? p0)
        {

            var jsonVal = p0?.GetString("value");
            if (jsonVal != null)
            {
                var tjClass = Java.Lang.Class.FromType(typeof(T));
                var serializer = SerializersKt.Serializer(tjClass);
                var obj = EmitterKt.DefaultJson.DecodeFromString(serializer, jsonVal);
                if (obj != null)
                {
                    var cobj = obj.JavaCast<T>();
                    if (cobj != null)
                        onEvent?.Invoke(cobj);
                }
            }
        }
    }
}
