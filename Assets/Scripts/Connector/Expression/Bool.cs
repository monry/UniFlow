using UnityEngine;

namespace UniFlow.Connector.Expression
{
    [AddComponentMenu("UniFlow/Expression/Bool", (int) ConnectorType.ExpressionBool)]
    public class Bool : CalculatorBase<bool, BoolCollector>, IAdditionCalculator<bool>, IMultiplicationCalculator<bool>
    {
        bool IAdditionCalculator<bool>.Add(bool left, bool right)
        {
            return left | right;
        }

        bool IMultiplicationCalculator<bool>.Multiply(bool left, bool right)
        {
            return left & right;
        }
    }
}
