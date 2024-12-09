using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

namespace RefDenovoMixAssembly
{
    public class position
    {
        public int ref_pos;
        public int depth;
        public string ref_base;
        public List<string> baseList = new List<string>();
        public List<int> baseReads = new List<int>();
        public string Rawline;
    }
    internal class Program
    {
        static List<position> contigReadcounts = new List<position>();
        static List<position> readsReadcounts = new List<position>();
        
        static void Readin(string contigfile, string readsfile)
        {
            contigReadcounts.Add(new position());
            readsReadcounts.Add(new position());

            int i, j, k;
            string line;

            //read contig count
            StreamReader read = new StreamReader(contigfile);
            line = read.ReadLine();
            line = read.ReadLine();
            while(line!=null)
            {
                string[] line1 = line.Split('\t');
                position newp = new position();
                newp.Rawline = line;
                newp.ref_pos = Convert.ToInt32(line1[1]);
                newp.ref_base = line1[2];
                newp.depth = Convert.ToInt32(line1[3]);
                for(i=5;i<line1.Length;i++)
                {
                    if (line1[i] != "")
                    {
                        string[] b1 = line1[i].Split(':');
                        newp.baseList.Add(b1[0]);
                        newp.baseReads.Add(Convert.ToInt32(b1[1]));
                    }
                }
                //Sort by reads
                string tmps;
                int tmpi;
                for(i=0;i<newp.baseList.Count;i++)
                    for(j=i+1;j<newp.baseList.Count;j++)
                        if (newp.baseReads[i] < newp.baseReads[j])
                        {
                            tmps = newp.baseList[i]; newp.baseList[i] = newp.baseList[j]; newp.baseList[j] = tmps;
                            tmpi = newp.baseReads[i]; newp.baseReads[i] = newp.baseReads[j]; newp.baseReads[j] = tmpi;
                        }
                contigReadcounts.Add(newp);
                line = read.ReadLine();
            }

            //read reads count
            read = new StreamReader(readsfile);
            line = read.ReadLine();
            line = read.ReadLine();
            while (line != null)
            {
                string[] line1 = line.Split('\t');
                position newp = new position();
                newp.Rawline = line;
                newp.ref_pos = Convert.ToInt32(line1[1]);
                newp.ref_base = line1[2];
                newp.depth = Convert.ToInt32(line1[3]);
                for (i = 5; i < line1.Length; i++)
                {
                    if (line1[i] != "")
                    {
                        string[] b1 = line1[i].Split(':');
                        newp.baseList.Add(b1[0]);
                        newp.baseReads.Add(Convert.ToInt32(b1[1]));
                    }
                }
                //Sort by reads
                string tmps;
                int tmpi;
                for (i = 0; i < newp.baseList.Count; i++)
                    for (j = i + 1; j < newp.baseList.Count; j++)
                        if (newp.baseReads[i] < newp.baseReads[j])
                        {
                            tmps = newp.baseList[i]; newp.baseList[i] = newp.baseList[j]; newp.baseList[j] = tmps;
                            tmpi = newp.baseReads[i]; newp.baseReads[i] = newp.baseReads[j]; newp.baseReads[j] = tmpi;
                        }
                readsReadcounts.Add(newp);
                line = read.ReadLine();
            }

            read.Close();
        }
        static void Output(string outfile)
        {
            //合并两个readscount 文件，以contig为主，contig没有覆盖度或者未能成为主碱基则替换为read
            StreamWriter write = new StreamWriter(outfile);
            write.Write("chrom\tposition\tref_base\tdepth\tq20_depth\tbase:reads:strands:avg_qual:map_qual:plus_reads:minus_reads\n");
            int i, j, k;
            for(i=1;i<contigReadcounts.Count;i++)
            {
                k = 0;
                if (contigReadcounts[i].baseReads[0] > 0)
                {
                    k++;
                    if (Convert.ToDouble(contigReadcounts[i].baseReads[0]) / contigReadcounts[i].depth > 0.5) k++;
                }
                if (k >= 2) write.Write(contigReadcounts[i].Rawline + "\n");
                else
                    write.Write(readsReadcounts[i].Rawline + "\n");
            }
            write.Close();
        }
        static void Main(string[] args)
        {
            Readin("R://BAT/Rawdata/WH/megahit_StaphylococcusNepalensis/CombineMapDenovo/contigMapRef.readcounts", "R://BAT/Rawdata/WH/readsMapRef.readcounts");
            Output("R://BAT/Rawdata/WH/megahit_StaphylococcusNepalensis/CombineMapDenovo/Merged.readcounts");
        }
    }
}
