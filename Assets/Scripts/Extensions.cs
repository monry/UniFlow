using System;
using System.Collections.Generic;
using System.Linq;
using UniFlow.Utility;
using UniRx;
using UnityEngine;
using Zenject;

namespace UniFlow
{
    public interface IBaseGameObjectSpecifyable
    {
        GameObject BaseGameObject { get; }
        string TransformPath { get; }
    }

    public static class Extensions
    {
        public static TComponent GetOrAddComponent<TComponent>(this GameObject gameObject) where TComponent : Component
        {
            return gameObject.GetComponent<TComponent>() != default
                ? gameObject.GetComponent<TComponent>()
                : gameObject.AddComponent<TComponent>();
        }

        public static Transform DeterminateTransform(this IBaseGameObjectSpecifyable baseGameObjectSpecifyable)
        {
            return string.IsNullOrEmpty(baseGameObjectSpecifyable.TransformPath)
                ? baseGameObjectSpecifyable.BaseGameObject.transform
                : baseGameObjectSpecifyable.BaseGameObject.transform.Find(baseGameObjectSpecifyable.TransformPath);
        }

        public static TComponent GetComponent<TComponent>(this IBaseGameObjectSpecifyable baseGameObjectSpecifyable) where TComponent : Component
        {
            return baseGameObjectSpecifyable.DeterminateTransform().gameObject.GetComponent<TComponent>();
        }

        public static TComponent GetOrAddComponent<TComponent>(this IBaseGameObjectSpecifyable baseGameObjectSpecifyable) where TComponent : Component
        {
            return baseGameObjectSpecifyable.DeterminateTransform().gameObject.GetOrAddComponent<TComponent>();
        }

        public static IObservable<Message> AsMessageObservable(this IObservable<Unit> observable, IConnector connector)
        {
            return observable.Select(connector.CreateMessage);
        }

        public static IObservable<Message> AsMessageObservable<T>(this IObservable<T> observable, IConnector connector)
        {
            return observable.Select(connector.CreateMessage);
        }

        public static IObservable<Message> AsMessageObservable<T>(this IObservable<T> observable, IConnector connector, string key)
        {
            return observable.Select(x => connector.CreateMessage(x, key));
        }

        public static Message CreateMessage(this IConnector connector)
        {
            var message = Message.Create(connector);
            message.StreamedMessages = connector.StreamedMessages;
            return message;
        }

        public static Message CreateMessage(this IConnector connector, Unit _)
        {
            var message = Message.Create(connector);
            message.StreamedMessages = connector.StreamedMessages;
            return message;
        }

        public static Message CreateMessage<T>(this IConnector connector, T parameter)
        {
            var message = Message.Create(connector, parameter);
            message.StreamedMessages = connector.StreamedMessages;
            return message;
        }

        public static Message CreateMessage<T>(this IConnector connector, T parameter, string key)
        {
            var message = Message.Create(connector, parameter, key);
            message.StreamedMessages = connector.StreamedMessages;
            return message;
        }

        public static bool HasMessage(this IEnumerable<Message> messages, IConnector connector)
        {
            return messages.Any(x => x.Connector == connector);
        }

        public static Message GetMessage(this IEnumerable<Message> messages, IConnector connector)
        {
            return connector == default ? default : messages.Last(x => x.Connector == connector);
        }
    }

    public static class ZenjectExtensions
    {
        public static DeclareSignalAsyncTickPriorityCopyBinder DeclareUniFlowSignal<TSignal>(this DiContainer container)
        {
            container.BindInterfacesTo<SignalHandler<TSignal>>().AsCached();
            return container.DeclareSignal<TSignal>().OptionalSubscriber();
        }
    }
}
