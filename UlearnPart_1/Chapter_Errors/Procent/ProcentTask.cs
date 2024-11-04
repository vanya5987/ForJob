using System;

namespace ProcentTask;

public class Calculate
{
    public static double Calculate(string userInput)
    {
        string[] commandList = userInput.Split(' ');

        double moneyCount = Convert.ToDouble(commandList[0]);
        double maxAnnualProcents = Convert.ToDouble(commandList[1]);
        double annualProcent = Convert.ToDouble(commandList[2]);

        double maxProcentCount = 100;
        double mounths = 12;
        double calculatedSum = 1 + (maxAnnualProcents / maxProcentCount) / mounths;

        return Convert.ToDouble(userInput = Convert.ToString(moneyCount * Math.Pow(calculatedSum, annualProcent)));
    }
}