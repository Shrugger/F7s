using System;
using System.Collections.Generic;
using System.Linq;

namespace F7s.Utility
{

    public static class Strings {

        #region digit counting

        /// <summary>
        ///     Counts the number of zeroes that come before any other digit.
        /// </summary>
        public static int CountUninformativeZeroes (double value) {

            int boringZeroes = 0;
            string numericalValueString = value.ToString();
            bool firstInformativeDigitReached = false;

            for (int i = 0; i < numericalValueString.Length && firstInformativeDigitReached == false; i++) {
                char c = numericalValueString[index: i];

                if (char.IsDigit(c: c)) {
                    if (c == '0') {
                        boringZeroes++; // Uninteresting zero, doesn't count as information
                    } else {
                        firstInformativeDigitReached = true;
                    }
                }
            }

            return boringZeroes;
        }

        public static int CountInformativeDigitsBeforeDecimal (double value) {

            if (Math.Abs(value) < 1) {
                return 0;
            }

            int digits = 0;

            string numericalValueString = Mathematik.RoundToInt(value).ToString();

            for (int i = 0; i < numericalValueString.Length; i++) {
                char c = numericalValueString[index: i];

                if (char.IsDigit(c: c)) {
                    digits++;
                }
            }

            return digits;
        }

        public static int ZerosAfterDecimalBeforeInterestingDigitsStart (double value) {
            int zeros = 0;
            double decimalsOnly = value % 1.0;

            if (decimalsOnly >= 1) {
                throw new Exception();
            }

            zeros -= 1; // For the zero before the decimal.
            string numericalValueString = decimalsOnly.ToString();

            for (int i = 0; i < numericalValueString.Length; i++) {
                char c = numericalValueString[index: i];

                if (char.IsDigit(c: c)) {
                    if (c == '0') {
                        zeros++;
                    } else {
                        return zeros;
                    }
                }
            }

            return zeros;
        }

        #endregion

        #region punctuation, capitalisation, line breaks

        public static int CountLineBreaks (string text) {
            return text.Count(predicate: c => IsLineBreak(c: c));
        }

        public static string CollapseToAcronym (
            string text,
            bool includeFirstLetterRegardlessOfCapitalization = false,
            bool preserveNonLetterCharacters = true
        ) {

            string acronym = "";
            int startIndex = 0;

            if (includeFirstLetterRegardlessOfCapitalization) {
                acronym += text.First();
                startIndex = 1;
            }

            for (int i = startIndex; i < text.Length; i++) {
                char c = text[index: i];

                if (IsMajuscule(c: c) || preserveNonLetterCharacters && IsLetter(c: c) == false) {
                    acronym += c;
                }
            }

            return acronym;
        }

        public static string SeparateCapitalisedWords (string text, bool keepAbbreviations) {
            for (int i = 1; i < text.Length; i++) {
                if (IsMajuscule(c: text[index: i])) {
                    if (keepAbbreviations
                     && IsMajuscule(c: text[index: i - 1])
                     && IsLetter(c: text[index: i - 1])) {
                        // Is part of an abbreviation, keep it.
                    } else if (text[index: i - 1] == ' ') {
                        // Is already a space, leave it.
                    } else {
                        text = text.Insert(startIndex: i, value: " ");
                        i++;
                    }
                }
            }

            return text;
        }

        public static string CamelCaseToWords (string camelCasedText) {
            string words = camelCasedText;
            words = CapitaliseFirstLetter(words);
            words = SeparateCapitalisedWords(words, true);
            return words;
        }

        public static string PunctuateIfNecessary (string text, char punctuation) {
            if (!IsPunctuation(c: text.Substring(startIndex: text.Length - 1, length: 1).ToCharArray()[0])) {
                return text + punctuation;
            }

            return text;
        }

        public static string RemoveLineBreaks (string text) {
            return Remove(text: text, "\n", "\r");
        }

