using System;

namespace DataEncoding.JSON
{
    public class JSONNumber : JSONBase
    {
        public double Content { get; set; }

        public JSONNumber()
        { }

        public JSONNumber(double number)
        {
            Content = number;
        }

        public override string Encode()
        {
            if (Content != double.PositiveInfinity && Content != double.NegativeInfinity && !double.IsNaN(Content))
                return Content.ToString().Replace(',', '.');
            else
                throw new ArgumentException("Encoded number cannot be infinity nor NaN.");
        }

        public override int Decode(string json, int start)
        {
            string numberStr = TakeOutNumber(json, start, out int lastIndex);

            if (numberStr != null)
            {
                numberStr = numberStr.Replace('.', ',').Replace('E', 'e');

                if (numberStr.Contains("e"))
                {
                    string[] numbers = numberStr.Split('e');

                    if (numbers.Length > 2)
                        throw new ArgumentException("The detected number has more than one exponent.");

                    Content = Convert.ToDouble(numbers[0]) * Math.Pow(10, Convert.ToDouble(numbers[1]));
                }
                else
                {
                    Content = Convert.ToDouble(numberStr);
                }

                return lastIndex + 1;
            }
            else
                throw new ArgumentException("No number was found in " + nameof(json) + ".");
        }

        private static string TakeOutNumber(string json, int start, out int lastIndex)
        {
            int numberStart = -1;
            for (int i = start; i < json.Length; i++)
            {
                if ((json[i] >= 48 && json[i] <= 57) || json[i] == '+' || json[i] == '-')
                {
                    numberStart = i;
                    break;
                }
            }

            if (numberStart != -1)
            {
                int numberEnd = -1;
                for (int i = numberStart; i < json.Length; i++)
                {
                    if ((json[i] >= 48 && json[i] <= 57) || json[i] == 'e' || json[i] == 'E' || json[i] == '.' || json[i] == '+' || json[i] == '-')
                    {
                        if (i + 1 < json.Length)
                            continue;
                        else
                            numberEnd = json.Length - 1;
                    }
                    else
                    {
                        numberEnd = i - 1;
                        break;
                    }
                }

                if (numberEnd != -1)
                {
                    lastIndex = numberEnd;
                    return json.Substring(numberStart, numberEnd - numberStart + 1);
                }
            }

            lastIndex = -1;
            return null;
        }
    }
}
