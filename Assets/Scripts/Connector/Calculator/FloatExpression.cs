using UnityEngine;

namespace UniFlow.Connector.Calculator
{
    [AddComponentMenu("UniFlow/Calculator/FloatCalculator", (int) ConnectorType.FloatCalculator)]
    public class FloatCalculator : CalculatorBase<float, FloatCollector>, IFourArithmeticCalculator<float>, IModulationCalculator<float>
    {
        float IAdditionCalculator<float, float>.Add(float left, float right)
        {
            return left + right;
        }

        float ISubtractionCalculator<float, float>.Subtract(float left, float right)
        {
            return left - right;
        }

        float IMultiplicationCalculator<float, float>.Multiply(float left, float right)
        {
            return left * right;
        }

        float IDivisionCalculator<float, float>.Divide(float left, float right)
        {
            return left / right;
        }

        float IModulationCalculator<float, float>.Modulo(float left, float right)
        {
            return left % right;
        }
    }
}
