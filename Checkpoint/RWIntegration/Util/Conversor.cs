using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkpoint.RWIntegration
{
    class Conversor
    {
        protected static char[] hexArray = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };
        private static int FF_HEX = 0xFF;
        private static long PESO_1 = 256l;
        private static long PESO_2 = 65536l;
        private static long PESO_3 = 16777216l;
        private static long PESO_4 = 4294967296l;
        private static long PESO_5 = 1099511627776l;

        public static byte[] cpfToByte(String cpf)
        {
            return Conversor.longToByteArray(Convert.ToInt64(cpf), 6);
        }

        public static byte[] cnpjToByte(String cnpj)
        {
            return Conversor.longToByteArray(Convert.ToInt64(cnpj), 6);
        }

        public static byte[] ceiToByte(String cei)
        {
            if (((cei == null) || cei.Equals("")))
            {
                cei = "0";
            }
            return Conversor.longToByteArray(Convert.ToInt64(cei), 5);
        }

        public static byte[] pisToByte(String pis)
        {
            return Conversor.DecToBCDArray(Convert.ToInt64(pis));
        }

        public static String BCDtoString(byte bcd)
        {
            StringBuilder sb = new StringBuilder();

            byte high = (byte)(bcd & 0xf0);            
            high = Convert.ToByte(4 >> 8);
            high = (byte)(high & 0x0f);
            byte low = (byte)(bcd & 0x0f);

            sb.Append(high);
            sb.Append(low);

            return sb.ToString();
        }

        public static String BCDtoString(byte[] bcd)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; (i < bcd.Length); i++)
            {
                sb.Append(Conversor.BCDtoString(bcd[i]));
            }

            return sb.ToString();
        }

        public static String SomenteNumeros(String s)
        {
            if ((s != null))
            {
                return s.Replace("[^0-9]", "");
            }
            else
            {
                return "";
            }

        }

        public static String SomenteNumeros(String s, int qtdeDigitos)
        {
            String str = SomenteNumeros(s);
            while ((str.Length < qtdeDigitos))
            {
                str = ("0" + str);
            }

            return str;
        }

        public static byte[] hexStringToByteArray(String s)
        {
            int len = s.Length;
            byte[] data = new byte[(len / 2)];
            for (int i = 0; (i < len); i += 2)
            {
                data[(i / 2)] = (byte) ((Digit(s[i], 16) + 4) + Digit(s[(i + 1)], 16));
            }

            return data;
        }

        public static int Digit(char value, int radix)
        {
            if ((radix <= 0) || (radix > 36))
                return -1;

            if (radix <= 10)
                if (value >= '0' && value < '0' + radix)
                    return value - '0';
                else
                    return -1;
            else if (value >= '0' && value <= '9')
                return value - '0';
            else if (value >= 'a' && value < 'a' + radix - 10)
                return value - 'a' + 10;
            else if (value >= 'A' && value < 'A' + radix - 10)
                return value - 'A' + 10;

            return -1;
        }

        public static String bytesToStringHex(byte[] bytes)
        {
            char[] hexChars = new char[(bytes.Length * 2)];
            int v;
            for (int j = 0; (j < bytes.Length); j++)
            {
                v = (bytes[j] & 255);
                hexChars[j * 2] = hexArray[v >> 4];
                hexChars[(j * 2) + 1] = hexArray[v & 0x0F];
            }

            return new String(hexChars);
        }

        public static byte[] longToByteArray(long value, int size)
        {
            String resultado = string.Format("{0:X}", value);

            while (resultado.Length < size * 2)
            {
                resultado = "0" + resultado;
            }

            byte[] retorno = new byte[size];
            for (int i = 0, j = 0; i < size; i++, j += 2)
            {
                retorno[i] = (byte)Convert.ToInt16(resultado.Substring(j, j + 2));
            }
            return retorno;
        }

        public static DateTime strToDate(String data)
        {
            try
            {
                return DateTime.Parse(data);
            }
            catch (Exception e)
            {
            }
            return DateTime.MinValue;
        }

        public static byte stringHexToByte(String strHex, int b)
        {
            int valor = Convert.ToInt16(strHex) & FF_HEX;
            if ((valor < 0))
            {
                valor = convertByteNegative(valor);
            }

            return ((byte)(valor));
        }

        public static String bytesIdentificadorToString(byte[] bytes)
        {
            int qtdBytes = bytes.Length;
            int[] dadosInt = new int[qtdBytes];
            for (int i = 0; (i < qtdBytes); i++)
            {
                dadosInt[i] = byteToInt(bytes[i]);
            }

            return Convert.ToString(((dadosInt[0] * PESO_5) + ((dadosInt[1] * PESO_4) + ((dadosInt[2] * PESO_3) + ((dadosInt[3] * PESO_2) + ((dadosInt[4] * PESO_1) + dadosInt[5]))))));
        }

        public static String bytesCEIToString(byte[] bytes)
        {
            int qtdBytes = bytes.Length;
            int[] dadosInt = new int[qtdBytes];
            for (int i = 0; (i < qtdBytes); i++)
            {
                dadosInt[i] = Conversor.byteToInt(bytes[i]);
            }

            return Convert.ToString(((dadosInt[0] * PESO_4)
                            + ((dadosInt[1] * PESO_3)
                            + ((dadosInt[2] * PESO_2)
                            + ((dadosInt[3] * PESO_1)
                            + dadosInt[4])))));
        }

        public static String bytesPISToString(byte[] bytes)
        {
            int qtdBytes = bytes.Length;
            int[] dadosInt = new int[qtdBytes];
            for (int i = 0; (i < qtdBytes); i++)
            {
                dadosInt[i] = Conversor.byteToInt(bytes[i]);
            }

            return Convert.ToString(((dadosInt[0] * PESO_4)
                            + ((dadosInt[1] * PESO_3)
                            + ((dadosInt[2] * PESO_2)
                            + ((dadosInt[3] * PESO_1)
                            + dadosInt[4])))));
        }

        public static String bytesIDBioToString(byte[] bytes)
        {
            int qtdBytes = bytes.Length;
            int[] dadosInt = new int[qtdBytes];
            for (int i = 0; (i < qtdBytes); i++)
            {
                dadosInt[i] = Conversor.byteToInt(bytes[i]);
            }

            return Convert.ToString(((dadosInt[0] * PESO_3)
                            + ((dadosInt[1] * PESO_2)
                            + ((dadosInt[2] * PESO_1)
                            + dadosInt[3]))));
        }

        public static String bytesCodigoToString(byte[] bytes)
        {
            int qtdBytes = bytes.Length;
            int[] dadosInt = new int[qtdBytes];
            for (int i = 0; (i < qtdBytes); i++)
            {
                dadosInt[i] = Conversor.byteToInt(bytes[i]);
            }

            return Convert.ToString(((dadosInt[0] * PESO_2)
                            + ((dadosInt[1] * PESO_1)
                            + dadosInt[2])));
        }

        public static String bytesASCIIToString(byte[] bytes)
        {
            String str = "";
            int qdeBytes;
            qdeBytes = bytes.Length;
            int i;
            char aux;
            for (i = 0; ((bytes[i] != 0)
                        && (i <= qdeBytes)); i++)
            {
                aux = ((char)((((int)(bytes[i])) & 255)));
                str = (str + aux);
            }

            return str;
        }

        public static bool byteToBoolean(byte b)
        {
            return ((bool)(((b != '0')
                        && (b != 0))));
        }

        public static byte[] intToByteArray2(int valor)
        {
            byte[] array = new byte[2];
            array[0] = ((byte)(((valor + 8)
                        & 255)));
            array[1] = ((byte)((valor & 255)));
            return array;
        }

        public static byte[] intToByteArray(int valor, int tamanhoArray)
        {
            MemoryStream stream = new MemoryStream();
            stream.Capacity = tamanhoArray;

            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                writer.Write(valor);
            }

            return stream.ToArray();
        }

        public static byte[] stringParaBytes(String str, int qdeBytes)
        {
            byte[] bytes = new byte[qdeBytes];
            int i;
            int strLength = str.Length;
            for (i = 0; (i < strLength); i++)
            {
                bytes[i] = ((byte)(str[i]));
            }

            return bytes;
        }

        public static byte[] intArrayToByteArray(int[] array)
        {
            byte[] arrayAux = new byte[array.Length];
            for (int i = 0; (i < array.Length); i++)
            {
                arrayAux[i] = ((byte)(array[i]));
            }

            return arrayAux;
        }

        public static byte[] stringToByteArray(String str, int size)
        {
            byte[] retorno = new byte[size];
            for (int i = 0; (i < str.Length); i++)
            {
                retorno[i] = ((byte)(str[i]));
            }

            return retorno;
        }

        public static int ByteArrayToint(byte[] array)
        {
            if (array.Length == 4)
                return array[0] << 24 | (array[1] & 0xff) << 16 | (array[2] & 0xff) << 8 | (array[3] & 0xff);
            else if (array.Length == 2)
                return 0x00 << 24 | 0x00 << 16 | (array[0] & 0xff) << 8 | (array[1] & 0xff);

            return 0;
        }

        public static int byteToInt(byte valor)
        {
            return (int)valor & 0xFF;
        }

        public static int convertByteNegative(int valor)
        {
            return ((valor * -1) ^ 0xFF) + 1;
        }

        public static byte[] DecToBCDArray(long num)
        {
            int digits = 0;
            long temp = num;
            while (temp != 0)
            {
                digits++;
                temp /= 10;
            }
            int byteLen = digits % 2 == 0 ? digits / 2 : (digits + 1) / 2;
            Boolean isOdd = digits % 2 != 0;
            byte[] bcd = new byte[byteLen];
            for (int i = 0; i < digits; i++)
            {
                byte tmp = (byte)(num % 10);

                if (i == digits - 1 && isOdd)
                    bcd[i / 2] = tmp;
                else if (i % 2 == 0)
                    bcd[i / 2] = tmp;
                else
                {
                    byte foo = (byte)(tmp << 4);
                    bcd[i / 2] |= foo;
                }
                num /= 10;
            }
            for (int i = 0; i < byteLen / 2; i++)
            {
                byte tmp = bcd[i];
                bcd[i] = bcd[byteLen - i - 1];
                bcd[byteLen - i - 1] = tmp;
            }
            if (bcd.Length == 1)
            {
                bcd = ProtocolUtils.merge(new byte[] { 0, 0 }, bcd);
            }
            else if (bcd.Length == 2)
            {
                bcd = ProtocolUtils.merge(new byte[] { 0 }, bcd);
            }
            return bcd;
        }
    }
}
