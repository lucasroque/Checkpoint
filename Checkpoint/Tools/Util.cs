using Checkpoint.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Checkpoint.Tools
{
    public class Util
    {
        public static List<String> getStates()
        {
            List<String> states = new List<String>();

            states.Add("AC");
            states.Add("AL");
            states.Add("AM");
            states.Add("AP");
            states.Add("BA");
            states.Add("CE");
            states.Add("DF");
            states.Add("ES");
            states.Add("GO");
            states.Add("MA");
            states.Add("MG");
            states.Add("MS");
            states.Add("MT");
            states.Add("PA");
            states.Add("PB");
            states.Add("PE");
            states.Add("PI");
            states.Add("PR");
            states.Add("RJ");
            states.Add("RN");
            states.Add("RO");
            states.Add("RR");
            states.Add("RS");
            states.Add("SC");
            states.Add("SE");
            states.Add("SP");
            states.Add("TO");

            return states;
        }

        public static List<Recess> getStandardRecess()
        {
            List<Recess> standardRecess = new List<Recess>();
            Recess recess;

            recess = new Recess("Conf. Universal", new DateTime(DateTime.Now.Year, 1, 1));
            standardRecess.Add(recess);
            recess = new Recess("Tiradentes", new DateTime(DateTime.Now.Year, 4, 21));
            standardRecess.Add(recess);
            recess = new Recess("Dia do Trabalho", new DateTime(DateTime.Now.Year, 5, 1));
            standardRecess.Add(recess);
            recess = new Recess("Independência", new DateTime(DateTime.Now.Year, 9, 7));
            standardRecess.Add(recess);
            recess = new Recess("N. Sra. Aparecida", new DateTime(DateTime.Now.Year, 10, 12));
            standardRecess.Add(recess);
            recess = new Recess("Finados", new DateTime(DateTime.Now.Year, 11, 2));
            standardRecess.Add(recess);
            recess = new Recess("Proc. da República", new DateTime(DateTime.Now.Year, 11, 15));
            standardRecess.Add(recess);
            recess = new Recess("Natal", new DateTime(DateTime.Now.Year, 12, 25));
            standardRecess.Add(recess);
            
            return standardRecess;
        }

        public static bool contains(string paragraph, string word)
        {
            return paragraph.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
