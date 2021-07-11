using System;
using System.IO;

namespace complicador
{
    class scannerLexer
    {
        private Token_Class token;
        private StreamReader arq;
        private char c;
        private char tokenNext;
        private int i = 1;
        private int j = 2;
        private int line = 1;
        private int column = 0;
        private static scannerLexer Scanner = null;

        public scannerLexer(string arquivo)
        {
            this.token = new Token_Class();
            this.arq = new StreamReader(arquivo);
            this.tokenNext = (char)arq.Read();
            this.c = ' ';
        }

        public static scannerLexer initializeScanner(string arquivo)
        {
            if (Scanner == null)
            {
                Scanner = new scannerLexer(arquivo);
                return Scanner;
            }
            else
            {
                return Scanner;
            }
        }

        public virtual Token_Class Scan()
        {
            token = new Token_Class();
            token.Buff = "";
            checkTokens();
            column++;
            c = tokenNext;
            tokenNext = (char)arq.Read();
            token.Lines = line;
            token.Columns = column;
            if (token.Type != Token_Types.Espaço_em_Branco && token.Type != Token_Types.Default)
            {
                return token;
            }
            else
            {
                return Scan();
            }
        }
        
        private void detectFloat()
        {
        	token.Buff = token.Buff + c;
        	column++;
            c = tokenNext;
            tokenNext = (char)arq.Read();
            if (char.IsDigit(c))
            {
            	if (char.IsDigit(tokenNext))
            	{
	            	do
	            	{
	            		token.Buff = token.Buff + c;
			        	column++;
			            c = tokenNext;
			            tokenNext = (char)arq.Read();
	            	}
	            	while(char.IsDigit(tokenNext));
            	}
            	token.Buff = token.Buff + c;
            	token.Type = Token_Types.value_Float;
            }
            else
            {
            	token.Type = Token_Types.Err;
            	Console.WriteLine("ERRO na linha " + (line) + ", coluna " + (column) + "\nFloat mal formado\n\n");
            }
        }

