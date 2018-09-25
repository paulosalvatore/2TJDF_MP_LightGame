using System.Collections;
using System.Collections.Generic;
using System;

public static class FakeArduino
{
    #region ENUM

    public enum DataTypes
    {
        DIGITAL_PWM,
        ANALOG,
        BOOL
    }

    #endregion ENUM

    private static List<DataTypes> fakeDataTypes = new List<DataTypes>
        {
            DataTypes.BOOL,
            DataTypes.BOOL,
            DataTypes.BOOL,
            DataTypes.BOOL
        };

    private static Random random = new Random();

    public static string ReadLine()
    {
        string data = "";

        for (int i = 0; i < fakeDataTypes.Count; i++)
        {
            DataTypes fakeDataType = fakeDataTypes[i];

            switch (fakeDataType)
            {
                case DataTypes.DIGITAL_PWM:
                    // 0 - 255

                    data += random.Next(0, 256).ToString();

                    break;

                case DataTypes.ANALOG:
                    // 0 - 1023
                    data += random.Next(0, 1024).ToString();

                    break;

                case DataTypes.BOOL:
                    // 0 - 1

                    data += random.Next(0, 2).ToString();

                    break;

                default:
                    // Erro - Tipo não tratado

                    throw new Exception("Arduino Data Type não tratado.");
            }

            if (i < fakeDataTypes.Count - 1)
            {
                data += ";";
            }
        }

        return data;
    }
}