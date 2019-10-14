using System;
using System.Collections.Generic;
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
            return observable.Select(connector.CreateMessage);
        }

        public static Message CreateMessage(this IConnector connector)
        {
            return Message.Create(connector);
        }

        public static Message CreateMessage(this IConnector connector, Unit _)
        {
            return Message.Create(connector);
        }

        public static Message CreateMessage<T>(this IConnector connector, T parameter)
        {
            return Message.Create(connector, parameter);
        }

        public static Message CreateMessage<T>(this IConnector connector, IEnumerable<T> parameters)
        {
            return Message.Create(connector, parameters);
        }
    }
}
