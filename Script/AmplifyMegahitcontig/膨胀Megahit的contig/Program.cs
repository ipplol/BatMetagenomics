using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AmplifyMegahitcontig
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string contigFa = "D://MAG_JX/T.contigs.fa";//input
            string resultFa = "D://MAG_JX/T.fa";//output

            Dictionary<string, string> taxdic = new Dictionary<string, string>();
            int i, j, k;

            StreamReader read = new StreamReader(contigFa);
            StreamWriter write = new StreamWriter(resultFa);
            string line = read.ReadLine();
            string fa = read.ReadLine();
            while (line != null)
            {
                string[] line1 = line.Split(' ');
                string[] line2 = line1[3].Split('=');
                    string[] multi = line1[2].Split('=');
                    int count = (int)Convert.ToDouble(multi[1]);
                    if (Convert.ToInt32(line2[1]) >= 500)
                    {
                        i = 0;
                        for (i = 0; i < count; i++)
                        {
                        write.Write(">R" + i + "_" + line.Substring(1, line.Length - 1) + "\n");
                        write.Write(fa + "\n");
                        }
                    }
                line = read.ReadLine();
                fa = read.ReadLine();
            }
            read.Close();
            write.Close();
        }
    }
}
