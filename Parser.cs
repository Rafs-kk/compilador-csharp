﻿using System;

namespace complicador
{
    class Parser
    {
        private Token_Class token;
        private Token_Class tokenNext;
        private static Token_Class conversao = null;
        private static Token_Types type1;
    	private static Token_Types type2;
        private static scannerLexer Scanner;
        private tabelaSimbolos symbolTable;
        private int contLabel = 0;
        private int contT = 0;
        private bool flag = false;
        private bool flagIteracao = false;
        private bool flagCondicional = false;
        private bool flagVazio = false;   
        private bool flagAtribuicao = false; 
        private string label;
        private string auxLabel;
        private string temp;
        private string aux;
        

        public Parser(string arquivo)
        {
            Scanner = scannerLexer.initializeScanner(arquivo);
            tokenNext = Scanner.Scan();
            symbolTable = new tabelaSimbolos();
        }

        private void Next()
        {
            token = tokenNext;
            tokenNext = Scanner.Scan();
        }

        public bool firstDeclVar()
        {
            if (token.Type == Token_Types.Int || token.Type == Token_Types.Float || token.Type == Token_Types.Char)
            {
                return true;
            }
            return false;
        }

        public bool tipoNext()
        {
            if (tokenNext.Type == Token_Types.Int || tokenNext.Type == Token_Types.Float || tokenNext.Type == Token_Types.Char)
            {
                return true;
            }
            return false;
        }

        public bool nextComando()
        {
            if (tokenNext.Type == Token_Types.Do || tokenNext.Type == Token_Types.While || tokenNext.Type == Token_Types.If)
            {
                return true;
            }
            return false;
        }

        public bool firstFator()
        {
            if (token.Type == Token_Types.Int || token.Type == Token_Types.Float || token.Type == Token_Types.Char || token.Type == Token_Types.variable || token.Type == Token_Types.value_Int || token.Type == Token_Types.value_Float || token.Type == Token_Types.value_Char)
            {
                return true;
            }
            return false;
        }
        
        public bool firstValue()
        {
            if (token.Type == Token_Types.value_Int || token.Type == Token_Types.value_Float || token.Type == Token_Types.value_Char)
            {
                return true;
            }
            return false;
        }

        public bool firstComandoBasico()
        {
            if (token.Type == Token_Types.variable || token.Type == Token_Types.abre_chaves)
            {
                return true;
            }
            return false;
        }

        public bool firstComando()
        {
            if (token.Type == Token_Types.Do || token.Type == Token_Types.While || token.Type == Token_Types.abre_chaves || token.Type == Token_Types.If || token.Type == Token_Types.variable)
            {
                return true;
            }
            return false;
        }

        public bool firstIteracao()
        {
            if (token.Type == Token_Types.Do || token.Type == Token_Types.While)
            {
                return true;
            }
            return false;
        }

        public bool firstTermo()
        {
            if (token.Type == Token_Types.value_Int || token.Type == Token_Types.value_Float || token.Type == Token_Types.value_Char || token.Type == Token_Types.abre_parentese || token.Type == Token_Types.variable)
            {
                return true;
            }
            return false;
        }

        public bool firstCompara()
        {
            if (token.Type == Token_Types.equals_to || token.Type == Token_Types.greater_equal || token.Type == Token_Types.greater_then || token.Type == Token_Types.lesser_equal || token.Type == Token_Types.lesser_then || token.Type == Token_Types.differs)
            {
                return true;
            }
            return false;
        }
        
        public void CodTermo (Token_Class T, Token_Class T2, char op)
        {
        	temp = "T" + contT;
        	contT++;
        	Console.WriteLine(temp + " = " + T.Buff + op + T2.Buff);
        	T.Buff = temp;
        }
        
