using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkpoint.RWIntegration
{
    class ProtocolUtils
    {
        public static byte[] calcularChecksum(byte[] dados)
        {
            byte cs = 0;
            for (int i = 0; i < dados.Length; i++)
            {
                cs ^= dados[i];
            }
            dados = merge(dados, new byte[]{cs});
            return dados;
        }

        public static byte getChecksum(byte[] dados)
        {
            byte cs = 0;
            for (int i = 0; i < dados.Length; i++)
            {
                cs ^= dados[i];
            }
            return cs;
        }

        /*public static byte[] merge2(byte[] arrays)
        {
            int count = 0;
            foreach (byte[] array in arrays)
            {
                count = (count + array.Length);
            }

            byte[] mergedArray = new byte[count];
            int start = 0;

            foreach (byte[] array in arrays)
            {
                System.arraycopy(array, 0, mergedArray, start, array.Length);
                start = (start + array.Length);
            }

            return mergedArray;
        }*/

        public static byte[] merge(params byte[][] arrays)
        {
            byte[] rv = new byte[arrays.Sum(a => a.Length)];
            int offset = 0;
            foreach (byte[] array in arrays)
            {
                System.Buffer.BlockCopy(array, 0, rv, offset, array.Length);
                offset += array.Length;
            }
            return rv;
        }

        public static byte[] data()
        {
            byte[] data = new byte[4];
            DateTime dt = new DateTime();
            Calendar myCal = CultureInfo.InvariantCulture.Calendar;

            data[0] = 0;
            data[1] = ((byte)(Convert.ToInt16(Convert.ToString(myCal.GetDayOfMonth(dt) ), 2)));
            String bitsMes = Convert.ToString(myCal.GetMonth(dt));
            while ((bitsMes.Length < 4))
            {
                bitsMes = ("0" + bitsMes);
            }

            String bitsAnoFull = Convert.ToString(dt.Year);
            String bitsAnoAux = bitsAnoFull.Substring((bitsAnoFull.Length - 8), bitsAnoFull.Length);
            bitsAnoFull = bitsAnoFull.Substring(0, (bitsAnoFull.Length - bitsAnoAux.Length));
            while ((bitsAnoFull.Length < 4))
            {
                bitsAnoFull = ("0" + bitsAnoFull);
            }

            data[2] = ((byte)(Convert.ToInt16((bitsMes + bitsAnoFull), 2)));
            data[3] = ((byte)(Convert.ToInt16(bitsAnoAux, 2)));
            return data;
        }

        public static byte[] horario()
        {
            int[] hora = new int[4];
            DateTime dt = new DateTime();
            Calendar myCal = CultureInfo.InvariantCulture.Calendar;

            hora[0] = 0;
            hora[1] = myCal.GetSecond(dt);
            hora[2] = myCal.GetMinute(dt);
            hora[3] = myCal.GetHour(dt);
            return Conversor.intArrayToByteArray(hora);
        }

        public static byte[] copyOfRange(byte[] src, int start, int end)
        {
            int len = end - start;
            byte[] dest = new byte[len];
            // note i is always from 0
            for (int i = 0; i < len; i++)
            {
                dest[i] = src[start + i]; // so 0..n = 0+x..n+x
            }
            return dest;
        }
    }
}
