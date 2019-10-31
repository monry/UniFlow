using UnityEngine;

namespace UniFlow.Connector.Calculator
{
    [AddComponentMenu("UniFlow/Calculator/IntCalculator", (int) ConnectorType.IntCalculator)]
    public class IntCalculator : CalculatorBase<int, IntCollector>, IFourArithmeticCalculator<int>, IModulationCalculator<int>
    {
        int IAdditionCalculator<int, int>.Add(int left, int right)
        {
            return left + right;
        }

        int ISubtractionCalculator<int, int>.Subtract(int left, int right)
        {
            return left - right;
        }

        int IMultiplicationCalculator<int, int>.Multiply(int left, int right)
        {
            return left * right;
        }

        int IDivisionCalculator<int, int>.Divide(int left, int right)
        {
            return left / right;
        }

        int IModulationCalculator<int, int>.Modulo(int left, int right)
        {
            return left % right;
        }
    }
}
