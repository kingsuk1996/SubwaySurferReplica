using UnityEngine;
using System;

public static class ColorConverter
{
    /// <summary>
    /// Convert any hexadecimal code to color
    /// </summary>
    /// <param name="hexCode"></param>
    /// <returns></returns>
    public static Color GetColorFromHexCode(string hexCode)
    {
        float red = HexadecimalToFloatNormalized(hexCode.Substring(0, 2));
        float green = HexadecimalToFloatNormalized(hexCode.Substring(2, 2));
        float blue = HexadecimalToFloatNormalized(hexCode.Substring(4, 2));

        return new Color(red, green, blue);
    }

    /// <summary>
    /// Convert hexadecimal to decimal
    /// </summary>
    /// <param name="hexadecimal"></param>
    /// <returns></returns>
    private static int HexadecimalToDecimal(string hexadecimal)
    {
        int dec = Convert.ToInt32(hexadecimal, 16);
        return dec;
    }

    /// <summary>
    /// Convert decimal to hexadecimal by 2 digits
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private static string DecimalToHexaDecimal(int value)
    {
        return value.ToString("X2");
    }

    /// <summary>
    /// Convert a float to hexadecimal normalized
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private static string FloatNormalizedToHexadecimal(float value)
    {
        return DecimalToHexaDecimal(Mathf.RoundToInt(value * 255f));
    }

    /// <summary>
    /// Convert hexadecimal to float normalized
    /// </summary>
    /// <param name="hexadecimal"></param>
    /// <returns></returns>
    private static float HexadecimalToFloatNormalized(string hexadecimal)
    {
        return HexadecimalToDecimal(hexadecimal) / 255f;
    }
}
