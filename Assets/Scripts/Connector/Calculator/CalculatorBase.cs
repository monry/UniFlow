using System;
using System.Collections.Generic;
using UniFlow.Utility;
using UnityEngine;

namespace UniFlow.Connector.Calculator
{
    public abstract class CalculatorBase<TValue, TValueCollector> : CalculatorBase<TValue, TValue, TValueCollector, TValueCollector>
        where TValueCollector : ValueCollectorBase<TValue>, new()
    {

    }

    public abstract class CalculatorBase<TLeftValue, TRightValue, TLeftValueCollector, TRightValueCollector> : ConnectorBase, IMessageCollectable, IMessageComposable
        where TLeftValueCollector : ValueCollectorBase<TLeftValue>, new()
        where TRightValueCollector : ValueCollectorBase<TRightValue>, new()
    {
        [SerializeField] private TLeftValue left = default;
        [SerializeField] private TRightValue right = default;
        [SerializeField] private OperatorType operatorType = default;
        private TLeftValue Left
        {
            get => left;
            set => left = value;
        }
        private TRightValue Right
        {
            get => right;
            set => right = value;
        }
        private OperatorType OperatorType => operatorType;

        [SerializeField] private TLeftValueCollector leftCollector = new TLeftValueCollector();
        [SerializeField] private TRightValueCollector rightCollector = new TRightValueCollector();
        private TLeftValueCollector LeftCollector => leftCollector;
        private TRightValueCollector RightCollector => rightCollector;

        public override IObservable<Message> OnConnectAsObservable()
        {
            return ObservableFactory.ReturnMessage(this, "Result", Calculate());
        }

        private TLeftValue Calculate()
        {
            switch (OperatorType)
            {
                case OperatorType.Add when this is IAdditionCalculator<TLeftValue, TRightValue> calculator:
                    return calculator.Add(Left, Right);
                case OperatorType.Sub when this is ISubtractionCalculator<TLeftValue, TRightValue> calculator:
                    return calculator.Subtract(Left, Right);
                case OperatorType.Mul when this is IMultiplicationCalculator<TLeftValue, TRightValue> calculator:
                    return calculator.Multiply(Left, Right);
                case OperatorType.Div when this is IDivisionCalculator<TLeftValue, TRightValue> calculator:
                    return calculator.Divide(Left, Right);
                case OperatorType.Mod when this is IModulationCalculator<TLeftValue, TRightValue> calculator:
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
                ComposableMessageAnnotationFactory.Create<TLeftValue>("Result"),
            };
    }

    public interface IAdditionCalculator<TValue> : IAdditionCalculator<TValue, TValue>
    {
    }

    public interface IAdditionCalculator<TLeftValue, in TRightValue>
    {
        TLeftValue Add(TLeftValue left, TRightValue right);
    }

    public interface ISubtractionCalculator<TValue> : ISubtractionCalculator<TValue, TValue>
    {
    }

    public interface ISubtractionCalculator<TLeftValue, in TRightValue>
    {
        TLeftValue Subtract(TLeftValue left, TRightValue right);
    }

    public interface IMultiplicationCalculator<TValue> : IMultiplicationCalculator<TValue, TValue>
    {
    }

    public interface IMultiplicationCalculator<TLeftValue, in TRightValue>
    {
        TLeftValue Multiply(TLeftValue left, TRightValue right);
    }

    public interface IDivisionCalculator<TValue> : IDivisionCalculator<TValue, TValue>
    {
    }

    public interface IDivisionCalculator<TLeftValue, in TRightValue>
    {
        TLeftValue Divide(TLeftValue left, TRightValue right);
    }

    public interface IModulationCalculator<TValue> : IModulationCalculator<TValue, TValue>
    {
    }

    public interface IModulationCalculator<TLeftValue, in TRightValue>
    {
        TLeftValue Modulo(TLeftValue left, TRightValue right);
    }

    public interface IFourArithmeticCalculator<TValue> : IFourArithmeticCalculator<TValue, TValue>
    {
    }

    public interface IFourArithmeticCalculator<TLeftValue, in TRightValue> : IAdditionCalculator<TLeftValue, TRightValue>, ISubtractionCalculator<TLeftValue, TRightValue>, IMultiplicationCalculator<TLeftValue, TRightValue>, IDivisionCalculator<TLeftValue, TRightValue>
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
