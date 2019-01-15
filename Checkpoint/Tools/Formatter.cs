using System;
using System.Text.RegularExpressions;

namespace Checkpoint.Tools
{
    class Formatter
    {
        private static Formatter instance;

        private int PHONE_LIMIT_LENGTH = 11;
        private int CNPJ_LIMIT_LENGTH = 14;
        private int ZIP_CODE_LIMIT_LENGTH = 8;
        private int PIS_PASEP_LIMIT_LENGTH = 11;
        private int DATE_LIMIT_LENGTH = 8;
        private int HOUR_LIMIT_LENGTH = 4;

        public static Formatter getInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Formatter();
                }
                return instance;
            }
        }

        public string formatPhoneNumber(String phoneNumber, String lastPhoneNumber)
        {
            phoneNumber = phoneNumber.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");

            if (phoneNumber.Length <= PHONE_LIMIT_LENGTH)
            {
                if (phoneNumber.Length == 10)
                {
                    return Regex.Replace(phoneNumber, @"(\d{2})(\d{4})(\d{4})", "($1) $2-$3");
                }
                else if (phoneNumber.Length == 11)
                {
                    return Regex.Replace(phoneNumber, @"(\d{2})(\d{5})(\d{4})", "($1) $2-$3");
                } else
                {
                    return phoneNumber;
                }
            } else
            {
                return lastPhoneNumber;
            }
        }

        public string formatDate(String date, String lastDate)
        {
            date = date.Replace("/", "").Replace(" ", "");

            if (date.Length <= DATE_LIMIT_LENGTH)
            {
                if (date.Length == 6)
                {
                    return Regex.Replace(date, @"(\d{2})(\d{2})(\d{2})", "$1/$2/$3");
                }
                else if (date.Length == 8)
                {
                    return Regex.Replace(date, @"(\d{2})(\d{2})(\d{4})", "$1/$2/$3");
                }
                else
                {
                    return date;
                }
            }
            else
            {
                return lastDate;
            }
        }

        public string formatCnpj(String cnpj, String lastCnpj)
        {
            cnpj = cnpj.Replace(".", "").Replace("/", "").Replace("-", "").Replace(" ", "");

            if (cnpj.Length <= CNPJ_LIMIT_LENGTH)
            {
                if (cnpj.Length == CNPJ_LIMIT_LENGTH)
                {
                    return Regex.Replace(cnpj, @"(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})", "$1.$2.$3/$4-$5");
                } else
                {
                    return cnpj;
                }     
            }
            else
            {
                return lastCnpj;
            }
        }

        public string formatPisPasep(String pisPasep, String lastpisPasep)
        {
            pisPasep = pisPasep.Replace(".", "").Replace("-", "").Replace(" ", "");

            if (pisPasep.Length <= PIS_PASEP_LIMIT_LENGTH)
            {
                if (pisPasep.Length == PIS_PASEP_LIMIT_LENGTH)
                {
                    return Regex.Replace(pisPasep, @"(\d{3})(\d{5})(\d{2})(\d{1})", "$1.$2.$3-$4");
                }
                else
                {
                    return pisPasep;
                }
            }
            else
            {
                return lastpisPasep;
            }
        }

        public string formatZipCode(String zipCode, String lastZipCode)
        {
            zipCode = zipCode.Replace("-", "").Replace(" ", "");

            if (zipCode.Length <= ZIP_CODE_LIMIT_LENGTH)
            {
                if (zipCode.Length == ZIP_CODE_LIMIT_LENGTH)
                {
                    return Regex.Replace(zipCode, @"(\d{5})(\d{3})", "$1-$2");
                }
                else
                {
                    return zipCode;
                }
            }
            else
            {
                return lastZipCode;
            }
        }

        public string formatHour(String hour)
        {
            hour = hour.Replace(":", "").Replace(" ", "");

            if (hour.Length <= HOUR_LIMIT_LENGTH)
            {
                if (hour.Length == HOUR_LIMIT_LENGTH)
                {
                    return Regex.Replace(hour, @"(\d{2})(\d{2})", "$1:$2");
                }
                else
                {
                    return hour;
                }
            }
            return hour;
        }

    }
}