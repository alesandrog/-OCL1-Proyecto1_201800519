using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _OCL1_Proyecto1_201800519
{
    public partial class Form1 : Form
    {

        public static int contador_afn = 0;
        public static int contador_afd = 0;
        public static LinkedList<Automaton> automatons = new LinkedList<Automaton>();
        bool afn_state = false;
        int contador = 0;
        static Parser pa = new Parser();


        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private RichTextBox getRtb()
        {
            RichTextBox rtb = null;
            TabPage tab = xuiFlatTab1.SelectedTab;
            if (tab != null)
            {
                rtb = tab.Controls[0] as RichTextBox;
            }

            return rtb;
        }

        private void pestanaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TabPage tab = new TabPage("New Tab");
            RichTextBox rtb = new RichTextBox();
            rtb.Dock = DockStyle.Fill;
            rtb.BackColor = Color.AliceBlue;
            rtb.ForeColor = Color.Black;
            tab.Controls.Add(rtb);
            xuiFlatTab1.TabPages.Add(tab);
        }

        private void cargarArchivoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog abrir = new OpenFileDialog();
            var resultado = abrir.ShowDialog();
            if (resultado == DialogResult.OK)
            {
                TabPage tab = new TabPage(abrir.SafeFileName.ToString());
                RichTextBox rtb = new RichTextBox();
                rtb.Dock = DockStyle.Fill;
                rtb.BackColor = Color.AliceBlue;
                rtb.ForeColor = Color.Black;
                tab.Controls.Add(rtb);
                xuiFlatTab1.TabPages.Add(tab);
                rtb.LoadFile(abrir.FileName.ToString(), RichTextBoxStreamType.PlainText);

            }
        }

        private void guardaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog guardar = new SaveFileDialog();
            guardar.Filter = "Documento de texto|*.er";
            guardar.Title = "Guardar";
            var resultado = guardar.ShowDialog();
            if (resultado == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(guardar.FileName);
                if (getRtb() != null)
                {
                    foreach (object line in getRtb().Lines)
                    {
                        sw.WriteLine(line);
                    }
                }
                else
                {
                    MessageBox.Show("Por favor seleccione una pestaña");
                }

                sw.Close();
            }
        }

        private void xuiButton1_Click(object sender, EventArgs e)
        {
    

            PictureBox pichur = new PictureBox();
           // xuiGradientPanel2.Controls.Add(pichur);
            pichur.SizeMode = PictureBoxSizeMode.AutoSize;
            pichur.ImageLocation = "";
            pichur.Image = null;
            pichur.Refresh();
            pichur.Update();
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            contador_afn = 0;
            contador_afd = 0;
            contador = 0;
            automatons.Clear();
            pictureBox1.ImageLocation = "";
            pictureBox1.Image = null;
            pictureBox1.Refresh();
            pictureBox1.Update();
            richTextBox2.Text = "";
            if (getRtb() != null)
            {
                string datosEntrada = getRtb().Text;
                if (datosEntrada != "")
                {
                    Lexer lex = new Lexer();
                    LinkedList<Token> lTokens = lex.scanner(datosEntrada);
                    lex.print_tokens(lTokens);
                    lex.print_tokens(lex.errores);

                    reporte_tokens(lTokens , lex.errores);

                    if (lex.errores.Count() == 0)
                    {

                        Parser p = new Parser();
                        p.parser(lTokens);
                        pa = p;
                        foreach (Expresion exp in p.GetExpresions())
                        {
                            BinaryTreeC tree = new BinaryTreeC();
                            try
                            {
                                tree.tree_constructor3(exp.getValores());
                                AFN afn = new AFN(Node.token_out, exp.getId());
                                afn.thompson2();
                                Node.token_output.Clear();
                                Node.token_out.Clear();
                                pictureBox1.ImageLocation = "";
                                string ruta = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                                if (afn_state == false)
                                {
                                    pictureBox1.ImageLocation = @ruta + "\\afd" + contador + ".png";

                                }
                                else
                                {

                                    pictureBox1.ImageLocation = @ruta + "\\afn" + contador + ".png";

                                }

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error en la expresion " + exp.getId());

                            }

                        }
                        string aux = "";
                        foreach (Lexema lexe in p.GetLexemas())
                        {
                            richTextBox2.Text = "";
                            bool b = analisis_lex(lexe);
                            if (b == true)
                            {
                                aux = aux + "\n" + "Lexema Correcto: " + lexe.getId() + "-> " + lexe.getLex();
                                richTextBox2.Text = aux;
                            }
                            else
                            {
                                aux = aux + "\n" + "Lexema Incorrecto: " + lexe.getId() + "-> " + lexe.getLex();
                                richTextBox2.Text = aux;
                            }

                        }
                        if (automatons.Count() > 0) {
                            tabla_transiciones();
                        }
                    }
                    else {
                        MessageBox.Show("ARCHIVO CONTINE ERRORES, VERIFIQUE EL REPORTE");
                        reporte_tokens(lTokens, lex.errores);
                        reporte_pdf(lex.errores);
                        //reporte_errores(lex.errores);
                    }

                    lTokens.Clear();
                    lex.errores.Clear();
                    Node.token_output.Clear();
                }
                else
                {
                    MessageBox.Show("El RTB no tiene datos");

                }

            }
            else
            {
                MessageBox.Show("Por favor agregue una pestaña");

            }
        }
        public void show_image() {
            string ruta = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            if (afn_state == false)
            {
                pictureBox1.Image = System.Drawing.Image.FromFile(@ruta + "\\afd" + contador + ".png");
            }
            else
            {
                pictureBox1.Image = System.Drawing.Image.FromFile(@ruta + "\\afn" + contador + ".png");            }
            }   
        public bool analisis_lex(Lexema lex) {

            if (automatons.Count() > 0) {

                if (lex.getLex().Count() > 0) {

                    try
                    {
                        Queue<Estado> lex_stack = new Queue<Estado>();
                        Estado aux;
                        foreach (Automaton aut in automatons)
                        {

                            if (aut.getId().Equals(lex.getId()))
                            {

                                aux = aut.getBegin();
                                lex_stack.Enqueue(aux);

                                int i = 0;



                                for (i = 0; i < lex.getLex().Count(); i++)
                                {
                                    Estado actual = lex_stack.Dequeue();
                                    char char_aux = lex.getLex().ElementAt(i);
                                    foreach (Transition t in actual.getTransitions())
                                    {

                                        int n = t.getSimbol().ToString().Count();

                                        if (n > 1)
                                        {

                                            if (t.getSimbol().ToString().Equals("\\n"))
                                            {
                                                char ch = '\n';
                                                if (ch == char_aux)
                                                {
                                                    lex_stack.Enqueue(t.getEnd());
                                                    break;
                                                }

                                            }
                                            else if (t.getSimbol().ToString().Equals("\\t"))
                                            {
                                                char ch = '\t';
                                                if (ch == char_aux)
                                                {
                                                    lex_stack.Enqueue(t.getEnd());
                                                    break;
                                                }

                                            }
                                            else if (t.getSimbol().ToString().Equals("\""))
                                            {
                                                char ch = '\"';
                                                if (ch == char_aux)
                                                {
                                                    lex_stack.Enqueue(t.getEnd());
                                                    break;
                                                }

                                            }
                                            else if (t.getSimbol().ToString().Equals("\\'"))
                                            {
                                                char ch = '\'';
                                                if (ch == char_aux)
                                                {
                                                    lex_stack.Enqueue(t.getEnd());
                                                    break;
                                                }

                                            }

                                            if (isConjunto((string)t.getSimbol()))
                                            {

                                                foreach (Conjunto c in pa.GetConjuntos())
                                                {

                                                    if (t.getSimbol().ToString() == c.getId())
                                                    {

                                                        string s = "";
                                                        s += char_aux;
                                                        if (c.getValores().Contains(s))
                                                        {
                                                            lex_stack.Enqueue(t.getEnd());
                                                            break;
                                                        }
                                                    }

                                                }

                                            }
                                            else
                                            {
                                                int n_tran = t.getSimbol().ToString().Count();
                                                int n_lex = lex.getLex().Count();
                                                if (n_lex > n_tran)
                                                {
                                                    string aux_lex = "";
                                                    int j = 0;
                                                    int aux_i = i;
                                                    for (j = 0; j < n; j++)
                                                    {
                                                        char s = lex.getLex().ElementAt(i);
                                                        aux_lex += s;
                                                        i++;
                                                    }
                                                    if (t.getSimbol().ToString() == aux_lex)
                                                    {
                                                        lex_stack.Enqueue(t.getEnd());
                                                        i = aux_i + (n - 1);
                                                        j = 0;
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        i = aux_i;
                                                        break;
                                                    }
                                                }

                                            }

                                        }
                                        else
                                        {

                                            if (isConjunto((string)t.getSimbol()))
                                            {

                                                foreach (Conjunto c in pa.GetConjuntos())
                                                {

                                                    if (t.getSimbol().ToString() == c.getId())
                                                    {

                                                        string s = "";
                                                        s += char_aux;
                                                        if (c.getValores().Contains(s))
                                                        {
                                                            lex_stack.Enqueue(t.getEnd());
                                                            break;
                                                        }
                                                    }

                                                }

                                            }
                                            else
                                            {
                                                string s = "";
                                                s += char_aux;
                                                if (t.getSimbol().ToString() == s)
                                                {
                                                    lex_stack.Enqueue(t.getEnd());
                                                    break;

                                                }
                                            }

                                        }

                                    }

                                    if (i == lex.getLex().Count() - 1)
                                    {
                                        try
                                        {
                                            Estado final = lex_stack.Dequeue();
                                            if (aut.getEstadosAceptacion().Contains(final))
                                                return true;
                                        }
                                        catch (Exception e)
                                        {
                                            return false;
                                        }

                                    }
                                }





                            }
                        }
                        return false;

                    }
                    catch (Exception e)
                    {
                        return false;
                    }


                }
            }

            return false;
        }

        public void tabla_transiciones() {

            Automaton aut = automatons.ElementAt(contador);

            dataGridView1.Columns.Add("Estados", "Estados");
            foreach (string s in aut.getAlphabet())
            {

                dataGridView1.Columns.Add(s, s);

            }
            foreach (Estado e in aut.getEstados()) {
                dataGridView1.Rows.Add(e.getId(), e.getId());
            }

            for (int i = 0; i < dataGridView1.Rows.Count-1; i++) {

                for (int j = 1; j < dataGridView1.Columns.Count; j++) {

                    Estado e = aut.getEstados().ElementAt(i);
                    foreach (Transition t in e.getTransitions()) {

                        if (t.getSimbol().ToString().Equals(dataGridView1.Columns[j].HeaderText))
                        {

                            dataGridView1.Rows[i].Cells[j].Value = t.getEnd().getId().ToString();
                        }
                    }
                }
            }

        }

        public bool isConjunto(string id) {
            foreach (Conjunto c in pa.GetConjuntos()) {
                if (c.getId().Equals(id))
                    return true;
            }
            return false;
        }

        public void reporte_tokens(LinkedList<Token> tokenList , LinkedList<Token> tokenListe)
        {


            string ruta = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            ruta = ruta + "\\reporte_tokens.xml";
            FileStream fs = new FileStream(ruta, FileMode.Create);
            StreamWriter file = new StreamWriter(fs);

            string sline = "";
            sline = "<ListaTokens>";
            file.WriteLine(sline);
            foreach (Token t in tokenList)
            {
                sline = "<Token>";
                file.WriteLine(sline);
                sline = "<Nombre> " + t.getTipo() + " </Nombre>";
                file.WriteLine(sline);
                sline = "<Valor> " + t.Getval() + " </Valor>";
                file.WriteLine(sline);
                sline = "<Fila> " + t.getFila() + " </Fila>";
                file.WriteLine(sline);
                sline = "<Columna> " + t.getCol() + " </Columna>";
                file.WriteLine(sline);
                sline = "</Token>";
                file.WriteLine(sline);
            }
            sline = "</ListaTokens>";

            sline = "<ListaErrores>";
            file.WriteLine(sline);
            foreach (Token t in tokenListe)
            {
                sline = "<Token>";
                file.WriteLine(sline);
                sline = "<Valor> " + t.Getval() + " </Valor>";
                file.WriteLine(sline);
                sline = "<Fila> " + t.getFila() + " </Fila>";
                file.WriteLine(sline);
                sline = "<Columna> " + t.getCol() + " </Columna>";
                file.WriteLine(sline);
                sline = "</Token>";
                file.WriteLine(sline);
            }
            sline = "</ListaErrores>";

            file.WriteLine(sline);
            file.Close();
            MessageBox.Show("REPORTE GENERADO CORRECTAMENTE");



        }

        public void reporte_errores(LinkedList<Token> tokenList)
        {

            string ruta = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            ruta = ruta + "\\reporte_errores.xml";
            FileStream fs = new FileStream(ruta, FileMode.Create);
            StreamWriter file = new StreamWriter(fs);

            string sline = "";
            sline = "<ListaErrores>";
            file.WriteLine(sline);
            foreach (Token t in tokenList)
            {
                sline = "<Token>";
                file.WriteLine(sline);
                sline = "<Valor> " + t.Getval() + " </Valor>";
                file.WriteLine(sline);
                sline = "<Fila> " + t.getFila() + " </Fila>";
                file.WriteLine(sline);
                sline = "<Columna> " + t.getCol() + " </Columna>";
                file.WriteLine(sline);
                sline = "</Token>";
                file.WriteLine(sline);
            }
            sline = "</ListaErrores>";
            file.WriteLine(sline);
            file.Close();
            MessageBox.Show("ARCHIVO CONTINE ERRORES, VERIFIQUE EL REPORTE");




        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void xuiGradientPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void xuiButton2_Click(object sender, EventArgs e)
        {
            if (contador_afn > 0)
            {
                dataGridView1.Columns.Clear();

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    dataGridView1.Rows[i].Cells.Clear();
                }
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    dataGridView1.Columns.RemoveAt(i);
                }
                dataGridView1.Rows.Clear();
                //pictureBox1.ImageLocation = "";
                if (contador < contador_afn - 1 && contador_afn > 0)
                {

                    contador++;
                    string ruta = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                    if (afn_state == false)
                    {
                        pictureBox1.ImageLocation = @ruta + "\\afd" + contador + ".png";
//                        pictureBox1.Update();
                    }
                    else
                    {
                        pictureBox1.ImageLocation = @ruta + "\\afn" + contador + ".png";
//                        pictureBox1.Update();
                    }
                }

                tabla_transiciones();

            }

        }

        private void xuiButton3_Click(object sender, EventArgs e)
        {

            if (contador_afn > 0) {
                dataGridView1.Columns.Clear();

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    dataGridView1.Rows[i].Cells.Clear();
                }
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    dataGridView1.Columns.RemoveAt(i);
                }
                dataGridView1.Rows.Clear();
                // pictureBox1.ImageLocation = "";
                if (contador > 0)
                {
                    contador--;
                    string ruta = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                    if (afn_state == false)
                    {
                        pictureBox1.ImageLocation = @ruta + "\\afd" + contador + ".png";
//                        pictureBox1.Update();
                    }
                    else
                    {
                        pictureBox1.ImageLocation = @ruta + "\\afn" + contador + ".png";
 //                       pictureBox1.Update();
                    }

                }

                tabla_transiciones();

            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void xuiSwitch1_Click(object sender, EventArgs e)
        {
            string ruta = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            if (contador_afn > 0) {
                pictureBox1.ImageLocation = "";
                if (afn_state == false)
                {
                    afn_state = true;
                    try
                    {
                        pictureBox1.ImageLocation = @ruta + "\\afn" + contador + ".png";
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }

                }
                else
                {
                    afn_state = false;
                    try
                    {
                        pictureBox1.ImageLocation = @ruta + "\\afd" + contador + ".png";
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }

                }
            }

        }

        private void xuiButton4_Click(object sender, EventArgs e)
        {

        }

        private void archivoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }


        public void reporte_pdf(LinkedList<Token> listaToken) {


            string ruta = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            if (listaToken.Count() > 0)
            {
                try
                {

                    Document pdoc = new Document(PageSize.A4, 20f, 20f, 20f, 30f);
                    PdfWriter pdfWriter = PdfWriter.GetInstance(pdoc, new FileStream(ruta + "\\" + "reporte_errores.pdf", FileMode.Create));
                    pdoc.Open();
                    Paragraph p = new Paragraph("REPORTE ERRORES LEXICOS");
                    pdoc.Add(p);

                    foreach (Token t in listaToken)
                    {
                        Paragraph tp = new Paragraph("*********************************\n");
                        pdoc.Add(tp);
                        tp = new Paragraph("TOKEN: " + t.Getval());
                        pdoc.Add(tp);
                        tp = new Paragraph("FILA: " + t.getFila());
                        pdoc.Add(tp);
                        tp = new Paragraph("COLUMNA: " + t.getCol());
                        pdoc.Add(tp);
                    }

                    pdoc.Close();

                }
                catch (Exception)
                {
                    MessageBox.Show("Error en la generacion del reporte ");
                }
            }
            else
            {
                MessageBox.Show("Por favor analize un texto para generar su reporte");
            }





        }
    }
}
