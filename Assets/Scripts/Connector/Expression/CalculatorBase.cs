using System;
using System.Collections.Generic;
using UniFlow.Utility;
using UnityEngine;

namespace UniFlow.Connector.Expression
{
    public class CalculatorBase<TValue, TValueCollector> : ConnectorBase, IMessageCollectable, IMessageComposable where TValueCollector : ValueCollectorBase<TValue>, new()
    {
        [SerializeField] private TValue left = default;
        [SerializeField] private TValue right = default;
        [SerializeField] private OperatorType operatorType = default;
        private TValue Left
        {
            get => left;
            set => left = value;
        }
        private TValue Right
        {
            get => right;
            set => right = value;
        }
        private OperatorType OperatorType => operatorType;

        [SerializeField] private TValueCollector leftCollector = new TValueCollector();
        [SerializeField] private TValueCollector rightCollector = new TValueCollector();
        private TValueCollector LeftCollector => leftCollector;
        private TValueCollector RightCollector => rightCollector;

        public override IObservable<Message> OnConnectAsObservable()
        {
            return ObservableFactory.ReturnMessage(this, "Result", Calculate());
        }

        private TValue Calculate()
        {
            switch (OperatorType)
            {
                case OperatorType.Add when this is IAdditionCalculator<TValue> calculator:
                    return calculator.Add(Left, Right);
                case OperatorType.Sub when this is ISubtractionCalculator<TValue> calculator:
                    return calculator.Subtract(Left, Right);
                case OperatorType.Mul when this is IMultiplicationCalculator<TValue> calculator:
                    return calculator.Multiply(Left, Right);
                case OperatorType.Div when this is IDivisionCalculator<TValue> calculator:
                    return calculator.Divide(Left, Right);
                case OperatorType.Mod when this is IModulationCalculator<TValue> calculator:
                    return calculator.Modulo(Left, Right);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                CollectableMessageAnnotationFactory.Create(LeftCollector, x => Left = x, nameof(Left)),
                CollectableMessageAnnotationFactory.Create(RightCollector, x => Right = x, nameof(Right)),
            };

        IEnumerable<IComposableMessageAnnotation> IMessageComposable.GetMessageComposableAnnotations() =>
            new[]
            {
                ComposableMessageAnnotationFactory.Create<TValue>("Result"),
            };
    }

    public interface IAdditionCalculator<TValue>
    {
        TValue Add(TValue left, TValue right);
    }

    public interface ISubtractionCalculator<TValue>
    {
        TValue Subtract(TValue left, TValue right);
    }

    public interface IMultiplicationCalculator<TValue>
    {
        TValue Multiply(TValue left, TValue right);
    }

    public interface IDivisionCalculator<TValue>
    {
        TValue Divide(TValue left, TValue right);
    }

    public interface IModulationCalculator<TValue>
    {
        TValue Modulo(TValue left, TValue right);
    }

    public interface IFourArithmeticCalculator<TValue> : IAdditionCalculator<TValue>, ISubtractionCalculator<TValue>, IMultiplicationCalculator<TValue>, IDivisionCalculator<TValue>
    {
    }

    public enum OperatorType
    {
        Add,
        Sub,
        Mul,
        Div,
        Mod,
    }
}
