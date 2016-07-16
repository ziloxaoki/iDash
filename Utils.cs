using System;
using System.Text;
using System.Text.RegularExpressions;

namespace iDash
{

    public class Utils
    {
        public const char LIST_SEPARATOR = ',';
        public const char ITEM_SEPARATOR = ';';
        public const char SIGN_EQUALS = '=';
        public const char SIGN_AMPERSAND = '&';

        public const byte TM1637_COLON_BIT = 128;
        //public const byte TM1637_CHAR_SPACE = 0;
        public const byte TM1637_CHAR_SPACE = TM1637_CHAR_S_COLON;
        public const byte TM1637_CHAR_EXC = 6;
        public const byte TM1637_CHAR_D_QUOTE = 34;
        public const byte TM1637_CHAR_POUND = 118;
        public const byte TM1637_CHAR_DOLLAR = 109;
        public const byte TM1637_CHAR_PERC = 36;
        public const byte TM1637_CHAR_AMP = 127;
        public const byte TM1637_CHAR_S_QUOTE = 32;
        public const byte TM1637_CHAR_L_BRACKET = 57;
        public const byte TM1637_CHAR_R_BRACKET = 15;
        public const byte TM1637_CHAR_STAR = 92;
        public const byte TM1637_CHAR_PLUS = 80;
        public const byte TM1637_CHAR_COMMA = 16;
        public const byte TM1637_CHAR_MIN = 64;
        public const byte TM1637_CHAR_DOT = 8;
        public const byte TM1637_CHAR_F_SLASH = 6;
        public const byte TM1637_CHAR_0 = 63;
        public const byte TM1637_CHAR_1 = 6;
        public const byte TM1637_CHAR_2 = 91;
        public const byte TM1637_CHAR_3 = 79;
        public const byte TM1637_CHAR_4 = 102;
        public const byte TM1637_CHAR_5 = 109;
        public const byte TM1637_CHAR_6 = 125;
        public const byte TM1637_CHAR_7 = 7;
        public const byte TM1637_CHAR_8 = 127;
        public const byte TM1637_CHAR_9 = 111;
        public const byte TM1637_CHAR_COLON = 48;
        public const byte TM1637_CHAR_S_COLON = 48;
        public const byte TM1637_CHAR_LESS = 88;
        public const byte TM1637_CHAR_EQUAL = 72;
        public const byte TM1637_CHAR_GREAT = 76;
        public const byte TM1637_CHAR_QUEST = 83;
        public const byte TM1637_CHAR_AT = 95;
        public const byte TM1637_CHAR_A = 119;
        public const byte TM1637_CHAR_B = 127;
        public const byte TM1637_CHAR_C = 57;
        public const byte TM1637_CHAR_D = 94;
        public const byte TM1637_CHAR_E = 121;
        public const byte TM1637_CHAR_F = 113;
        public const byte TM1637_CHAR_G = 61;
        public const byte TM1637_CHAR_H = 118;
        public const byte TM1637_CHAR_I = 6;
        public const byte TM1637_CHAR_J = 14;
        public const byte TM1637_CHAR_K = 117;
        public const byte TM1637_CHAR_L = 56;
        public const byte TM1637_CHAR_M = 21;
        public const byte TM1637_CHAR_N = 55;
        public const byte TM1637_CHAR_O = 63;
        public const byte TM1637_CHAR_P = 115;
        public const byte TM1637_CHAR_Q = 103;
        public const byte TM1637_CHAR_R = 51;
        public const byte TM1637_CHAR_S = 109;
        public const byte TM1637_CHAR_T = 120;
        public const byte TM1637_CHAR_U = 62;
        public const byte TM1637_CHAR_V = 28;
        public const byte TM1637_CHAR_W = 42;
        public const byte TM1637_CHAR_X = 118;
        public const byte TM1637_CHAR_Y = 110;
        public const byte TM1637_CHAR_Z = 91;
        public const byte TM1637_CHAR_L_S_BRACKET = 57;
        public const byte TM1637_CHAR_B_SLASH = 48;
        public const byte TM1637_CHAR_R_S_BRACKET = 15;
        public const byte TM1637_CHAR_A_CIRCUM = 19;
        public const byte TM1637_CHAR_UNDERSCORE = 8;
        public const byte TM1637_CHAR_A_GRAVE = 16;
        public const byte TM1637_CHAR_a = 95;
        public const byte TM1637_CHAR_b = 124;
        public const byte TM1637_CHAR_c = 88;
        public const byte TM1637_CHAR_d = 94;
        public const byte TM1637_CHAR_e = 123;
        public const byte TM1637_CHAR_f = 113;
        public const byte TM1637_CHAR_g = 111;
        public const byte TM1637_CHAR_h = 116;
        public const byte TM1637_CHAR_i = 4;
        public const byte TM1637_CHAR_j = 12;
        public const byte TM1637_CHAR_k = 117;
        public const byte TM1637_CHAR_l = 48;
        public const byte TM1637_CHAR_m = 21;
        public const byte TM1637_CHAR_n = 84;
        public const byte TM1637_CHAR_o = 92;
        public const byte TM1637_CHAR_p = 115;
        public const byte TM1637_CHAR_q = 103;
        public const byte TM1637_CHAR_r = 80;
        public const byte TM1637_CHAR_s = 109;
        public const byte TM1637_CHAR_t = 120;
        public const byte TM1637_CHAR_u = 28;
        public const byte TM1637_CHAR_v = 28;
        public const byte TM1637_CHAR_w = 42;
        public const byte TM1637_CHAR_x = 118;
        public const byte TM1637_CHAR_y = 102;
        public const byte TM1637_CHAR_z = 91;
        public const byte TM1637_CHAR_L_ACCON = 121;
        public const byte TM1637_CHAR_BAR = 6;
        public const byte TM1637_CHAR_R_ACCON = 79;
        public const byte TM1637_CHAR_TILDE = 64;