        public static string InsertLineBreaks (
            string text,
            int desiredLength,
            string firstLineStarter = "",
            string newLineStarter = ""
        ) {

            if (text == null) {
                throw new NullReferenceException();
            }

            if (text.Length <= desiredLength) {
                return text;
            }

            text = firstLineStarter + text;

            int lastSpace = 0;
            bool spaceFoundInCurrentLine = false;
            int currentLineLength = 0;

            for (int i = 0; i < text.Length; i++) {
                char c = text[index: i];
                currentLineLength += 1;

                if (IsWhitespace(c: c)) {
                    lastSpace = i;
                    spaceFoundInCurrentLine = true;
                }

                if (text.Substring(startIndex: i, length: i < text.Length - 1 ? 2 : 1).Contains(value: "\n")) {
                    currentLineLength = 0;
                    spaceFoundInCurrentLine = false;
                }

                if (currentLineLength > desiredLength) {
                    if (spaceFoundInCurrentLine == false) {
                        throw new Exception(
                                                               message: "Text "
                                                                      + text
                                                                      + " contains insufficient spaces to be broken into lines of length "
                                                                      + desiredLength
                                                                      + "."
                                                              );
                    }

                    text = text.Remove(startIndex: lastSpace, count: 1);
                    text = text.Insert(startIndex: lastSpace, value: "\n" + newLineStarter);
                    currentLineLength = 0;
                    spaceFoundInCurrentLine = false;
                }
            }

            return text;
        }

        public static string BeginEachLineWith (string text, string lineStarter) {
            text = lineStarter + text;

            for (int i = 0; i < text.Length; i++) {
                if (text.Length > i + 1 && IsLineBreak(s: text.Substring(startIndex: i, length: 2))) {
                    text.Insert(startIndex: i + 2, value: lineStarter);
                }
            }

            return text;
        }

        public static string CapitaliseFirstLetter (string s) {
            if (s == null) {
                return null;
            }

            if (s.Length > 1) {
                return char.ToUpper(c: s[index: 0]) + s.Substring(startIndex: 1);
            }

            return s.ToUpper();
        }

        public static string ReportLineLengths (string text) {

            string lineLengths = "";

            foreach (string line in SplitIntoLines(text: text)) {
                lineLengths += line.Length.ToString().PadRight(totalWidth: 4) + " " + line + "\n";
            }

            return lineLengths;
        }

        public static string GetFirstLine (string text) {
            return GetFirstLines(text: text, lines: 1).First();
        }

        public static List<string> GetFirstLines (string text, int lines) {
            return SplitIntoLines(text: text, lineCountLimit: lines);
        }

        public static List<string> SplitIntoLines (
            string text,
            bool preservePairedLineBreaksAsEmptyLines = true,
            int? lineCountLimit = null
        ) {

            string originalText = text; // For debugging

            if (IsWhitespace(s: text)) {
                throw new Exception(message: "Text \"" + originalText + "\" is purely whitespace.");
            }
            if (text.Length == 0) {
                throw new Exception(message: "Text \"" + originalText + "\" has no length.");
            }

            if (preservePairedLineBreaksAsEmptyLines) {

                List<string> lines = new List<string>();

                int safetyCatch = text.Length + 10;

                for (int c = 0; c < text.Length;) {

                    #region stop if the limit is reached

                    if (lineCountLimit != null && lines.Count > lineCountLimit.Value) {
                        return lines;
                    }

                    #endregion

                    #region stop if it gets out of hand

                    safetyCatch--;

                    if (safetyCatch < 0) {
                        throw new Exception(
                                                            message: originalText
                                                                   + "\n"
                                                                   + "'"
                                                                   + CollapseIntoSingleLine(
                                                                                                    text: text,
                                                                                                    separator: "[\\n]"
                                                                                                   )
                                                                   + "'"
                                                                   + "\n"
                                                                   + lines.ToString()
                                                                   + "\n"
                                                           );
                    }

                    #endregion

                    if (IsLineBreak(c: text[index: c])) {
                        if (c > 0) {
                            // First off, let's get all the text up to this point.

                            lines.Add(item: text.Substring(startIndex: 0, length: c));
                            text = text.Remove(startIndex: 0, count: c);
                            c = 0;
                        }

                        // Line break reached
                        if (c < text.Length - 1) {
                            if (IsLineBreak(c: text[index: c + 1])) {
                                // Another line break straight ahead, combine them into one blank line
                                lines.Add(item: "");
                                text = text.Remove(startIndex: 0, count: 1);
                                c = 0;
                            } else {
                                // Single line break; drop it. We know it's there because the text is saved to the list as a distinct line.
                                text = text.Remove(startIndex: 0, count: 1);
                                c = 0;
                            }
                        } else {
                            // Text has reached its end; this last line break will be included as a blank line at the end.
                            lines.Add(item: "");
                            c++; // This terminates the for loop under these conditions.
                        }
                    } else {
                        // This isn't a line break; keep going.
                        c++;
                    }

                }

                // Add the rest of the text to the lines
                lines.Add(item: text);

                return lines;
            }

            return text.Split('\n', '\r').ToList(); // This simply removes line breaks
        }

