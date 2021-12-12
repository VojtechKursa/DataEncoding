using System;

namespace DataEncoding.JSON
{
    public class JSONString : JSONBase
    {
        public string Content { get; set; } = null;

        public JSONString()
        { }

        public JSONString(string value)
        {
            Content = value;
        }

        public override string Encode()
        {
            string result = String.Format("\"{0}\"", Escape(Content));

            return result;
        }

        public override int Decode(string json, int start)
        {
            int stringStart = json.IndexOf('\"', start);
            if (stringStart == -1)
                throw new ArgumentException("Start of the string (\") not found in " + nameof(json) + ".");

            int stringEnd = FindEndOfString(json, stringStart + 1);

            if (stringEnd != -1)
                Content = Unescape(json.Substring(stringStart + 1, stringEnd - stringStart - 1));
            else
                throw new ArgumentException("End of the string not found in " + nameof(json) + ".");

            return stringEnd + 1;
        }

        private static int FindEndOfString(string json, int start)
        {
            for (int i = start > 0 ? start : 1; i < json.Length; i++)
            {
                if (json[i] == '\"')
                {
                    if (json[i - 1] != '\\')
                        return i;
                }
            }

            return -1;
        }

        private static string Escape(string unescaped)
        {
            string result = "";

            foreach (char character in unescaped)
            {
                switch (character)
                {
                    case '\"':
                    case '\\': result += "\\" + character; break;
                    case (char)0x8: result += "\\b"; break;
                    case (char)0xC: result += "\\f"; break;
                    case (char)0xA: result += "\\n"; break;
                    case (char)0xD: result += "\\r"; break;
                    case (char)0x9: result += "\\t"; break;
                    default:
                        if (character < 0x20)
                            result += UnicodeEscape(character);
                        else
                            result += character;
                        break;
                }
            }

            return result;
        }

        private static string Unescape(string escaped)
        {
            string result = "";

            for (int i = 0; i < escaped.Length; i++)
            {
                if (escaped[i] != '\\')
                    result += escaped[i];
                else
                {
                    if (i + 1 < escaped.Length)
                    {
                        switch (escaped[i + 1])
                        {
                            case '\"':
                            case '\\':
                            case '/': result += escaped[i + 1]; break;
                            case 'b': result += (char)0x8; break;
                            case 'f': result += (char)0xC; break;
                            case 'n': result += (char)0xA; break;
                            case 'r': result += (char)0xD; break;
                            case 't': result += (char)0x9; break;
                            case 'u':
                                bool isPair;
                                result += UnicodeUnescape(escaped, i, out isPair);

                                if (isPair)
                                    i += 10;
                                else
                                    i += 4;

                                break;
                            default:
                                throw new ArgumentException("Unknown escape sequence detected.");
                        }

                        i++;
                    }
                    else
                        throw new ArgumentException("Start of an escape sequence (\\) detected at the end of string.");
                }
            }

            return result;
        }

        private static string UnicodeEscape(char character)
        {
            if (character > 0xFFFF)
            {
                int temp = character - 0x10000;
                int highSurrogate = 0xD800 | ((temp & 0xFFC00) >> 10);
                int lowSurrogate = 0xDC00 | (temp & 0x3FF);

                return String.Format("{0}{1}{0}{2}", "\\u", Convert.ToString(highSurrogate, 16), Convert.ToString(lowSurrogate, 16));
            }
            else
            {
                return "\\u" + Convert.ToString((int)character, 16).PadLeft(4, '0');
            }
        }

        private static char UnicodeUnescape(string sequence, int startIndex, out bool isPair)
        {
            if (startIndex + 6 < sequence.Length)
            {
                if (sequence[startIndex] == '\\' && sequence[startIndex + 1] == 'u')
                {
                    int character = Convert.ToInt32(sequence.Substring(startIndex + 2, 4), 16);

                    if ((character & 0xFC00) != 0xD800)
                    {
                        isPair = false;
                        return (char)character;
                    }
                    else
                    {
                        if (startIndex + 12 < sequence.Length)
                        {
                            if (sequence[startIndex + 6] == '\\' && sequence[startIndex + 7] == 'u')
                            {
                                int higherSurrogate = character;
                                int lowerSurrogate = Convert.ToInt32(sequence.Substring(startIndex + 8, 4), 16);

                                if ((lowerSurrogate & 0xFC00) == 0xDC00)
                                {
                                    higherSurrogate &= 0x3FF;
                                    lowerSurrogate &= 0x3FF;

                                    character = ((higherSurrogate << 10) | lowerSurrogate) + 0x10000;

                                    isPair = true;
                                    return (char)character;
                                }
                                else
                                    throw new ArgumentException("Surrogate pair detected, but the lower surrogate is not a valid lower surrogate.");
                            }
                            else
                                throw new ArgumentException("Surrogate pair detected in " + nameof(sequence) + ", but only the higher surrogate is present.");
                        }
                        else
                            throw new ArgumentOutOfRangeException("Surrogate pair detected in " + nameof(sequence) + ", but the " + nameof(sequence) + " isn't long enough to contain a surrogate pair");
                    }

                }
                else
                    throw new ArgumentException(nameof(sequence) + " doesn't start with sequence \\u");
            }
            else
                throw new ArgumentOutOfRangeException("Position " + nameof(startIndex) + " in " + nameof(sequence) + " is not followed by enough characters to be an escaped Unicode character.");
        }
    }
}