        public static byte convertByteTo7Segment(byte b)
        {
            switch (b)
            {
                case (byte)'.':
                    return TM1637_COLON_BIT; // 128
                case (byte)' ':
                    return TM1637_CHAR_SPACE; // 0
                case (byte)'!':
                    return TM1637_CHAR_EXC; // 6
                case (byte)'"':
                    return TM1637_CHAR_D_QUOTE; // 34
                case (byte)'#':
                    return TM1637_CHAR_POUND; // 118
                case (byte)'$':
                    return TM1637_CHAR_DOLLAR; // 109
                case (byte)'%':
                    return TM1637_CHAR_PERC; // 36
                case (byte)'&':
                    return TM1637_CHAR_AMP; // 127
                case (byte)'\'':
                    return TM1637_CHAR_S_QUOTE; // 32
                case (byte)'(':
                    return TM1637_CHAR_L_BRACKET; // 57
                case (byte)')':
                    return TM1637_CHAR_R_BRACKET; // 15
                case (byte)'*':
                    return TM1637_CHAR_STAR; // 92
                case (byte)'+':
                    return TM1637_CHAR_PLUS; // 80
                case (byte)',':
                    return TM1637_CHAR_COMMA; // 16
                case (byte)'-':
                    return TM1637_CHAR_MIN; // 64
                //case (byte)'.':
                //    return TM1637_CHAR_DOT; // 8
                case (byte)'/':
                    return TM1637_CHAR_F_SLASH; // 6
                case (byte)'0':
                    return TM1637_CHAR_0; // 63
                case (byte)'1':
                    return TM1637_CHAR_1; // 6
                case (byte)'2':
                    return TM1637_CHAR_2; // 91
                case (byte)'3':
                    return TM1637_CHAR_3; // 79
                case (byte)'4':
                    return TM1637_CHAR_4; // 102
                case (byte)'5':
                    return TM1637_CHAR_5; // 109
                case (byte)'6':
                    return TM1637_CHAR_6; // 125
                case (byte)'7':
                    return TM1637_CHAR_7; // 7
                case (byte)'8':
                    return TM1637_CHAR_8; // 127
                case (byte)'9':
                    return TM1637_CHAR_9; // 111
                case (byte)':':
                    return TM1637_CHAR_COLON; // 48
                case (byte)';':
                    return TM1637_CHAR_S_COLON; // 48
                case (byte)'<':
                    return TM1637_CHAR_LESS; // 88
                case (byte)'=':
                    return TM1637_CHAR_EQUAL; // 72
                case (byte)'>':
                    return TM1637_CHAR_GREAT; // 76
                case (byte)'?':
                    return TM1637_CHAR_QUEST; // 83
                case (byte)'@':
                    return TM1637_CHAR_AT; // 95
                case (byte)'A':
                    return TM1637_CHAR_A; // 119
                case (byte)'B':
                    return TM1637_CHAR_B; // 127
                case (byte)'C':
                    return TM1637_CHAR_C; // 57
                case (byte)'D':
                    return TM1637_CHAR_D; // 94
                case (byte)'E':
                    return TM1637_CHAR_E; // 121
                case (byte)'F':
                    return TM1637_CHAR_F; // 113
                case (byte)'G':
                    return TM1637_CHAR_G; // 61
                case (byte)'H':
                    return TM1637_CHAR_H; // 118
                case (byte)'I':
                    return TM1637_CHAR_I; // 6
                case (byte)'J':
                    return TM1637_CHAR_J; // 14
                case (byte)'K':
                    return TM1637_CHAR_K; // 117
                case (byte)'L':
                    return TM1637_CHAR_L; // 56
                case (byte)'M':
                    return TM1637_CHAR_M; // 21
                case (byte)'N':
                    return TM1637_CHAR_N; // 55
                case (byte)'O':
                    return TM1637_CHAR_O; // 63
                case (byte)'P':
                    return TM1637_CHAR_P; // 115
                case (byte)'Q':
                    return TM1637_CHAR_Q; // 103
                case (byte)'R':
                    return TM1637_CHAR_R; // 51
                case (byte)'S':
                    return TM1637_CHAR_S; // 109
                case (byte)'T':
                    return TM1637_CHAR_T; // 120
                case (byte)'U':
                    return TM1637_CHAR_U; // 62
                case (byte)'V':
                    return TM1637_CHAR_V; // 28
                case (byte)'W':
                    return TM1637_CHAR_W; // 42
                case (byte)'X':
                    return TM1637_CHAR_X; // 118
                case (byte)'Y':
                    return TM1637_CHAR_Y; // 110
                case (byte)'Z':
                    return TM1637_CHAR_Z; // 91
                case (byte)'[':
                    return TM1637_CHAR_L_S_BRACKET; // 57
                case (byte)'\\':
                    return TM1637_CHAR_B_SLASH; // 48
                case (byte)']':
                    return TM1637_CHAR_R_S_BRACKET; // 15
                case (byte)'^':
                    return TM1637_CHAR_A_CIRCUM; // 19
                case (byte)'_':
                    return TM1637_CHAR_UNDERSCORE; // 8
                case (byte)'`':
                    return TM1637_CHAR_A_GRAVE; // 16
                case (byte)'a':
                    return TM1637_CHAR_a; // 95
                case (byte)'b':
                    return TM1637_CHAR_b; // 124
                case (byte)'c':
                    return TM1637_CHAR_c; // 88
                case (byte)'d':
                    return TM1637_CHAR_d; // 94
                case (byte)'e':
                    return TM1637_CHAR_e; // 123
                case (byte)'f':
                    return TM1637_CHAR_f; // 113
                case (byte)'g':
                    return TM1637_CHAR_g; // 111
                case (byte)'h':
                    return TM1637_CHAR_h; // 116
                case (byte)'i':
                    return TM1637_CHAR_i; // 4
                case (byte)'j':
                    return TM1637_CHAR_j; // 12
                case (byte)'k':
                    return TM1637_CHAR_k; // 117
                case (byte)'l':
                    return TM1637_CHAR_l; // 48
                case (byte)'m':
                    return TM1637_CHAR_m; // 21
                case (byte)'n':
                    return TM1637_CHAR_n; // 84
                case (byte)'o':
                    return TM1637_CHAR_o; // 92
                case (byte)'p':
                    return TM1637_CHAR_p; // 115
                case (byte)'q':
                    return TM1637_CHAR_q; // 103
                case (byte)'r':
                    return TM1637_CHAR_r; // 80
                case (byte)'s':
                    return TM1637_CHAR_s; // 109
                case (byte)'t':
                    return TM1637_CHAR_t; // 120
                case (byte)'u':
                    return TM1637_CHAR_u; // 28
                case (byte)'v':
                    return TM1637_CHAR_v; // 28
                case (byte)'w':
                    return TM1637_CHAR_w; // 42
                case (byte)'x':
                    return TM1637_CHAR_x; // 118
                case (byte)'y':
                    return TM1637_CHAR_y; // 102
                case (byte)'z':
                    return TM1637_CHAR_z; // 91
                case (byte)'{':
                    return TM1637_CHAR_L_ACCON; // 121
                case (byte)'|':
                    return TM1637_CHAR_BAR; // 6
                case (byte)'}':
                    return TM1637_CHAR_R_ACCON; // 79
                case (byte)'~':
                    return TM1637_CHAR_TILDE; // 64
            }

            return 0;
        }


