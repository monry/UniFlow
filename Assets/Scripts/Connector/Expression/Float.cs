using UnityEngine;

namespace UniFlow.Connector.Expression
{
    [AddComponentMenu("UniFlow/Expression/Float", (int) ConnectorType.ExpressionFloat)]
    public class Float : CalculatorBase<float, FloatCollector>, IFourArithmeticCalculator<float>, IModulationCalculator<float>
    {
        float IAdditionCalculator<float>.Add(float left, float right)
        {
            return left + right;
        }

        float ISubtractionCalculator<float>.Subtract(float left, float right)
        {
            return left - right;
        }

        float IMultiplicationCalculator<float>.Multiply(float left, float right)
        {
            return left * right;
        }

        float IDivisionCalculator<float>.Divide(float left, float right)
        {
            return left / right;
        }

        float IModulationCalculator<float>.Modulo(float left, float right)
        {
            return left % right;
        }
    }
}