        public static bool ContainsLineBreak (string text) {
            foreach (char c in text) {
                if (IsLineBreak(c: c)) {
                    return true;
                }
            }

            return false;
        }

        public static string CollapseIntoSingleLine (string text, string separator = "") {
            return text.Replace(oldValue: "\n", newValue: separator).Replace(oldValue: "\r", newValue: separator);
        }

        public static int CountLines (string text) {
            return SplitIntoLines(text: text).Count;
        }

        public static int LongestLineWidth (string text) {
            return SplitIntoLines(text: text).Max(selector: line => line.Length);
        }
        #endregion

        public static List<string> SeparateWords (string text) {
            return text.Split(' ').ToList();
        }
        public static List<string> SeparateWordsButRecombineParantheses (string text) {
            List<string> words = SeparateWords(text);

            for (int i = 0; i < words.Count; i++) {
                string word = words[i];
                bool beginsWithOpening = word.First() == '(';
                bool endsWithClosing = word.Last() == ')';

                if (beginsWithOpening && endsWithClosing || !beginsWithOpening && !endsWithClosing) {
                    continue;
                } else if (beginsWithOpening) {
                    string combined = word + " " + words[i + 1];
                    words.RemoveAt(i + 1);
                    words[i] = combined;

                    if (combined.Last() == ')') {
                        continue;
                    } else {
                        i--;
                    }
                }
            }

            for (int i = 0; i < words.Count; i++) {
                words[i] = words[i].Trim('(', ')');
            }

            Console.WriteLine(text + " -> " + words);

            return words;
        }

        #region content changes

        public static string ForceLengthPadLeft (string text, int length, string forcedPadding = "") {
            string cut = CutToSize(text: text, length: length);
            string cutAndPadded = forcedPadding + cut.PadLeft(totalWidth: length);

            return cutAndPadded;
        }

        public static string ForceLengthPadRight (string text, int length, string forcedPadding = "") {
            string cut = CutToSize(text: text, length: length);
            string cutAndPadded = cut.PadRight(totalWidth: length) + forcedPadding;

            return cutAndPadded;
        }

        public static string CutToSize (string text, int length) {
            return text.Substring(startIndex: 0, length: Math.Min(length, text.Length));
        }

        public static string Remove (string text, params string[] removees) {
            foreach (string removee in removees) {
                for (int i = 0; i < text.Length - removee.Length; i++) {
                    if (text.Substring(startIndex: i, length: removee.Length) == removee) {
                        text.Remove(startIndex: i, count: removee.Length);
                    }
                }
            }

            return text;
        }

        /// <summary>
        ///     Removes all connected whitespaces from the index provided to the first non-whitespace character.
        /// </summary>
        public static string RemoveWhitespaceSequence (string text, int startIndex = 0) {
            for (int i = startIndex;
                 i < text.Length
              && IsWhitespace(
                                      c: text[index: i]
                                     ); /* No increment because the text collapses to the left as chars are removed. */) {
                text = text.Remove(startIndex: i, count: 1);
            }

            return text;
        }

        public static string TrimNewlinesAndWhitespaces (string text) {
            return TrimNewlines(text: text.Trim());
        }

        public static string TrimNewlines (string text) {
            return text.TrimStart(trimChars: Chars.newline).TrimEnd(trimChars: Chars.newline);
        }

        #endregion

        #region miscellaneous

        public static string ReportArrayContents<T> (T[] array) {

            string report = "";

            foreach (T item in array) {
                if (report.Length > 1) {
                    report += ", ";
                }

                report += item;
            }

            return report;
        }

