using UnityEngine;

namespace UniFlow.Connector.Calculator
{
    [AddComponentMenu("UniFlow/Calculator/BoolCalculator", (int) ConnectorType.BoolCalculator)]
    public class BoolCalculator : CalculatorBase<bool, BoolCollector>, IAdditionCalculator<bool>, IMultiplicationCalculator<bool>
    {
        bool IAdditionCalculator<bool, bool>.Add(bool left, bool right)
        {
            return left | right;
        }

        bool IMultiplicationCalculator<bool, bool>.Multiply(bool left, bool right)
        {
            return left & right;
        }
    }
}
