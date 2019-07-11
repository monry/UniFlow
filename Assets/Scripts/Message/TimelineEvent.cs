using System;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EventConnector.Message
{
    [Serializable][PublicAPI]
    public class TimelineEvent
    {
        [SerializeField] private int intParameter;
        [SerializeField] private float floatParameter;
        [SerializeField] private string stringParameter;
        [SerializeField] private Object objectReferenceParameter;
        public int IntParameter => intParameter;
        public float FloatParameter => floatParameter;
        public string StringParameter => stringParameter;
        public Object ObjectReferenceParameter => objectReferenceParameter;

        public TimelineEvent(float floatValue) : this(default, floatValue)
        {
        }

        public TimelineEvent(string stringValue) : this(default, default, stringValue)
        {
        }

        public TimelineEvent(Object objectValue) : this(default, default, default, objectValue)
        {
        }

        public TimelineEvent(int intParameter = default, float floatParameter = default, string stringParameter = default, Object objectReferenceParameter = default)
        {
            this.intParameter = intParameter;
            this.floatParameter = floatParameter;
            this.stringParameter = stringParameter;
            this.objectReferenceParameter = objectReferenceParameter;
        }
    }
}