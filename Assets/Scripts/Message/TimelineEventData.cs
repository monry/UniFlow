using System;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EventConnector.Message
{
    [Serializable][PublicAPI]
    public class TimelineEventData
    {
        [SerializeField] private int intParameter = default;
        [SerializeField] private float floatParameter = default;
        [SerializeField] private string stringParameter = default;
        [SerializeField] private Object objectReferenceParameter = default;
        public int IntParameter => intParameter;
        public float FloatParameter => floatParameter;
        public string StringParameter => stringParameter;
        public Object ObjectReferenceParameter => objectReferenceParameter;

        public TimelineEventData(float floatValue) : this(default, floatValue)
        {
        }

        public TimelineEventData(string stringValue) : this(default, default, stringValue)
        {
        }

        public TimelineEventData(Object objectValue) : this(default, default, default, objectValue)
        {
        }

        public TimelineEventData(int intParameter = default, float floatParameter = default, string stringParameter = default, Object objectReferenceParameter = default)
        {
            this.intParameter = intParameter;
            this.floatParameter = floatParameter;
            this.stringParameter = stringParameter;
            this.objectReferenceParameter = objectReferenceParameter;
        }
    }
}