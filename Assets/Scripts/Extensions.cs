using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace UniFlow
{
    public interface IBaseGameObjectSpecifyable
    {
        GameObject BaseGameObject { get; }
        string TransformPath { get; }
    }

    public static class Extensions
    {
        private static TComponent GetOrAddComponent<TComponent>(this GameObject gameObject) where TComponent : Component
        {
            return gameObject.GetComponent<TComponent>() != default
                ? gameObject.GetComponent<TComponent>()
                : gameObject.AddComponent<TComponent>();
        }

        public static TComponent GetComponent<TComponent>(this IBaseGameObjectSpecifyable baseGameObjectSpecifyable) where TComponent : Component
        {
            return string.IsNullOrEmpty(baseGameObjectSpecifyable.TransformPath)
                ? baseGameObjectSpecifyable.BaseGameObject.GetComponent<TComponent>()
                : baseGameObjectSpecifyable.BaseGameObject.transform.Find(baseGameObjectSpecifyable.TransformPath).gameObject.GetComponent<TComponent>();
        }

        public static TComponent GetOrAddComponent<TComponent>(this IBaseGameObjectSpecifyable baseGameObjectSpecifyable) where TComponent : Component
        {
            return string.IsNullOrEmpty(baseGameObjectSpecifyable.TransformPath)
                ? baseGameObjectSpecifyable.BaseGameObject.GetOrAddComponent<TComponent>()
                : baseGameObjectSpecifyable.BaseGameObject.transform.Find(baseGameObjectSpecifyable.TransformPath).gameObject.GetOrAddComponent<TComponent>();
        }

        public static IObservable<Message> AsMessageObservable(this IObservable<Unit> observable, IConnector connector)
        {
            return observable.Select(connector.CreateMessage);
        }

        public static IObservable<Message> AsMessageObservable<T>(this IObservable<T> observable, IConnector connector)
        {
            return observable.AsMessageObservable(connector, typeof(T).Name);
        }

        public static IObservable<Message> AsMessageObservable<T>(this IObservable<T> observable, IConnector connector, string key)
        {
            return observable.Select(x => connector.CreateMessage(key, x));
        }

        public static Message CreateMessage(this IConnector connector)
        {
            var message = Message.Create(connector, connector.StreamedMessages);
            message.StreamedMessages = connector.StreamedMessages;
            return message;
        }

        public static Message CreateMessage(this IConnector connector, Unit _)
        {
            var message = Message.Create(connector, connector.StreamedMessages);
            message.StreamedMessages = connector.StreamedMessages;
            return message;
        }

        public static Message CreateMessage<T>(this IConnector connector, T parameter)
        {
            return connector.CreateMessage(typeof(T).Name, parameter);
        }

        public static Message CreateMessage<T>(this IConnector connector, string key, T parameter)
        {
            var message = Message.Create(connector, key, parameter);
            message.StreamedMessages = connector.StreamedMessages;
            return message;
        }

        public static Message CreateMessage<T>(this IConnector connector, IEnumerable<T> parameters)
        {
            return connector.CreateMessage($"{typeof(T).Name}List", parameters);
        }

        public static Message CreateMessage<T>(this IConnector connector, string key, IEnumerable<T> parameters)
        {
            var message = Message.Create(connector, key, parameters);
            message.StreamedMessages = connector.StreamedMessages;
            return message;
        }

        public static bool HasMessage<T>(this IEnumerable<Message> messages)
        {
            return messages.HasMessage<T>(typeof(T).Name);
        }

        public static bool HasMessage<T>(this IEnumerable<Message> messages, string key)
        {
            return messages.Any(x => x.HasParameter<T>(key));
        }

        public static bool HasMessage<T>(this IEnumerable<Message> messages, Func<T, bool> predicate)
        {
            return messages.HasMessage(typeof(T).Name, predicate);
        }

        public static bool HasMessage<T>(this IEnumerable<Message> messages, string key, Func<T, bool> predicate)
        {
            return messages.Any(x => x.HasParameter(key, predicate));
        }

        public static Message FindMessage<T>(this IEnumerable<Message> messages, bool latest = true)
        {
            return messages.FindMessage<T>(typeof(T).Name, latest);
        }

        public static Message FindMessage<T>(this IEnumerable<Message> messages, string key, bool latest = true)
        {
            return latest ? messages.Last(x => x.HasParameter<T>(key)) : messages.First(x => x.HasParameter<T>(key));
        }

        public static Message FindMessage<T>(this IEnumerable<Message> messages, Func<T, bool> predicate, bool latest = true)
        {
            return messages.FindMessage(typeof(T).Name, predicate, latest);
        }

        public static Message FindMessage<T>(this IEnumerable<Message> messages, string key, Func<T, bool> predicate, bool latest = true)
        {
            return latest ? messages.Last(x => x.HasParameter(key, predicate)) : messages.First(x => x.HasParameter(key, predicate));
        }
    }
}
