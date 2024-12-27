using IO.Hammerhead.Karooext;
using IO.Hammerhead.Karooext.Aidl;
using IO.Hammerhead.Karooext.Internal;
using IO.Hammerhead.Karooext.Models;
using Java.Lang.Annotation;
using Java.Util;
using Kotlinx.Serialization.Json;
using KotlinX.Serialization;
using KotlinX.Serialization.Descriptors;
using KotlinX.Serialization.Encoding;
using KotlinX.Serialization.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karooext.dotnet.Classes
{

    internal class Listener<T> : KarooSystemListener where T : Java.Lang.Object
    {
        Consumer<T> consumer;
        string packageName;
        KarooEventParams param;
        public Listener(Consumer<T> consumer, KarooEventParams param, string packageName, string consumerId) : base(consumerId)
        {
            this.consumer = consumer;
            this.packageName = packageName;
            this.param = param;

        }

        public override void Register(IKarooSystem? controller)
        {
            var b = new Bundle();            
            var serializer = SerializersKt.Serializer(param.Class);
            var json = KarooSystemService.DefaultJson.EncodeToString(serializer, param);
            b.PutString("value", json);
            b.PutString("package", packageName);
            controller?.AddEventConsumer(Id, b, consumer);
        }

        public override void Unregister(IKarooSystem? controller)
        {
            controller?.RemoveEventConsumer(Id);
        }
    }
}
