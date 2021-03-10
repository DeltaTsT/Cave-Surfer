using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLASS_Math
{
    public static class MAIN
    {
        public static double Get_Standard_Deviation(List<double> Input_Numbers)
        {
            double Mean = Get_Mean(Input_Numbers);
            double STD_Dev = 0;

            foreach (double Number in Input_Numbers)
            { STD_Dev = STD_Dev + Math.Pow((Number - Mean), 2); }

            STD_Dev = Math.Sqrt(STD_Dev / (Input_Numbers.Count - 1));

            return STD_Dev;
        }

        public static double Get_Mean(List<double> Input_Numbers)
        {
            double Mean = 0;
            foreach (double Number in Input_Numbers)
            { Mean = Mean + Number; }
            Mean = Mean / (Input_Numbers.Count());
            return Mean;
        }
    }
}
