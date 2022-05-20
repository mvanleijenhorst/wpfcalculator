using System;

namespace WpfCalculator.Services
{
    /// <summary>
    /// Calculator service.
    /// </summary>
    public static class CalculatorService
    {
        /// <summary>
        /// Calculator the sum.
        /// </summary>
        /// <param name="operator">Operator</param>
        /// <param name="firstNumber">First number</param>
        /// <param name="secondNumber">Second number</param>
        /// <returns>Result of the sum</returns>
        /// <exception cref="NotImplementedException">Occurs when the operator is not implemented</exception>
        public static decimal Calculate(char @operator, decimal firstNumber, decimal secondNumber)
        {
            return @operator switch
            {
                '+' => firstNumber + secondNumber,
                '-' => firstNumber - secondNumber,
                '*' => firstNumber * secondNumber,
                '/' => firstNumber / secondNumber,
                _ => throw new NotImplementedException($"Operator {@operator} is not implemented"),
            };
        }
    }
}
