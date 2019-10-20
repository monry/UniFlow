using UnityEngine;

namespace UniFlow.Connector.Expression
{
    [AddComponentMenu("UniFlow/Expression/String", (int) ConnectorType.ExpressionString)]
    public class String : CalculatorBase<string, StringCollector>, IAdditionCalculator<string>
    {
        string IAdditionCalculator<string>.Add(string left, string right)
        {
            return left + right;
        }
    }
}
