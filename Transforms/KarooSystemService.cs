using IO.Hammerhead.Karooext.Models;
using karooextlink.Classes;
using Kotlinx.Serialization.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO.Hammerhead.Karooext
{
    public partial class KarooSystemService
    {
        static Json? _defaultJson;
        public static Json DefaultJson
        {
            get
            {
                if (_defaultJson == null)
                {
                    var ba = new Func1<JsonBuilder>((JsonBuilder b) => 
                    { 
                        b.EncodeDefaults = true; 
                        b.ExplicitNulls = false; 
                        b.IgnoreUnknownKeys = true;
                        b.ClassDiscriminatorMode = ClassDiscriminatorMode.AllJsonObjects;
                    });
                    _defaultJson = JsonKt.Json(Json.Default_, ba);
                }
                return _defaultJson;
            }
        }

        public void Connect(Action<bool> resultDelegate)
        {
            Action<Java.Lang.Boolean> jResult = (Java.Lang.Boolean result) =>
            {
                resultDelegate(result.BooleanValue());
            };
            Connect(new Func1<Java.Lang.Boolean>(jResult));
        }

        public string AddConsumer<T>(Action<T> onEvent, KarooEventParams? param = null, Action<string>? onError = null, Action? onComplete = null) where T : KarooEvent
        {
            var consumerId = Java.Util.UUID.RandomUUID().ToString();
            var onErrorWrapper = (string e) =>
            {
                onError?.Invoke(e);
                RemoveConsumer(consumerId);
            };
            var onCompleteWrapper = () =>
            {
                onComplete?.Invoke();
                RemoveConsumer(consumerId);
            };
            var consumer = new Consumer<T>(onError, onComplete, onEvent);
            if (param == null)
            {
                var paramType = typeof(T).GetNestedType("Params");
                if (paramType != null)
                {
                    param = (KarooEventParams)paramType.GetProperty("Instance").GetValue(null);
                }
                else
                {
                    throw new Exception("No default KarooEventParams for class " + typeof(T).Name);
                }
            }
            var klistener = new Listener<T>(consumer, param, PackageName, consumerId);
            return AddConsumer(klistener);
        }
    }
}
