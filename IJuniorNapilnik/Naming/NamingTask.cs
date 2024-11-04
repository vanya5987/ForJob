public static int Clamp(int minValue, int inputValue, int maxValue)
{
    if (inputValue < minValue)
        return minValue;
    else if (inputValue > maxValue)
        return maxValue;
    else
        return inputValue;
}