        public static string byteArrayToString(byte[] ba)
        {
            if (ba != null && ba.Length > 0)
            {
                string hex = BitConverter.ToString(ba);
                return hex.Replace("-", "");
            }

            return "empty";
        }

        public static void resetArray(byte[] array)
        {
            for(int i = 0; i < array.Length; i++)
            {
                array[i] = 0;
            }
        }

        public static T[] getSubArray<T>(T[] array, int from, int length)
        {
            T[] result = new T[length];
            Array.Copy(array, from, result, 0, length);

            return result;
        }

        public static long getCurrentTimeMillis()
        {
            return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        public static bool hasTimedOut(long startTime, long millisec)
        {
            return getCurrentTimeMillis() - startTime > millisec; 
        }

        public static string byteArrayToStr(byte[] array)
        {
            return System.Text.Encoding.Default.GetString(array);
        }

        public static byte[] getBytes(string str)
        {
            return Encoding.ASCII.GetBytes(str);
        }        

        public static int Count(byte[] array, byte val)
        {
            int result = 0;
            foreach (byte b in array) {
                if (b == val) result++;
            }

            return result;
        }

        public static byte[] convertByteTo7Segment(byte[] content, int offset)
        {
            byte[] result = null;
            int pos = 0;

            if (content != null) {
                var count = Count(content, (byte)'.');
                result = new byte[content.Length - offset - count];
                for (int x = offset; x < content.Length; x++)
                {
                    if (x < content.Length - 2 && content[x + 1] == (byte)'.')
                    {
                        result[pos++] = (byte)(convertByteTo7Segment(content[x++]) + 128);
                    } else
                    {
                        result[pos++] = convertByteTo7Segment(content[x]);
                    }
                }
            }

            return result;
        }


        public static string getIntBinaryString(int n)
        {
            char[] b = new char[8];
            int pos = 7;
            int i = 0;

            while (i < 8)
            {
                if ((n & (1 << i)) != 0)
                {
                    b[pos] = '1';
                }
                else
                {
                    b[pos] = '0';
                }
                pos--;
                i++;
            }
            return new string(b);
        }

        public static string formatString(string text, string pattern)
        {
            string[] index = pattern.Split(SIGN_EQUALS);

            if (index.Length > 1)
            {
                if (index[0].StartsWith("pl") || index[0].StartsWith("pr"))
                {
                    string[] par = index[1].Split(SIGN_AMPERSAND);
                    if (par.Length != 2)
                        return text;
                    if (index[0].StartsWith("pl"))
                        return text.PadLeft(Int32.Parse(par[0]), par[1][0]);
                    return text.PadRight(Int32.Parse(par[0]), par[1][0]);
                }
                if (index[0].StartsWith("@"))
                {
                    string value = Regex.Replace(text, index[0], index[1]);
                    return value;
                }
            }

            return String.Format(pattern, text);
        }
    }

    public enum DebugMode
    {
        None,
        Default,
        Verbose
    }
}
