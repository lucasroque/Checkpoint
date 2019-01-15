
using System.Linq;

namespace Checkpoint.Tools
{
    class Inspector
    {
        private static Inspector instance;

        public static Inspector getInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Inspector();
                }
                return instance;
            }
        }

        public bool validateCnpj(string cnpj)
        {
            int[] multiply1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiply2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            int sum;
            int rest;
            string digit;
            string tempCnpj;

            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");

            if (cnpj.Length != 14)
            {
                return false;
            }

            if (cnpj.Equals("00000000000000"))
            {
                return false;
            }

            tempCnpj = cnpj.Substring(0, 12);
            sum = 0;

            for (int i = 0; i < 12; i++)
            {
                sum += int.Parse(tempCnpj[i].ToString()) * multiply1[i];
            }

            rest = (sum % 11);

            if (rest < 2)
            {
                rest = 0;
            }
            else
            {
                rest = 11 - rest;
            }

            digit = rest.ToString();
            tempCnpj = tempCnpj + digit;
            sum = 0;

            for (int i = 0; i < 13; i++)
            {
                sum += int.Parse(tempCnpj[i].ToString()) * multiply2[i];
            }

            rest = (sum % 11);

            if (rest < 2)
            {
                rest = 0;
            }
            else
            {
                rest = 11 - rest;
            }

            digit = digit + rest.ToString();

            return cnpj.EndsWith(digit);
        }

        public bool validatePis(string pis)
        {

            int[] multiply = new int[10] { 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int sum;
            int rest;

            pis = pis.Trim();
            pis = pis.Replace("-", "").Replace(".", "").PadLeft(11, '0');

            if (!pis.All(char.IsDigit))
            {
                return false;
            }

            if (pis.Trim().Length != 11)
            {
                return false;
            }

            sum = 0;
            for (int i = 0; i < 10; i++)
            {
                sum += int.Parse(pis[i].ToString()) * multiply[i];
            }

            rest = sum % 11;

            if (rest < 2)
            {
                rest = 0;
            }
            else
            {
                rest = 11 - rest;
            }

            return pis.EndsWith(rest.ToString());
        }

        public bool validateEmail(string email)
        {
            string strModelo = "^([0-9a-zA-Z]([-.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$";

            if (System.Text.RegularExpressions.Regex.IsMatch(email, strModelo))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

}