        public static string ReportDictionaryContents<T1, T2> (Dictionary<T1, T2> dictionary) {
            return ReportDictionaryContents(
                                                    dictionary: dictionary,
                                                    stringProducer: (t1, t2) => t2 + Chars.multiplication + t1
                                                   );
        }

        public static string ReportDictionaryContents<T1, T2> (
            Dictionary<T1, T2> dictionary,
            Func<T1, T2, string> stringProducer
        ) {

            if (dictionary.Count == 0) {
                return "Empty Dictionary";
            }

            string report = "";

            foreach (T1 key in dictionary.Keys) {
                if (report.Length > 1) {
                    report += ", ";
                }

                report += stringProducer.Invoke(arg1: key, arg2: dictionary[key: key]);
            }

            return report;
        }

        public static int NextNonWhitespaceIndex (string text, int startIndex = 0) {
            for (int i = startIndex; i < text.Length; i++) {
                if (IsWhitespace(c: text[index: i]) == false) {
                    return i;
                }
            }

            throw new Exception(
                                             message: "String "
                                                    + text
                                                    + " contains no non-whitespace characters after index "
                                                    + startIndex
                                                    + "."
                                            );
        }

        /// <summary>
        ///     <param name="skipWhitespaceAndIncompleteWord">
        ///         If false, then placing startIndex in the middle of a word will return the remainder of that word, but not the
        ///         next one.
        ///         If true, will skip ahead to the next complete word and return that.
        ///     </param>
        public static string NextWord (string text, int startIndex = 0, bool skipWhitespaceAndIncompleteWord = true) {

            // Finding the first non-whitespace character's index; i.e. the start of the next word.
            for (;
                startIndex < text.Length
             && (startIndex > 0 && IsWhitespace(c: text[index: startIndex - 1]) == false
              || IsWhitespace(c: text[index: startIndex]));
                startIndex++) {
                /* All handled by the for specification */
            }

            // The text is all whitespace.
            if (startIndex >= text.Length) {
                //throw new Exception("No word in " + text + " after index " + startIndex + ".");
                return null;
            }

            int length = 0;

            // Find out how long this word is.
            for (int i = startIndex; i < text.Length; i++) {
                if (IsWhitespace(c: text[index: i])) {
                    return text.Substring(startIndex: i - length, length: length);
                }

                length++;
            }

            // The word occupies the remainder of the text, so return all that.
            return text.Substring(startIndex: startIndex);
        }

        public static int LengthOfNextWord (string text, int startIndex = 0) {
            return NextWord(text: text, startIndex: startIndex).Length;

        }

        public static bool IsLineBreak (char c) {
            return c == '\n';
        }

        public static bool IsLineBreak (string s) {
            return s == "\n";
        }

        public static bool HasContent (string s) {
            return !IsWhitespaceOrNullOrEmpty(s);
        }

        public static bool IsOrReadsNullOrEmpty (string s) {
            return s == null || s == "" || s == "NULL";
        }

        public static bool IsWhitespaceOrNullOrEmpty (string s) {
            return string.IsNullOrEmpty(value: s) || IsWhitespace(s: s);
        }

        public static bool IsWhitespace (string s) {
            foreach (char c in s) {
                if (IsWhitespace(c: c) == false) {
                    return false;
                }
            }
            return true;
        }

        public static bool IsWhitespace (char c) {
            return c == Chars.tab || c == ' ' || c.ToString() == " " || c == '\r' || c == '\n';
        }

        public static bool IsMajuscule (char c) {
            return c.ToString() != c.ToString().ToLower() && IsLetter(c: c);
        }

        public static bool IsPunctuation (char c) {
            return c == '.' || c == ',' || c == ':' || c == ';' || c == '?' || c == '!';
        }

        public static bool IsKonsonant (char c) {
            return IsVokal(c: c) == false && IsLetter(c: c);
        }

        public static bool IsLetter (char c) {
            return c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z';
        }

        public static bool IsVokal (char c) {
            c = char.ToLower(c: c);

            return c == 'a' || c == 'e' || c == 'i' || c == 'o' || c == 'u' || c == 'y' || c == '_' || c == '-'
                /*|| c == 'ä' || c == 'ö' || c == 'ü'*/;
        }

        #endregion

    }

}
