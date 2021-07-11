
namespace complicador
{
    public enum Token_Types
    {
        Default, // id default
        Espaço_em_Branco, //espaço / tab / quebra de linha
        Int, Float, Char, String, //tipos de variavel
        If, Else, //condicionais
        While, Do, //loops
        sum, sub, div, mult, assignment_operator, greater_then, lesser_then, equals_to, greater_equal, lesser_equal, differs, //operandos 
        value_Int, value_Float, value_Char, typed_String, variable, //valores das variaveis
        ponto_e_virgula, virgula, abre_chaves, fecha_chaves, abre_parentese, fecha_parentese, //caracteres necessariosc ou especiais
        Main, //main
        End, Err //fim de arquivo / erro
    }
}