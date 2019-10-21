using UnityEngine;

namespace UniFlow.Connector.Expression
{
    [AddComponentMenu("UniFlow/Expression/Int", (int) ConnectorType.ExpressionInt)]
    public class Int : CalculatorBase<int, IntCollector>, IFourArithmeticCalculator<int>, IModulationCalculator<int>
    {
        int IAdditionCalculator<int>.Add(int left, int right)
        {
            return left + right;
        }

        int ISubtractionCalculator<int>.Subtract(int left, int right)
        {
            return left - right;
        }

        int IMultiplicationCalculator<int>.Multiply(int left, int right)
        {
            return left * right;
        }

        int IDivisionCalculator<int>.Divide(int left, int right)
        {
            return left / right;
        }

        int IModulationCalculator<int>.Modulo(int left, int right)
        {
            return left % right;
        }
    }
}
