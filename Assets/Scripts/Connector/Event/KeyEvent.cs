using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace UniFlow.Connector.Event
{
    [AddComponentMenu("UniFlow/Event/KeyEvent", (int) ConnectorType.KeyEvent)]
    public class KeyEvent : ConnectorBase
    {
        [SerializeField] private string groupName = default;
        [SerializeField] private KeyEventType keyEventType = KeyEventType.Down;
        [SerializeField] private KeyCode keyCode = default;

        private string GroupName => groupName;
        private KeyEventType KeyEventType => keyEventType;
        private KeyCode KeyCode => keyCode;

        public override IObservable<Message> OnConnectAsObservable()
        {
            return KeyEventAsObservable().Where(_ => RegisterCommandGroup(GroupName).Execute(Unit.Default)).Select(this.CreateMessage);
        }

        private IObservable<Unit> KeyEventAsObservable()
        {
            switch (KeyEventType)
            {
                case KeyEventType.Down:
                    return this
                        .UpdateAsObservable()
                        .Where(_ => Input.GetKeyDown(KeyCode))
                        .AsUnitObservable();
                case KeyEventType.Press:
                    return this
                        .UpdateAsObservable()
                        .Where(_ => Input.GetKey(KeyCode))
                        .AsUnitObservable();
                case KeyEventType.Up:
                    return this
                        .UpdateAsObservable()
                        .Where(_ => Input.GetKeyUp(KeyCode))
                        .AsUnitObservable();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnDestroy()
        {
            UnregisterCommandGroup(GroupName);
        }

        internal static IReactiveProperty<bool> Gate { get; } = new BoolReactiveProperty(true);

        private static IDictionary<string, ReactiveCommandReference> CommandGroups { get; } = new Dictionary<string, ReactiveCommandReference>();

        private static IReactiveCommand<Unit> RegisterCommandGroup(string groupName)
        {
            if (!CommandGroups.ContainsKey(groupName) || CommandGroups[groupName] == default)
            {
                CommandGroups[groupName] = new ReactiveCommandReference(Gate);
            }

            CommandGroups[groupName].ReferenceCount++;
            return CommandGroups[groupName].Command;
        }

        private static void UnregisterCommandGroup(string groupName)
        {
            if (CommandGroups.ContainsKey(groupName) && CommandGroups[groupName] != default)
            {
                CommandGroups[groupName].ReferenceCount--;

                if (CommandGroups[groupName].ReferenceCount <= 0)
                {
                    CommandGroups.Remove(groupName);
                }
            }
        }

        private class ReactiveCommandReference
        {
            internal ReactiveCommandReference(IObservable<bool> gate)
            {
                Command = gate.ToReactiveCommand();
            }

            internal IReactiveCommand<Unit> Command { get; }
            internal int ReferenceCount { get; set; }
        }
    }

    public enum KeyEventType
    {
        Down,
        Press,
        Up,
    }
}
