using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OCL1_Proyecto1_201800519
{
    public class Graphviz
    {
        String ruta;
        StringBuilder grafo;


        public Graphviz()
        {
            ruta = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

        }

        public void generarDot(String rdot, String rpng)
        {
            System.IO.File.WriteAllText(rdot, grafo.ToString());
            String comandoDot = "dot.exe -Tpng " + rdot + " -o " + rpng + " ";
            var comando = string.Format(comandoDot);
            var procStart = new System.Diagnostics.ProcessStartInfo("cmd", "/C" + comando);
            var proc = new System.Diagnostics.Process();
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo = procStart;
            proc.Start();
            proc.WaitForExit();

        }

        public void graph_automaton( string text)
        {

            grafo = new StringBuilder();
            grafo.Append(text);

            string filename = ruta + "\\afn" + Form1.contador_afn + ".dot";
            if (File.Exists(filename)) {
                File.Delete(filename);
            }

            string filename2 = ruta + "\\afn" + Form1.contador_afd + ".png";
            if (File.Exists(filename2))
            {
                File.Delete(filename2);
            }

            generarDot(ruta + "\\afn" +Form1.contador_afn+".dot", ruta + "\\afn" + Form1.contador_afn + ".png");
            Form1.contador_afn++;
        }

        public void graph_automaton2(string text)
        {

            grafo = new StringBuilder();
            grafo.Append(text);
            string filename = ruta + "\\afd" + Form1.contador_afd + ".dot";
            if (File.Exists(filename))
            {
                File.Delete(filename);


            }
            string filename2 = ruta + "\\afd" + Form1.contador_afd + ".png";
            if (File.Exists(filename2)) {
                File.Delete(filename2);
            }

            generarDot(ruta + "\\afd" + Form1.contador_afd + ".dot", ruta + "\\afd" + Form1.contador_afd + ".png");
            Form1.contador_afd++;
        }


    }
}