        public void programa()
        {
            Next();
            if (token.Type == Token_Types.Int)
            {
                Next();
                if (token.Type == Token_Types.Main)
                {
                    Next();
                    if (token.Type == Token_Types.abre_parentese)
                    {
                        Next();
                        if (token.Type == Token_Types.fecha_parentese)
                        {
                            Next();
                            if (bloco())
                            {
                            	Next();
                                if (token.Type != Token_Types.End)
                                {
                                    Console.WriteLine("ERRO na linha " + (token.Lines) + ", coluna " + (token.Columns) + " : Fim de arquivo não encontrado\n\n");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("ERRO na linha " + (token.Lines) + ", coluna " + (token.Columns) + " : Erro ao inicializar o Main ')' não encontrado\n\n");
                        }
                    }
                    else
                    {
                        Console.WriteLine("ERRO na linha " + (token.Lines) + ", coluna " + (token.Columns) + " : Erro ao inicializar o Main '(' não encontrado\n\n");
                    }
                }
                else
                {
                    Console.WriteLine("ERRO na linha " + (token.Lines) + ", coluna " + (token.Columns) + " : Erro ao inicializar o Main ('main' não encontrado)\n\n");
                }
            }
            else
            {
                Console.WriteLine("ERRO na linha " + (token.Lines) + ", coluna " + (token.Columns) + " : Erro ao inicializar o Main ('int' não encontrado)\n\n");
            }
        }

        public bool bloco()
        {
            if (token.Type != Token_Types.abre_chaves)
            {
                Console.WriteLine("ERRO na linha " + (token.Lines) + ", coluna " + (token.Columns) + " : Erro ao abrir o bloco ('{' não encontrado)\n\n");
                return false;
            }
            else
            {
            	symbolTable.addToken(token);
                Next();
                if (token.Type == Token_Types.fecha_chaves)
                {
                    Console.WriteLine("ERRO na linha " + (token.Lines) + ", coluna " + (token.Columns) + " : Bloco inválido (faltando comandos)\n\n");
                }
                else
                {
                    while (firstDeclVar())
                    {
                        if (declaraVar())
                        {
                            Next();
                            if (!firstDeclVar())
                            {
                                break;
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    while (firstComando())
                    {
                        if (comando())
                        {
                        	Next();
                            if (!firstComando())
                            {
                                break;
                            }
                            else
                            {
                            	continue;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                if (token.Type != Token_Types.fecha_chaves)
                {
                    Console.WriteLine("ERRO na linha " + (token.Lines) + ", coluna " + (token.Columns) + " : Erro ao fechar o bloco ('}' não encontrado)\n\n");
                    return false;
                }
            }
            return true;
        }

        public bool declaraVar()
        {
        	Token_Types type;
            if (!firstDeclVar())
            {
                Console.WriteLine("ERRO na linha " + (token.Lines) + ", coluna " + (token.Columns) + " : Declaração invalida (token encontrado não é variável)\n\n");
            }
            else
            {
            	type = token.Type;
                Next();
                if (token.Type == Token_Types.variable)
                {
                	if(symbolTable.ScopeSearch(token) != Token_Types.Default)
                	{
                		Console.WriteLine("ERRO na linha " + (token.Lines) + ", coluna " + (token.Columns) + " : Declaração invalida (variável a ser declarada já está presente no escopo)\n\n");
                	}
                	token.Type = type;
                	symbolTable.addToken(token);
	                if (tokenNext.Type == Token_Types.virgula)
	                {
	                    Next();
	                    do
	                    {
	                        Next();
	                        if(symbolTable.ScopeSearch(token) != Token_Types.Default)
			                {
			                	Console.WriteLine("ERRO na linha " + (token.Lines) + ", coluna " + (token.Columns) + " : Declaração invalida (variável a ser declarada já está presente no escopo)\n\n");
			                }
			                else
			                {
				               	token.Type = type;
	                			symbolTable.addToken(token);
                			}
	                        Next();
	                    }
	                    while (token.Type == Token_Types.virgula);
	                    if (token.Type != Token_Types.ponto_e_virgula)
	                    {
	                        Console.WriteLine("ERRO na linha " + (token.Lines) + ", coluna " + (token.Columns) + " : Ponto e virgula não encontrado no fim da declaração de variavel\n\n");
	                    }
	                    else
	                    {
	                        return true;
	                    }
	                }
	                else
	                {
	                    if (tokenNext.Type == Token_Types.ponto_e_virgula)
	                    {
	                        Next();
	                    }
	                    else
	                    {
	                    	Console.WriteLine("ERRO na linha " + (token.Lines) + ", coluna " + (token.Columns) + " : Ponto e virgula não encontrado\n\n");
	                    }
	                }
                }
            }
            return true;
        }

        public bool comando()
        {
            if (firstComandoBasico())
            {
                if (atribuicao())
                {
                    return true;
                }
                else if (bloco())
                {
                    return true;
                }
                else if (token.Type == Token_Types.fecha_chaves)
                {
                    return false;
                }
                else
                {
                    Console.WriteLine("ERRO na linha " + (token.Lines) + ", coluna " + (token.Columns) + " : Comando básico inválido (token encontrado não é 'variavel' nem '{')\n\n");
                }
            }
            else if (firstIteracao())
            {
                if (iteracao())
                {
                    if (flagVazio == true)
                    {
                        Console.WriteLine("ERRO na linha " + (token.Lines) + ", coluna " + (token.Columns) + " : Operação inválida (faltando comandos dentro dos parenteses)\n\n");
                    }
                    return true;
                }
                else if (flagIteracao == false)
                {
                    Console.WriteLine("ERRO na linha " + (token.Lines) + ", coluna " + (token.Columns) + " : Operação inválida (token diferente de '==', '>=', '<=', '>', '<')\n\n");
                    return true;
                }
                else
                {
                    Console.WriteLine("ERRO na linha " + (token.Lines) + ", coluna " + (token.Columns) + " : Iteração inválida (token não é 'Do' nem 'While')\n\n");
                }
            }
            else
            {
                if (condicional())
                {
                    if (flagVazio == true)
                    {
                        Console.WriteLine("ERRO na linha " + (token.Lines) + ", coluna " + (token.Columns) + " : Operação inválida (faltando comandos dentro dos parenteses)\n\n");
                    }
                    return true;
                }
                else if (flagCondicional == false)
                {
                    Console.WriteLine("ERRO na linha " + (token.Lines) + ", coluna " + (token.Columns) + " : Operação inválida (token diferente de '==', '>=', '<=', '>', '<')\n\n");
                    return true;
                }
                else
                {
                    Console.WriteLine("ERRO na linha " + (token.Lines) + ", coluna " + (token.Columns) + " : Condicional mal formada\n\n");
                }

            }
            return false;
        }

        public bool condicional()
        {
        	string LabelAux;
            if (token.Type == Token_Types.If)
            {
                Next();
                if (token.Type == Token_Types.abre_parentese)
                {
                    Next();
                    if (token.Type == Token_Types.fecha_parentese)
                    {
                        flagVazio = true;
                        return flagVazio;
                    }
                    if (expr_relacional())
                    {
                    	label = "L" + contLabel;
                    	contLabel++;
                    	Console.WriteLine("if " + aux + " == " + "false goto " + label);
                        if (token.Type == Token_Types.fecha_parentese)
                        {
                            Next();
                            if (!firstComando())
                            {
                                Console.WriteLine("ERRO na linha " + (token.Lines) + ", coluna " + (token.Columns) + " : Condicional inválida (faltando comandos | comando inválido para a condicional)\n\n");
                            }
                            LabelAux = label;
                            if (comando())
                            {
                                flagCondicional = true;
                                if (tokenNext.Type == Token_Types.Else)
                                {
                                    Next();
                                }
                                else
                                {
                                	Console.WriteLine(LabelAux + ": ");
                                }
                                if  (token.Type == Token_Types.Else)
                                {
                                	Next();
                                	auxLabel = label;
                                	label = "L" + contLabel;
                    				contLabel++;
                                	Console.WriteLine("goto " + label);
                                	Console.WriteLine(LabelAux + ": ");
                                	if (!firstComando())
                                	{
                                		Console.WriteLine("ERRO na linha " + (token.Lines) + ", coluna " + (token.Columns) + " : Condicional inválida (faltando comandos | comando inválido para a condicional)\n\n");
                                	}
                                	if (comando())
                                	{
                                		Console.WriteLine(label + ": ");
                                		return true;
                                	}
                                }
                            }
                            else
                            {
                                flagCondicional = true;
                            }
                        }
                    }
                    else
                    {
                        Next();
                        flagCondicional = false;
                        Next();
                    }
                }
            }
            return flagCondicional;
        }

        public bool iteracao()
        {
        	string LabelAux, LabelAux2, LabelAux3;
            if (token.Type == Token_Types.While)
            {
            	label = "L" + contLabel;
                contLabel++;
            	Console.WriteLine(label + ": ");
                Next();
                if (token.Type == Token_Types.abre_parentese)
                {
                    Next();
                    if (token.Type == Token_Types.fecha_parentese)
                    {
                        flagVazio = true;
                        return flagVazio;
                    }
                    else if (expr_relacional())
                    {
                    	auxLabel = label;
                    	label = "L" + contLabel;
                    	contLabel++;
                        flagVazio = false;
                        Console.WriteLine("if " + aux + " == " + "false goto " + label);
                        if (token.Type == Token_Types.fecha_parentese)
                        {
                            Next();
                            if (!firstComando())
                            {
                                Console.WriteLine("ERRO na linha " + (token.Lines) + ", coluna " + (token.Columns) + " : Iteração inválida (faltando comandos | comando inválido para a iteração)\n\n");
                            }
                            LabelAux = label;
                            LabelAux2 = auxLabel;
                            if (comando())
                            {
                            	Console.WriteLine("goto " + LabelAux2);
                            	Console.WriteLine(LabelAux + ": ");
                                return true;
                            }
                        }
                    }
                    else
                    {
                        Next();
                        flagIteracao = false;
                        Next();
                    }
                }
                return flagIteracao;
            }
            else if (token.Type == Token_Types.Do)
            {
            	label = "L" + contLabel;
                contLabel++;
            	Console.WriteLine(label + ": ");
            	LabelAux3 = label;
                Next();
                if (firstComando())
                {
                    if (comando())
                    {
                        Next();
                        if (token.Type == Token_Types.While)
                        {
                            Next();
                            if (token.Type == Token_Types.abre_parentese)
                            {
                                Next();
                                if (token.Type == Token_Types.fecha_parentese)
                                {
                                    flagVazio = true;
                                    Next();
                                    if (token.Type == Token_Types.ponto_e_virgula)
                                    {
                                        return flagVazio;
                                    }
                                    else
                                    {
                                        Console.WriteLine("ERRO na linha " + (token.Lines) + ", coluna " + (token.Columns) + " : Operação inválida (';' não encontrado)\n\n");
                                    }
                                }
                                if (expr_relacional())
                                {
                                    flagVazio = false;
                                    Console.WriteLine("if " + aux + " != " + "false goto " + LabelAux3);
                                    if (token.Type == Token_Types.fecha_parentese)
                                    {
                                        Next();
                                        if (token.Type == Token_Types.ponto_e_virgula)
                                        {
                                            return true;
                                        }
                                        else
                                        {
                                            Console.WriteLine("ERRO na linha " + (token.Lines) + ", coluna " + (token.Columns) + " : Operação inválida (';' não encontrado)\n\n");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("ERRO na linha " + (token.Lines) + ", coluna " + (token.Columns) + " : Operação inválida (')' não encontrado)\n\n");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("ERRO na linha " + (token.Lines) + ", coluna " + (token.Columns) + " : Operação inválida (expressão relacional inválida)\n\n");
                                }
                            }
                            else
                            {
                                Console.WriteLine("ERRO na linha " + (token.Lines) + ", coluna " + (token.Columns) + " : Operação inválida ('(' não encontrado)\n\n");
                            }
                        }
                        else
                        {
                            Console.WriteLine("ERRO na linha " + (token.Lines) + ", coluna " + (token.Columns) + " : Operação inválida ('while' não encontrado)\n\n");
                        }
                    }
                    else
                    {
                        Console.WriteLine("ERRO na linha " + (token.Lines) + ", coluna " + (token.Columns) + " : Operação inválida (comando inválido)\n\n");
                    }
                }
                else
                {
                    Console.WriteLine("ERRO na linha " + (token.Lines) + ", coluna " + (token.Columns) + " : Operação inválida (comando first inválido)\n\n");
                }
            }
            return false;
        }

        public bool atribuicao()
        {
        	flagAtribuicao = true;
        	Token_Class atri;
        	Token_Class auxi;
        	Token_Types type;
            if (token.Type == Token_Types.variable)
            {
            	if(symbolTable.TypeSearch(token) == Token_Types.Default)
            	{
            		Console.WriteLine("ERRO na linha " + (token.Lines) + ", coluna " + (token.Columns) + " : Atribuição inválida (Variável a ser usada ainda não foi declarada)\n\n");
            	}
            	type = symbolTable.TypeSearch(token);
            	type1 = type;
            	atri = token;
                Next();
                if (token.Type == Token_Types.assignment_operator)
                {
                	Next();
                	auxi = expr_aritm();
                	flagAtribuicao = false;
                	Console.WriteLine(atri.Buff + " = " + auxi.Buff);
                    if (token.Type == Token_Types.ponto_e_virgula)
                    {
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("ERRO na linha " + (token.Lines) + ", coluna " + (token.Columns) + " : Atribuição inválida (';' não encontrado)\n\n");
                    }
                }
                else
                {
                    Console.WriteLine("ERRO na linha " + (token.Lines) + ", coluna " + (token.Columns) + " : Atribuição inválida (token encontrado não é '=')\n\n");
                    auxi = expr_aritm();
                	if (auxi.Type == Token_Types.variable)
                	{
                		type = symbolTable.TypeSearch(auxi);
		            	if(type == Token_Types.Default)
		            	{
		            		Console.WriteLine("ERRO na linha " + (token.Lines) + ", coluna " + (token.Columns) + " : Atribuição inválida (Variável a ser usada ainda não foi declarada)\n\n");
		            	}
                	}
                	flagAtribuicao = false;
                    if (token.Type == Token_Types.ponto_e_virgula)
                    {
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("ERRO na linha " + (token.Lines) + ", coluna " + (token.Columns) + " : Atribuição inválida (';' não encontrado)\n\n");
                    }
                }
            }
            return false;
        }

        public bool expr_relacional()
        {
        	Token_Types type;
        	Token_Class aux1 = null;
        	Token_Class aux2 = null;
        	string op = " ";
        	if (token.Type == Token_Types.variable)
        	{
        		if(symbolTable.TypeSearch(token) == Token_Types.Default)
                {
               		Console.WriteLine("ERRO na linha " + (token.Lines) + ", coluna " + (token.Columns) + " : Variável a ser usada ainda não foi declarada\n\n");
               	}
               	type = symbolTable.TypeSearch(token);
               	type1 = type;
        	}
        	else
        	{
        		type = symbolTable.TypeSearch(token);
               	type1 = type;
        	}
        	aux1 = expr_aritm();
            if (firstCompara())
            {
            	if (token.Type == Token_Types.greater_then)
            	{
            		op = ">";
            	}
            	else if (token.Type == Token_Types.lesser_then)
            	{
            		op = "<";
            	}
            	else if (token.Type == Token_Types.equals_to)
            	{
            		op = "==";
            	}
            	else if (token.Type == Token_Types.greater_equal)
            	{
            		op = ">=";
            	}
            	else if (token.Type == Token_Types.lesser_equal)
            	{
            		op = "<=";
            	}
            	else if (token.Type == Token_Types.differs)
            	{
            		op = "!=";
            	}
                Next();
                aux2 = expr_aritm();
                temp = "T" + contT;
        		contT++;
                aux = temp;
                flag = true;
                Console.WriteLine(temp + " = " + aux1.Buff + op + aux2.Buff);
            }
        	return flag;
        }

        public Token_Class expr_aritm()
        {
        	Token_Class aux1 = null;
        	Token_Class aux2 = null;
        	char op;
            if (firstTermo())
            {
            	aux1 = termo();
                if (token.Type == Token_Types.sum || token.Type == Token_Types.sub)
                {
	                do
	                {
	                	if(token.Type == Token_Types.sum)
	                	{
	                		op = '+';
	                	}
	                	else
	                	{
	                		op = '-';
	                	}
	                    Next();
	                    aux2 = termo();
	                    if(conv(aux1,aux2,op))
	                    {
	                    	continue;
	                    }
	                    temp = "T" + contT;
        				contT++;
	                    Console.WriteLine(temp + " = " + aux1.Buff + op + aux2.Buff);
	                    aux1.Buff = temp;
	                }
	                while (token.Type == Token_Types.sum || token.Type == Token_Types.sub);
	            }
            }
            else
            {
            	Console.WriteLine("ERRO na linha " + (token.Lines) + ", coluna " + (token.Columns) + " : Expressão aritmética inválida\n\n");
            }
            return aux1;
        }

        public Token_Class termo()
        {
        	Token_Class aux1 = null;
        	Token_Class aux2 = null;
        	char op;
            if (firstTermo())
            {
            	aux1 = fator();
            	Next();
                if (token.Type == Token_Types.mult || token.Type == Token_Types.div)
                {
                    do
                    {
                    	if (token.Type == Token_Types.div && type1 == Token_Types.Int)
                    	{
                    		Console.WriteLine("ERRO na linha " + (token.Lines) + ", coluna " + (token.Columns) + " : Tipos incompatíveis (Resultado será dado em Float)\n\n");
                    	}
                    	if(token.Type == Token_Types.mult)
	                	{
	                		op = '*';
	                	}
	                	else
	                	{
	                		op = '/';
	                	}
                        Next();
                        aux2 = fator();
                        Next();
                        if(conv(aux1,aux2,op))
                        {
                        	continue;
                        }
                        CodTermo(aux1,aux2,op);
                    }
                    while (token.Type == Token_Types.mult || token.Type == Token_Types.div);
                }
            }
            else
            {
            	Console.WriteLine("ERRO na linha " + (token.Lines) + ", coluna " + (token.Columns) + " : Termo inválido\n\n");
            }
            return aux1;
        }

        public Token_Class fator()
        {
        	Token_Types type;
        	Token_Class tokenAux;
            if (firstFator())
            {
            	if(token.Type == Token_Types.variable)
            	{
            		if(symbolTable.TypeSearch(token) == Token_Types.Default)
                	{
                		Console.WriteLine("ERRO na linha " + (token.Lines) + ", coluna " + (token.Columns) + " : Variável a ser utilizada ainda não foi declarada\n\n");
                	}
                	type = symbolTable.TypeSearch(token);
	            	type2 = type;
	            	if ((type1 == Token_Types.Char && (type2 != Token_Types.Char && type2 != Token_Types.value_Char)))
	            	{
	            		Console.WriteLine("ERRO na linha " + (token.Lines) + ", coluna " + (token.Columns) + " : Tipos incompatíveis (Char sendo atribuido a Int/Float)\n\n");
	            	}
	            	else if (type1 != Token_Types.Char && (type2 == Token_Types.Char || type2 == Token_Types.value_Char))
	            	{
	            		if (type1 == Token_Types.Int)
	            		{
	            			Console.WriteLine("ERRO na linha " + (token.Lines) + ", coluna " + (token.Columns) + " : Tipos incompatíveis (Int sendo atribuido a Char)\n\n");
	            		}
	            		else if (type1 == Token_Types.Float)
	            		{
	            			Console.WriteLine("ERRO na linha " + (token.Lines) + ", coluna " + (token.Columns) + " : Tipos incompatíveis (Float sendo atribuido a Char)\n\n");
	            		}
	            	}
	            	if (flagAtribuicao)
	            	{
	            		if (type1 == Token_Types.Int && type2 == Token_Types.Float)
	            		{
	            			Console.WriteLine("ERRO na linha " + (token.Lines) + ", coluna " + (token.Columns) + " : Float sendo atribuido a Int\n\n");
	            		}
	            	}
	                return token;
	            }
	            else if (firstValue())
	            {
	            	type = token.Type;
	            	type2 = type;
	            	if (type1 == Token_Types.Char && (type2 != Token_Types.Char && type2 != Token_Types.value_Char))
	            	{
	            		Console.WriteLine("ERRO na linha " + (token.Lines) + ", coluna " + (token.Columns) + " : Tipos incompatíveis (Operação com Char e Int/Float)\n\n");
	            	}
	            	else if (type1 != Token_Types.Char && (type2 == Token_Types.Char || type2 == Token_Types.value_Char))
	            	{
	            		if (type1 == Token_Types.Int)
	            		{
	            			Console.WriteLine("ERRO na linha " + (token.Lines) + ", coluna " + (token.Columns) + " : Tipos incompatíveis (Operação com Int e Char)\n\n");
	            		}
	            		else if (type1 == Token_Types.Float)
	            		{
	            			Console.WriteLine("ERRO na linha " + (token.Lines) + ", coluna " + (token.Columns) + " : Tipos incompatíveis (Operação com Float e Char)\n\n");
	            		}
	            	}
	            	if (flagAtribuicao)
	            	{
	            		if (type1 == Token_Types.Int && (type2 == Token_Types.Float || type2 == Token_Types.value_Float))
	            		{
	            			Console.WriteLine("ERRO na linha " + (token.Lines) + ", coluna " + (token.Columns) + " : Tipos incompatíveis (Operação com Float e Int)\n\n");
	            		}
	            	}
	                return token;
	            }
            }
            else if (token.Type == Token_Types.abre_parentese)
            {
                Next();
                tokenAux = expr_aritm();
                if (token.Type == Token_Types.fecha_parentese)
                {
                    return tokenAux;
                }
                else
                {
                    Console.WriteLine("ERRO na linha " + (token.Lines) + ", coluna " + (token.Columns) + " : Expressão aritmetica inválida (')' não encontrado no final da expressão)\n\n");
                }
            }
            else
            {
                Console.WriteLine("ERRO na linha " + (token.Lines) + ", coluna " + (token.Columns) + " : Fator inválido\n\n");
            }
            return token;
        }
        
        public bool conv (Token_Class T, Token_Class T2, char op)
        {
        	Token_Types type;
        	type = symbolTable.TypeSearch(T);
        	if(type1 == Token_Types.Float)
        	{
        		if(op == '/')
        		{
	        		if(T.Type == Token_Types.value_Int || type == Token_Types.Int)
	        		{
	        			temp = "T" + contT;
       				 	contT++;
	        			Console.WriteLine(temp + " = to_float " + T.Buff);
	        			T.Type = Token_Types.value_Float;
	        			T.Buff = temp;
	        			CodTermo(T, T2, op);
	        			return true;
	        		}
	        		if(T2.Type == Token_Types.value_Int || type2 == Token_Types.Int)
	        		{
	        			temp = "T" + contT;
        				contT++;
	        			Console.WriteLine(temp + " = to_float " + T2.Buff);
	        			T2.Type = Token_Types.value_Float;
	        			T2.Buff = temp;
	        			CodTermo(T, T2, op);
	        			return true;
	        		}
        		}
        		if ((T.Type != Token_Types.value_Int && type != Token_Types.Int) && (T2.Type == Token_Types.value_Int || type2 == Token_Types.Int))
        		{
        			temp = "T" + contT;
        			contT++;
        			Console.WriteLine(temp + " = to_float " + T2.Buff);
        			T2.Buff = temp;
        			CodTermo(T, T2, op);
        			conversao = null;
        			return true;
        		}
        		if ((T.Type == Token_Types.value_Int || type == Token_Types.Int) && (T2.Type != Token_Types.value_Int && type2 != Token_Types.Int))
        		{
        			temp = "T" + contT;
        			contT++;
        			Console.WriteLine(temp + " = to_float " + T.Buff);
        			T.Buff = temp;
        			CodTermo(T, T2, op);
        			conversao = null;
        			return true;
        		}
        		if(conversao != null)
        		{
        			if (T.Buff.Equals(conversao.Buff))
        			{
        				T = conversao;
        			}
        			else if (T2.Buff.Equals(conversao.Buff))
        			{
        				T2 = conversao;
        			}
        		}
        		else
        		{
        			if((T.Type == Token_Types.value_Int || type == Token_Types.Int) && (T2.Type == Token_Types.value_Int || type2 == Token_Types.Int))
	        		{
	        			CodTermo(T, T2, op);
				        temp = "T" + contT;
        				contT++;
				        Console.WriteLine(temp + " = to_float " + T.Buff);
				        T.Buff = temp;
				        conversao = T;
				        conversao.Type = Token_Types.value_Float;
				        return true;
        			}
        		}
        	}
        	return false;
        }
    }
}