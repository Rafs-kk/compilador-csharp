using System;

namespace complicador
{
    public class Token_Class
    {
        private int line;
        private int column;
        private Token_Types type;
        private String Buffer;
        private String iniLexer;

        public Token_Class()
        {
            this.Buffer = "";
        }

        public virtual Token_Types Type
        {
            get
            {
                return type;
            }
            set
            {
                this.type = value;
            }
        }
        public virtual string Buff
        {
            get
            {
                return Buffer;
            }
            set
            {
                this.Buffer = value;
            }
        }
        public virtual int Lines
        {
            get
            {
                return line;
            }
            set
            {
                this.line = value;
            }
        }
        public virtual int Columns
        {
            get
            {
                return column;
            }
            set
            {
                this.column = value;
            }
        }
        
        public virtual String initialLexer
        {
        	get
        	{
        		return iniLexer;
        	}
        	set
        	{
        		this.iniLexer = value;
        	}
        }
    }
}