        public virtual void checkTokens()
        {
            if (c == ';')
            {
                token.Type = Token_Types.ponto_e_virgula;
            }
            else if (c == '{')
            {
                token.Type = Token_Types.abre_chaves;
            }
            else if (c == '}')
            {
                token.Type = Token_Types.fecha_chaves;
            }
            else if (c == '(')
            {
                token.Type = Token_Types.abre_parentese;
            }
            else if (c == ')')
            {
                token.Type = Token_Types.fecha_parentese;
            }
            else if (c == ',')
            {
                token.Type = Token_Types.virgula;
            }
            else if (c == '+')
            {
                token.Type = Token_Types.sum;
            }
            else if (c == '-')
            {
                token.Type = Token_Types.sub;
            }
            else if (c == '*')
            {
                token.Type = Token_Types.mult;
            }
            else if (c == '=')
            {
                if (tokenNext == '=')
                {
                    column++;
                    c = tokenNext;
                    tokenNext = (char)arq.Read();
                    token.Type = Token_Types.equals_to;
                }
                else
                {
                    token.Type = Token_Types.assignment_operator;
                }
            }
            else if (c == '!')
            {
                if (tokenNext == '=')
                {
                    column++;
                    c = tokenNext;
                    tokenNext = (char)arq.Read();
                    token.Type = Token_Types.differs;
                }
                else
                {
                    token.Type = Token_Types.Err;
                    Console.WriteLine("ERRO na linha " + (line) + ", coluna " + (column) + "\nExclamacao sozinha (nao sucedida por '=')\n\n");
                }
            }
            else if (c == '>')
            {
                if (tokenNext == '=')
                {
                    column++;
                    c = tokenNext;
                    tokenNext = (char)arq.Read();
                    token.Type = Token_Types.greater_equal;
                }
                else
                {
                    token.Type = Token_Types.greater_then;
                }
            }
            else if (c == '<')
            {
                if (tokenNext == '=')
                {
                    column++;
                    c = tokenNext;
                    tokenNext = (char)arq.Read();
                    token.Type = Token_Types.lesser_equal;
                }
                else
                {
                    token.Type = Token_Types.lesser_then;
                }
            }
            else if (c == '.')
            {
                detectFloat();
            }
            else if (c == '\'')
            {
                token.Buff = token.Buff + c;
                column++;
                c = tokenNext;
                tokenNext = (char)arq.Read();
                if (Char.IsLetter(c) || Char.IsDigit(c))
                {
                    token.Buff = token.Buff + c;
                    column++;
                    c = tokenNext;
                    tokenNext = (char)arq.Read();
                    if (c == '\'')
                    {
                    	token.Buff = token.Buff + c;
                        token.Type = Token_Types.value_Char;
                    }
                    else
                    {
                        column++;
                        token.Buff = token.Buff + c;
                        c = tokenNext;
                        tokenNext = (char)arq.Read();
                        if ((int)c == 10 || (int)c == 13)
                        {
                            line++;
                            column = 0;
                        }
                        token.Type = Token_Types.Err;
                        Console.WriteLine("ERRO na linha " + (line) + ", coluna " + (column) + "\nChar mal formado\n\n");
                    }
                }
            }
            else if (Char.IsLetter(c) || c == '_')
            {
                while (Char.IsLetterOrDigit(tokenNext) || tokenNext == '_')
                {
                    token.Buff = token.Buff + c;
                    column++;
                    c = tokenNext;
                    tokenNext = (char)arq.Read();
                }
                token.Buff = token.Buff + c;
                if (string.Equals(token.Buff, "main"))
                {
                    token.Type = Token_Types.Main;
                }
                else if (string.Equals(token.Buff, "int"))
                {
                    token.Type = Token_Types.Int;
                }
                else if (string.Equals(token.Buff, "float"))
                {
                    token.Type = Token_Types.Float;
                }
                else if (string.Equals(token.Buff, "char"))
                {
                    token.Type = Token_Types.Char;
                }
                else if (string.Equals(token.Buff, "if"))
                {
                    token.Type = Token_Types.If;
                }
                else if (string.Equals(token.Buff, "else"))
                {
                    token.Type = Token_Types.Else;
                }
                else if (string.Equals(token.Buff, "do"))
                {
                    token.Type = Token_Types.Do;
                }
                else if (string.Equals(token.Buff, "while"))
                {
                    token.Type = Token_Types.While;
                }
                else
                {
                    token.Type = Token_Types.variable;
                }
            }
            else if (c == '/')
            {
                if (tokenNext != '/' && tokenNext != '*')
                {
                    token.Type = Token_Types.div;
                }
                else if (tokenNext == '/')
                {
                    do
                    {
                        column++;
                        c = tokenNext;
                        tokenNext = (char)arq.Read();
                        if (c == '\uffff')
                        {
                            token.Type = Token_Types.End;
                            break;
                        }
                    }
                    while ((int)c != 10 && (int)c != 13);
                    line++;
                    i++;
	                if (i > j)
	                {
	                	line--;
	                	i--;
	                	j++;
	                }
                    column = 0;
                }
                else if (tokenNext == '*')
                {
                    column++;
                    c = tokenNext;
                    tokenNext = (char)arq.Read();
                    while (c != '\uffff')
                    {
                        if (c == '*' && tokenNext == '/')
                        {
                            column++;
                            c = tokenNext;
                            tokenNext = (char)arq.Read();
                            break;
                        }
                        if ((int)c == 10 || (int)c == 13)
                        {
                            column = 0;
                            line++;
                            c = tokenNext;
                            tokenNext = (char)arq.Read();
                        }
                        column++;
                        c = tokenNext;
                        tokenNext = (char)arq.Read();
                    }
                    if (c == '\uffff')
                    {
                        token.Type = Token_Types.Err;
                        Console.WriteLine("ERRO na linha " + (line) + ", coluna " + (column) + "\nFim de arquivo dentro de comentario\n\n");
                    }
                }
            }
            else if (char.IsDigit(c))
            {
            	if (char.IsDigit(tokenNext))
            	{
	                do
		            {
		                token.Buff = token.Buff + c;
		                column++;
		                c = tokenNext;
		                tokenNext = (char)arq.Read();
		            }
		            while (char.IsDigit(tokenNext));
	            }
	            if (tokenNext == '.')
	            {
	                token.Buff = token.Buff + c;
	                column++;
	                c = tokenNext;
	                tokenNext = (char)arq.Read();
	                detectFloat();
	            }
	            else
	            {
	                token.Buff = token.Buff + c;
	                token.Type = Token_Types.value_Int;
	            }
            }
            else if ((int)c == 10 || (int)c == 13)
            {
                token.Type = Token_Types.Espaço_em_Branco;
                line++;
                i++;
                if (i > j)
                {
                	line--;
                	i--;
                	j++;
                }
                column = 0;
            }
            else if ((int)c == 9 || (int)c == 32)
            {
                token.Type = Token_Types.Espaço_em_Branco;
            }
            else if (c == '\uffff')
            {
                token.Type = Token_Types.End;
            }
            else
            {
                token.Type = Token_Types.Err;
                Console.WriteLine("ERRO na linha " + (line) + ", coluna " + (column) + "\nCaracter invalido\n\n");
            }
        }
    }
}