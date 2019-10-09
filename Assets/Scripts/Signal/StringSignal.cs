using System;
using JetBrains.Annotations;
using UnityEngine;

namespace UniFlow.Signal
{
    [Serializable]
    [PublicAPI]
    public struct StringSignal : IEquatableSignal<StringSignal>
    {
        public StringSignal(string name)
        {
            this.name = name;
        }

        [SerializeField] private string name;

        public string Name
        {
            get => name;
            set => name = value;
        }

        public bool Equals(StringSignal other)
        {
            return Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            return obj is StringSignal other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Name != null ? Name.GetHashCode() : 0;
        }
    }
}
