using UnityEngine;

namespace UniFlow.Connector.Calculator
{
    [AddComponentMenu("UniFlow/Calculator/StringCalculator", (int) ConnectorType.StringCalculator)]
    public class StringCalculator : CalculatorBase<string, StringCollector>, IAdditionCalculator<string>
    {
        string IAdditionCalculator<string, string>.Add(string left, string right)
        {
            return left + right;
        }
    }
}
