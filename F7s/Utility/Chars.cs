using System;

namespace F7s.Utility {

    public class CharWidthUncalculableException : Exception {

        public CharWidthUncalculableException(string message)
            : base(message: message) { }

    }

    public static class Chars {

        private static bool fontEngineInitialized;

        public static void FontEngineHasBeenInitialized() {
            if (fontEngineInitialized) {
                //	throw new RedundantOperationException("Font engine has already been initialized.");
            }

            fontEngineInitialized = true;
        }


        public class ImplicitChar {

            private readonly char character;

            public ImplicitChar(char c) {
                this.character = c;
            }

            public static implicit operator char(ImplicitChar character) {
                return character.character;
            }

            public static implicit operator string(ImplicitChar character) {
                return character.character.ToString();
            }

            public override string ToString() {
                return this.character.ToString();
            }

        }

        #region preset chars


        public static ImplicitChar at => new ImplicitChar(c: '\u0040');

        public static ImplicitChar average => new ImplicitChar(c: '\u2205');

        public static ImplicitChar backslash => new ImplicitChar(c: '\\');

        public static string LineBreaks(int lineBreaks) {

            string s = "";

            for (int i = 0; i < lineBreaks; i++) {
                s += lineBreak.ToString();
            }

            return s;
        }

        public static ImplicitChar lineBreak => new ImplicitChar(c: '\n');

        public static ImplicitChar capitalDelta => new ImplicitChar(c: '\u0394');

        public static ImplicitChar capitalPi => new ImplicitChar(c: '\u03a0');

        public static ImplicitChar capitalEpsilon => new ImplicitChar(c: '\u0395');

        public static ImplicitChar capitalIota => new ImplicitChar(c: '\u0399');

        public static ImplicitChar circumflex => new ImplicitChar(c: '\u005E');

        public static ImplicitChar cubic => new ImplicitChar(c: '\u00B3');

        public static ImplicitChar degree => new ImplicitChar(c: '\u00B0');

        public static ImplicitChar mu => new ImplicitChar(c: '\u00B5');

        public static ImplicitChar multiplication => new ImplicitChar(c: '\u00D7');

        public static ImplicitChar newline => new ImplicitChar(c: '\n');

        public static ImplicitChar not => new ImplicitChar(c: '\u00AC');

        public static ImplicitChar tab => new ImplicitChar(c: '\u0009');

        public static ImplicitChar tilde => new ImplicitChar(c: '\u007E');

        public static ImplicitChar plusminus => new ImplicitChar(c: '\u00B1');

        public static ImplicitChar scharfS => new ImplicitChar(c: '\u00DF');

        public static ImplicitChar section => new ImplicitChar(c: '\u00A7');

        public static ImplicitChar squared => new ImplicitChar(c: '\u00B2');

        public static ImplicitChar yen => new ImplicitChar(c: '\u00A5');

        public static string Tabs(int tabs) {
            string s = "";

            for (int i = 0; i < tabs; i++) {
                s += tab.ToString();
            }

            return s;
        }

        #endregion

    }

}