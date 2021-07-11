using System.Collections.Generic;

namespace complicador
{
	
	public class tabelaSimbolos {
		private List<Token_Class> lista_tokens;
		
		public tabelaSimbolos(){
			this.lista_tokens = new List<Token_Class>();
		}
		
		public virtual void addToken(Token_Class token)
		{
			lista_tokens.Add(token);
		}
		
		public virtual Token_Types TypeSearch(Token_Class token)
		{
			int i;
			for(i = lista_tokens.Count - 1; i >= 0; i--)
			{
				if(lista_tokens[i].Buff.Equals(token.Buff))
				{
					return lista_tokens[i].Type;
				}
			}
			return Token_Types.Default;
		}
		
		public virtual Token_Types ScopeSearch(Token_Class token)
		{
			int i;
			for(i = lista_tokens.Count - 1; i >= 0; i--)
			{
				if(lista_tokens[i].Type == Token_Types.abre_chaves)
				{
					break;
				}
				else if(lista_tokens[i].Buff.Equals(token.Buff))
				{
					return lista_tokens[i].Type;
				}
			}
			return Token_Types.Default;
		}
	}
}
