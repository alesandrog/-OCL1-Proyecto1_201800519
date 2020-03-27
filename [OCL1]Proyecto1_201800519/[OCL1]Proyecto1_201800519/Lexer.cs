using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _OCL1_Proyecto1_201800519
{
    class Lexer
    {


        private LinkedList<Token> output;
        public LinkedList<Token> errores = new LinkedList<Token>();
        private int pointer;
        private string lex;
        static int fila = 0;
        static int columna = 0;
        bool flag = false;
        bool flag_conj = false;



        public LinkedList<Token> scanner(String data) {

            output = new LinkedList<Token>();
            data = data + '`';
            pointer = 0;
            lex = "";
            char c ;

            for (int i = 0; i < data.Length ; i++) {

                c = data.ElementAt(i);

                switch (pointer) {

                    case 0:

                        if (c == '\n') {

                            pointer = 0;
                            columna=0;
                            fila++;

                        }
                        else if (c == '\t')
                        {

                            pointer = 0;

                        }
                        else if (c == ' ')
                        {

                            pointer = 0;
                            columna++;

                        }
                        else if (c == '*')
                        {

                            lex += c;
                            addToken(Token.Tipo.OPERADOR_KLEENE);
                            columna++;
                        }
                        else if (c == '+')
                        {

                            lex += c;
                            addToken(Token.Tipo.OPERADOR_POSITIVA);
                            columna++;
                        }
                        else if (c == '?')
                        {

                            lex += c;
                            addToken(Token.Tipo.OPERADOR_INTE);
                            columna++;
                        }
                        else if (c == '|')
                        {

                            lex += c;
                            addToken(Token.Tipo.OPERADOR_OR);
                            columna++;
                        }
                        else if (c == '\'')
                        {

                            pointer = 13;
                            columna++;
                        }
                        else if (c == '.')
                        {

                            lex += c;
                            addToken(Token.Tipo.OPERADOR_CONCAT);
                            columna++;
                        }
                        else if (c == ':')
                        {

                            lex += c;
                            addToken(Token.Tipo.DOS_PUNTOS);
                            columna++;
                        }
                        else if (c == ',')
                        {

                            lex += c;
                            addToken(Token.Tipo.COMA);
                            columna++;
                        }
                        else if (c == '%')
                        {
                            lex += c;
                            columna++;
                            if (flag == true)
                            {
                                addToken(Token.Tipo.IDENTIFICADOR);
                            }
                            else {
                                pointer = 16;
                            }


                        }
                        else if (c == ';')
                        {

                            lex += c;
                            flag = false;
                            flag_conj = false;
                            addToken(Token.Tipo.PUNTO_COMA);
                            columna++;

                        }
                        else if (c == '/')
                        {

                            lex += c;
                            pointer = 3;
                            columna++;
                        }
                        else if (c == '\\')
                        {

                            lex += c;
                            pointer = 5;
                            columna++;
                        }
                        else if (c == '-')
                        {

                            lex += c;
                            pointer = 6;
                            columna++;
                        }
                        else if (c == '<')
                        {

                            lex += c;
                            pointer = 7;
                            columna++;
                        }
                        else if (c == '~')
                        {

                            lex += c;
                            addToken(Token.Tipo.OPERADOR_VIRGUILLA);
                            columna++;
                        }
                        else if (c == '\"')
                        {

                            pointer = 2;
                            columna++;
                        }
                        else if (c == '\'')
                        {

                            pointer = 2;
                            columna++;
                        }
                        else if (c == '{')
                        {

                            lex += c;
                            pointer = 11;
                            columna++;
                        }
                        else if (c == '}')
                        {

                            lex += c;
                            addToken(Token.Tipo.IDENTIFICADOR);
                            columna++;
                        }
                        else if (c == '[')
                        {

                            pointer = 10;
                            columna++;
                        }
                       
                        else if (c == '(')
                        {

                            pointer = 0;
                            columna++;
                        }
                        else if (c == ')')
                        {

                            pointer = 0;
                            columna++;
                        }
                        else if (Char.IsDigit(c) || Char.IsLetter(c))
                        {

                            lex += c;
                            pointer = 1;
                            columna++;

                        }
                        else

                        {
                            if (c.CompareTo('`') == 0 && i == data.Length - 1)
                            {
                                //Hemos concluido el análisis léxico.
                                Console.WriteLine("Hemos concluido el analiss con exito");
                                lex += c;
                                addToken(Token.Tipo.ULTIMO);


                            }
                            else if ((int)c >= 32 && (int)c <= 125 && flag == true)  {

                                lex += c;
                                columna++;
                                addToken(Token.Tipo.IDENTIFICADOR);

                            }
                            else
                            {
                                // Console.WriteLine("Error lexico con: " + c);
                                //i = i - 1;
                                lex += c;
                                addError(Token.Tipo.ERROR);
                                columna++;
                            }
                        }
                        break;

                    case 1:

                        if (Char.IsDigit(c) || Char.IsLetter(c))
                        {

                            lex += c;
                            pointer = 1;
                            columna++;
                        }
                        else {


                            if (flag_conj = true && c != '-' && c != ':' && c != ',' && c != '~' && c != ';' && c != ' ')
                            {
                                lex += c;
                                pointer = 1;
                                columna++;
                            }
                            else {
                                if (lex.Equals("CONJ"))
                                    flag_conj = true;
                                i = i - 1;
                                addToken(Token.Tipo.IDENTIFICADOR);
                            }




                        }

                        break;

                    case 2:

                        if (c != '"')
                        {

                            lex += c;
                            pointer = 2;
                            columna++;
                        }
                        else
                        {

                            addToken(Token.Tipo.IDENTIFICADOR);

                        }

                        break;

                    case 3:
                        if (c == '/') {
                            lex += c;
                            addToken(Token.Tipo.INICIO_COMENTARIOS);
                            pointer = 4;
                            columna++;
                        }
                        else
                        {
                            addToken(Token.Tipo.IDENTIFICADOR);

                        }
                        break;

                    case 4:

                        if (c != '\n')
                        {
                            lex += c;
                            pointer = 4;
                            columna++;
                        }
                        else {
                            addToken(Token.Tipo.COMENTARIO_SIMPLE);
                        }

                        break;

                    case 5:

                        if (c == 'n' || c == '\'' || c == 't' || c == '\"')
                        {
                            lex += c;
                            columna++;
                            addToken(Token.Tipo.IDENTIFICADOR);
                        }
                        else {
                            i = i - 1;
                            addToken(Token.Tipo.DIAGONAL_INVERTIDA);
                        }

                        break;

                    case 6:
                        if (c == '>')
                        {
                            lex += c;
                            columna++;
                            addToken(Token.Tipo.OPERADOR_ASIGNACION);
                            flag = true;
                        }
                        else {
                            i = i - 1;
                            addToken(Token.Tipo.IDENTIFICADOR);
                        }
                        break;

                    case 7:

                        if (c == '!')
                        {
                            lex += c;
                            columna++;
                            addToken(Token.Tipo.INICIO_COMENTARIOM);
                            pointer = 8;
                        }
                        else {
                            i = i - 1;
                            addToken(Token.Tipo.IDENTIFICADOR);
                        }

                        break;

                    case 8:

                        if (c != '!')
                        {
                            lex += c;
                            columna++;
                            pointer = 8;
                        }
                        else {
                            pointer = 9;
                        }

                        break;

                    case 9:
                        if (c == '>') {
                            addToken(Token.Tipo.COMENTARIO_MULTI);
                            lex += '!';
                            lex += c;
                            columna++;
                            addToken(Token.Tipo.FIN_COMENTARIOM);
                        }

                        break;

                    case 10:

                        if (c == ':')
                        {
                            columna++;
                            pointer = 14;
                        }
                        else {

                            addError(Token.Tipo.ERROR);
                            
                        }

                        break;

                    case 11:
                        if (flag == true && flag_conj == false)
                        {
                            i = i - 1;
                            lex = "";
                            pointer = 12;
                        }
                        else {
                            i = i - 1;
                            addToken(Token.Tipo.IDENTIFICADOR);
                        }
                        break;

                    case 12:
                        if (c != '}')
                        {
                            lex += c;
                            columna++;
                            pointer = 12;
                        }
                        else
                        {
                            addToken(Token.Tipo.IDENTIFICADOR);
                        }
                        break;

                    case 13:

                        if (c != '\'')
                        {

                            lex += c;
                            columna++;
                            pointer = 13;

                        }
                        else
                        {

                            addToken(Token.Tipo.IDENTIFICADOR);

                        }

                        break;

                    case 14:
                        if (c != ':')
                        {

                            lex += c;
                            pointer = 14;

                        }
                        else {
                            //addToken(Token.Tipo.IDENTIFICADOR);
                            pointer = 15;
                        }
                        break;

                    case 15:
                        if (c == ']')
                        {

                            addToken(Token.Tipo.IDENTIFICADOR);
                        }
                        else
                        {
                            lex += c;
                            pointer = 14;
                        }
                        break;

                    case 16:
                        if (c == '%')
                        {
                            lex += c;
                            addToken(Token.Tipo.OPERADOR_PORCENTAJE);

                        }
                        else {
                            i = i - 1;
                            addError(Token.Tipo.ERROR);
                        }
                        break;
                }


            }


            //   reporte_tokens(output);
            columna = 0;
            fila = 0;
            return output;
        }

        public void addToken(Token.Tipo tipo)
        {
            Token t = new Token(tipo, lex);
            t.setFila(fila);
            t.setCol(columna);
            output.AddLast(t);
            lex = "";
            pointer = 0;
        }

        public void addError(Token.Tipo tipo)
        {
            Token t = new Token(tipo, lex);
            t.setFila(fila);
            t.setCol(columna);
            errores.AddLast(t);
            lex = "";
            pointer = 0;
        }


        public void print_tokens(LinkedList<Token> tokenList) {

            foreach (Token t in tokenList) {

                Console.WriteLine(t.Getval() + " -> " + t.getTipo());

            }

        }




    }
}
