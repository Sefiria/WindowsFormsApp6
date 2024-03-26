using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using static Script.InputScript;

namespace Script
{
    public partial class InputScriptEditor : Form
    {
        public class TestResult
        {
            public bool TestPass;
            public int WordID;
            public string Error;

            public TestResult()
            {
                TestPass = true;
                WordID = -1;
                Error = "";
            }
            public TestResult(bool _TestPass, int _WordID, string _Error)
            {
                TestPass = _TestPass;
                WordID = _WordID;
                Error = _Error;
            }
        }
        public class RawInfo
        {
            public int WordIndex, StartCharIndex, Length;
            public KeywordType Type;
            public string Text;

            public RawInfo(int _WordIndex, int _StartCharIndex, int _Length, KeywordType _Type, string _Text)
            {
                WordIndex = _WordIndex;
                StartCharIndex = _StartCharIndex;
                Length = _Length;
                Type = _Type;
                Text = _Text;
            }
        }

        Action Save;
        InputScriptEditor_HelpList HelpListForm = null;
        InputScript Script;
        Dictionary<string, string> Variables = new Dictionary<string, string>();
        Dictionary<int, (RawInfo Info, string Reason)> errors = new Dictionary<int, (RawInfo Info, string Reason)>();

        public InputScriptEditor(InputScript Script, Type EntityType, Action Save)
        {
            InitializeComponent();

            SetProperties(EntityType.GetProperties().Where(x => x.GetCustomAttributes(typeof(ScriptPropertyAttribute)).Count() == 1).Select(x => x.Name).ToList());

            this.Save = Save;
            this.Script = Script;
            rtbScript.Text = Script.script;
            rtbScript.SelectionStart = rtbScript.Text.Length;
            rtbScript.Focus();
            AutoFontAll();
            GetVariables();
            AutoFontVariables();
            ActiveControl = rtbScript;
        }

        private void btHelpList_Click(object sender, EventArgs e)
        {
            if(HelpListForm == null)
            {
                HelpListForm = new InputScriptEditor_HelpList(ColorFromKeywordType, Callback_AddStringFromHelpList);
                HelpListForm.Show();
            }
            else
            {
                HelpListForm.Close();
                HelpListForm = null;
            }
        }
        private void btTest_Click(object sender, EventArgs e)
        {
            Test();
        }
        private void btSave_Click(object sender, EventArgs e)
        {
            if (Test(false))
            {
                Script.script = rtbScript.Text;
                Save();
                MessageBox.Show("Saved.");
            }
            else
            {
                MessageBox.Show("Save failed : the test failed.");
                if(MessageBox.Show("Force Save ?", "Force Save", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Script.script = rtbScript.Text;
                    Save();
                    MessageBox.Show("Saved (force).");
                }
            }
        }
        private void Callback_AddStringFromHelpList(string scriptRaw, KeywordType type)
        {
            rtbScript.SelectionLength = 0;
            rtbScript.SelectionColor = ColorFromKeywordType(type);
            rtbScript.SelectedText = scriptRaw + ' ';
        }
        private Color ColorFromKeywordType(KeywordType type)
        {
            switch(type)
            {
                default:
                case KeywordType.Unknown: return Color.White;
                case KeywordType.Function: return Color.DeepSkyBlue;
                case KeywordType.Condition: return Color.MediumAquamarine;
                case KeywordType.Subject: return Color.LightGoldenrodYellow;
                case KeywordType.Operator: return Color.GreenYellow;
                case KeywordType.Assignator: return Color.HotPink;
                case KeywordType.Comparator: return Color.SpringGreen;
                case KeywordType.Modifier: return Color.Lavender;
                case KeywordType.Value: return Color.Gold;
            }
        }

        private bool Test(bool showDialog = true)
        {
            HighlightErrors();

            if (showDialog)
            {
                if (errors.Count > 0)
                    MessageBox.Show("There's some errors : the test failed.");
                else
                    MessageBox.Show("No error : the test is successful.");
            }

            return errors.Count == 0;
        }

        private void AutoFontAll()
        {
            var words = rtbScript.Text.Split(new char[] { ' ', '\n' }, StringSplitOptions.None);

            for(int i=0; i<words.Length; i++)
            {

                var info = GetRawInfoFromWordIndex(i);
                if (string.IsNullOrWhiteSpace(info.Text))
                    continue;

                var color = IsNonErrorWord(words[i]) ? Color.Magenta : ColorFromKeywordType(info.Type);
                var previousCursorPosition = rtbScript.SelectionStart;

                rtbScript.TextChanged -= rtbScript_TextChanged;
                rtbScript.SuspendLayout();
                rtbScript.SelectionStart = info.StartCharIndex;
                rtbScript.SelectionLength = info.Length;
                rtbScript.SelectionColor = color;
                rtbScript.SelectionFont = new Font(rtbScript.SelectionFont, (info.Type != KeywordType.Unknown ? FontStyle.Bold : FontStyle.Regular));

                rtbScript.SelectionStart = previousCursorPosition;
                rtbScript.SelectionLength = 0;
                rtbScript.ResumeLayout();
                rtbScript.TextChanged += rtbScript_TextChanged;

            }
        }
        private string AutoFontCurrentModification()
        {
            var info = GetRawInfoFromCursor();
            if (string.IsNullOrWhiteSpace(info.Text))
                return "";

            var color = IsNonErrorWord(info.Text) ? Color.Magenta : ColorFromKeywordType(info.Type);
            var previousCursorPosition = rtbScript.SelectionStart;

            rtbScript.SuspendLayout();
            rtbScript.SelectionStart = info.StartCharIndex;
            rtbScript.SelectionLength = info.Length;
            rtbScript.SelectionColor = color;
            rtbScript.SelectionFont = new Font(rtbScript.SelectionFont, (info.Type != KeywordType.Unknown ? FontStyle.Bold : FontStyle.Regular));

            rtbScript.SelectionStart = previousCursorPosition;
            rtbScript.SelectionLength = 0;
            rtbScript.ResumeLayout();

            return info.Text;
        }
        private void AutoFontVariables()
        {
            var words = rtbScript.Text.Split(new char[] { ' ', '\n' }, StringSplitOptions.None);

            for (int i = 0; i < words.Length; i++)
            {
                var info = GetRawInfoFromWordIndex(i);

                if (info.Type == KeywordType.Unknown && (IsVariable(words[i]) || IsPropertyStructure(words[i])))
                {
                    var previousCursorPosition = rtbScript.SelectionStart;

                    rtbScript.SuspendLayout();
                    rtbScript.SelectionStart = info.StartCharIndex;
                    rtbScript.SelectionLength = info.Length;
                    rtbScript.SelectionColor = Color.White;
                    rtbScript.SelectionFont = new Font(rtbScript.SelectionFont, FontStyle.Italic);
                    rtbScript.SelectionStart = previousCursorPosition;
                    rtbScript.SelectionLength = 0;
                    rtbScript.ResumeLayout();
                }
            }
        }
        private RawInfo GetRawInfoFromWordIndex(int WordIndex)
        {
            var Text = rtbScript.Text.Split(new char[]{ ' ', '\n' }, StringSplitOptions.None)[WordIndex];
            return GetRawInfo(Text, false, WordIndex);
        }
        private RawInfo GetRawInfoFromCursor()
        {
            var Text = rtbScript.Text.Substring(0, rtbScript.SelectionStart).Split(new char[] { ' ', '\n' }, StringSplitOptions.None).Last();
            return GetRawInfo(Text, true);
        }
        private RawInfo GetRawInfo(string Text, bool FromCursor = false, int WordIndex = -1)
        {
            var Length = Text.Length;
            int CharIndex;
            if (FromCursor)
            {
                int ncharsAfterCursor = 0;
                for (int i = rtbScript.SelectionStart; i < rtbScript.Text.Length; i++)
                    if (rtbScript.Text[i] != ' ' && rtbScript.Text[i] != '\n') ncharsAfterCursor++;
                    else break;
                Text = rtbScript.Text.Substring(0, rtbScript.SelectionStart + ncharsAfterCursor).Split(new char[] { ' ', '\n' }).Last();
                Length = Text.Length;
                CharIndex = rtbScript.Text.Substring(0, rtbScript.SelectionStart).Length + ncharsAfterCursor - Length;
            }
            else
            {
                var words = rtbScript.Text.Split(new char[] { ' ','\n' }, StringSplitOptions.None);
                var list = new List<string>();
                for (int i = 0; i < WordIndex; i++)
                    list.AddRange(new string[] { words[i], " " });

                CharIndex = string.Concat(list).Count();
            }

            var Type = GetTypeFromRaw(Text);

            return new RawInfo(WordIndex, CharIndex, Length, Type, Text);
        }
        private void HighlightErrors()
        {
            errors.Clear();

            string line = "";
            string word = "";
            string[] words;
            string[] ArgumentValues = null;

            var lines = rtbScript.Text.Split(new char[] { '\n' }, StringSplitOptions.None);
            for (int y = 0; y < lines.Length; y++)
            {
                line = lines[y];
                words = line.Split(new char[] { ' ' }, StringSplitOptions.None);
                var TestCondition = new TestResult();

                int firstWordNotModifier = 0;
                for (int w = 0; w < words.Length; w++)
                    if (IsModifier(words[w])) firstWordNotModifier++;
                    else break;

                word = words[firstWordNotModifier];
                string[] Arguments;
                string ArgumentType;
                TestResult test = new TestResult(true, -1, "");

                switch (GetTypeFromRaw(word, Variables))
                {
                    case KeywordType.Function:
                        Arguments = GetVerbFromSyntax(Functions, word).Arguments;
                        foreach (string arg in Arguments)
                        {
                            ArgumentType = arg.Split(':')[0];
                            ArgumentValues = arg.Split(':')[1].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string argValue in ArgumentValues)
                            {
                                if ((test = TestStructure(words, firstWordNotModifier, $"${ArgumentType}", argValue)).TestPass) break;
                            }
                        }
                        break;
                    case KeywordType.Condition:

                        bool CheckFuncArgsCount(int wordIndex)
                        {
                            int ArgumentsLength = GetVerbFromSyntax(Functions, words[5]).Arguments.Length;
                            if (words.Skip(6).ToArray().Length != ArgumentsLength)
                            {
                                test = new TestResult(false, 6, $"Expected function arguments counts : {ArgumentsLength}");
                                return false;
                            }
                            return true;
                        }

                        if (TestStructure(words, firstWordNotModifier, "if", "$Subject", "$Comparator", "@VAL@", "then", "$Subject", "$Assignator", "@VAL@").TestPass)
                            break;

                        if (TestStructure(words, firstWordNotModifier, "if", "$Subject", "$Comparator", "@VAL@", "then", "$Function", "@FUNCARGS@").TestPass)
                            if(CheckFuncArgsCount(5))
                                break;

                        if (TestStructure(words, firstWordNotModifier, "if", "$Variable", "$Comparator", "@VAL@", "then", "$Subject", "$Assignator", "@VAL@").TestPass)
                            break;

                        if (TestStructure(words, firstWordNotModifier, "if", "$Variable", "$Comparator", "@VAL@", "then", "$Function", "@FUNCARGS@").TestPass)
                            if (CheckFuncArgsCount(5))
                                break;

                        if (TestStructure(words, firstWordNotModifier, "if", "$PropertyStructure", "$Comparator", "@VAL@", "then", "$Subject", "$Assignator", "@VAL@").TestPass)
                            break;

                        if (TestStructure(words, firstWordNotModifier, "if", "$PropertyStructure", "$Comparator", "@VAL@", "then", "$Function", "@FUNCARGS@").TestPass)
                            if (CheckFuncArgsCount(5))
                                break;

                        if (TestStructure(words.Skip(firstWordNotModifier).Take(5).ToArray(), 0, "if", "$Subject", "$Assignator", "@VAL@", "$LogicalOperator").TestPass)
                        {
                            List<string[]> conditions = new List<string[]>();
                            List<string> logicalOperator = new List<string>();
                            int curId = 0;
                            do
                            {
                                if (curId > 0)
                                {
                                    logicalOperator.Add(words[curId]);
                                }
                                curId ++;
                                conditions.Add(words.Skip(curId).Take(3).ToArray());
                                curId += 3;
                            }
                            while (GetTypeFromRaw(words[curId]) == KeywordType.LogicalOperator);// Get conditions and logicalOperators

                            if (TestStructure(words.Skip(firstWordNotModifier + curId).Take(4).ToArray(), 0, "then", "$Subject", "$Assignator", "@VAL@").TestPass)
                                break;

                            if (TestStructure(words.Skip(firstWordNotModifier + curId).Take(3).ToArray(), 0, "then", "$Function", "@FUNCARGS@").TestPass)
                                break;
                        }
                        
                        string errorMsg = "";
                        int errorWordId = 1;
                        TestAfterLogicalOperator:
                        if (GetTypeFromRaw(words[errorWordId++]) != KeywordType.Subject &&
                            GetTypeFromRaw(words[errorWordId - 1]) != KeywordType.Variable &&
                            GetTypeFromRaw(words[errorWordId - 1]) != KeywordType.PropertyStructure) errorMsg = "Expected: Subject";
                   else if (GetTypeFromRaw(words[errorWordId++]) != KeywordType.Comparator) errorMsg = "Expected: Comparator";
                   else if (GetTypeFromRaw(words[errorWordId++]) != KeywordType.Value &&
                               GetTypeFromRaw(words[errorWordId - 1]) != KeywordType.Variable &&
                               GetTypeFromRaw(words[errorWordId - 1]) != KeywordType.PropertyStructure) errorMsg = "Expected: Value/Variable/PropertyStructure";
                   else if (words[errorWordId++].CompareTo("then") != 0)
                   {
                       if (GetTypeFromRaw(words[errorWordId - 1]) != KeywordType.LogicalOperator) errorMsg = "Expected: 'then'/LogicalOperator";
                       else
                       {
                           goto TestAfterLogicalOperator;
                       }
                   }
                   else if (GetTypeFromRaw(words[errorWordId++]) != KeywordType.Function)
                   {
                       if (GetTypeFromRaw(words[errorWordId++]) != KeywordType.Value &&
                           GetTypeFromRaw(words[errorWordId - 1]) != KeywordType.Variable &&
                           GetTypeFromRaw(words[errorWordId - 1]) != KeywordType.PropertyStructure) errorMsg = "Expected: Function/Value/Variable/PropertyStructure";
                   }
                   else
                   {
                        int ArgumentsLength = GetVerbFromSyntax(Functions, words[errorWordId - 1]).Arguments.Length;
                        if(words.Skip(errorWordId).ToArray().Length != ArgumentsLength)
                            errorMsg = $"Expected function arguments counts : {ArgumentsLength}";
                   }

                        if (!string.IsNullOrWhiteSpace(errorMsg))
                        {
                            test = new TestResult(false, errorWordId - 1, errorMsg);
                        }

                        break;

                    case KeywordType.NonErrorWord: Arguments = GetVerbFromSyntax(NonErrorWords, word).Arguments; break;
                    case KeywordType.Subject:
                        if (!(test = TestStructure(words, firstWordNotModifier, "$Subject", "++")).TestPass) break;
                        if (!(test = TestStructure(words, firstWordNotModifier, "$Subject", "--")).TestPass) break;
                        if (!(test = TestStructure(words, firstWordNotModifier, "$Subject", "$Assignator", "@VAL@")).TestPass) break;
                        if (!(test = TestStructure(words, firstWordNotModifier, "$Subject", "$Assignator", "$Subject")).TestPass) break;
                        break;
                    case KeywordType.Variable:
                        if (!(test = TestStructure(words, firstWordNotModifier, "$Variable", "++")).TestPass) break;
                        if (!(test = TestStructure(words, firstWordNotModifier, "$Variable", "--")).TestPass) break;
                        if (!(test = TestStructure(words, firstWordNotModifier, "$Variable", "$Assignator", "@VAL@")).TestPass) break;
                        if (!(test = TestStructure(words, firstWordNotModifier, "$Variable", "$Assignator", "$Variable")).TestPass) break;
                        if (!(test = TestStructure(words, firstWordNotModifier, "$Variable", "$Assignator", "$PropertyStructure")).TestPass) break;
                        break;
                    case KeywordType.PropertyStructure:
                        if (!(test = TestStructure(words, firstWordNotModifier, "$PropertyStructure", "++")).TestPass) break;
                        if (!(test = TestStructure(words, firstWordNotModifier, "$PropertyStructure", "--")).TestPass) break;
                        if (!(test = TestStructure(words, firstWordNotModifier, "$PropertyStructure", "$Assignator", "@VAL@")).TestPass) break;
                        if (!(test = TestStructure(words, firstWordNotModifier, "$PropertyStructure", "$Assignator", "$Variable")).TestPass) break;
                        if (!(test = TestStructure(words, firstWordNotModifier, "$PropertyStructure", "$Assignator", "$PropertyStructure")).TestPass) break;
                        break;
                }

                if (!TestCondition.TestPass)
                {
                    var ID = TestCondition.WordID;
                    if (TestCondition.WordID >= 0)
                    {
                        for (int j = 0; j < y; j++)
                            ID += lines[j].Split(new char[] { ' ' }, StringSplitOptions.None).Length;
                    }
                    else
                    {
                        ID = -y;
                    }
                    HighlightThatError(ID, TestCondition.Error);
                }
            }

            words = rtbScript.Text.Split(new char[] { ' ', '\n' }, StringSplitOptions.None);
            for (int i = 0; i < words.Length; i++)
            {
                word = words[i];

                if (string.IsNullOrWhiteSpace(word))
                    continue;

                if (GetTypeFromRaw(word) == KeywordType.Unknown)
                {
                    if (!IsPropertyStructure(word) && !IsNonErrorWord(word) && !Variables.Keys.Contains(word) && (ArgumentValues == null ? true : !ArgumentValues.ToList().Contains(word)))
                    {
                        HighlightThatError(i, "Unrecognized verb");
                    }
                }
            }
        }
        private TestResult TestStructure(string[] _Words, int firstWordIndex, params string[] Values)
        {
            var Words = _Words.Skip(firstWordIndex).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            bool mayBeModifier = true;
            int firstWordNonModifierID = 0;

            for (int i=0; i< Words.Length; i++)
            {
                if (IsLogicalOperator(Words[i]))
                {
                    var newValues = Values.ToList();
                    if (i >= newValues.Count)
                        i = newValues.Count;
                    newValues.Insert(i, "$LogicalOperator");
                    newValues.Insert(++i, "@VAL@");
                    newValues.Insert(++i, "$Comparator");
                    newValues.Insert(++i, "@VAL@");
                    Values = newValues.ToArray();
                    continue;
                }

                if (IsOperator(Words[i]))
                {
                    var newValues = Values.ToList();
                    newValues.Insert(i, "$Operator");
                    newValues.Insert(++i, "@VAL@");
                    Values = newValues.ToArray();
                    continue;
                }

                if (IsModifier(Words[i]))
                {
                    if (mayBeModifier)
                    {
                        var newValues = Values.ToList();
                        newValues.Insert(i, "$Modifier");
                        Values = newValues.ToArray();
                        firstWordNonModifierID++;
                        continue;
                    }
                    else
                        return new TestResult(false, i, "Modifier can only appears at beggning of a sentence (#ARG" + Words[i] + ", #TYPE : " + KeywordType.Modifier.ToString() + ")");
                }
                else
                    mayBeModifier = false;// Modifier can only be at begining of a sentence
            }

            if (Values[0] == "$Function" || (Values.Length > 5 + firstWordNonModifierID ? Values[5 + firstWordNonModifierID] == "$Function" : false))
            {
                if (IsFunction(Words[firstWordNonModifierID]))
                {
                    if (GetVerbFromSyntax(Functions, Words[firstWordNonModifierID]).Arguments.Length == 0)
                        Values = Values.Take(Values.Count() - 1).ToArray();
                }
                else
                {
                    if(Words.Length > 5 + firstWordNonModifierID)
                        if (IsFunction(Words[5 + firstWordNonModifierID]))
                            if (GetVerbFromSyntax(Functions, Words[5 + firstWordNonModifierID]).Arguments.Length == 0)
                                Values = Values.Take(Values.Count() - 1).ToArray();
                }
            }

            if (Values.ToList().IndexOf("$Function") > -1)
            {
                if (IsFunction(Words[Values.ToList().IndexOf("$Function")]))
                {
                    if (GetVerbFromSyntax(Functions, Words[Values.ToList().IndexOf("$Function")]).Arguments.Length > 0)
                    {
                        if (Words.Length < Values.Length)
                            return new TestResult(false, -1, "Wrong structure length (#ARG : " + Words.Length + ", #EXPECTED : " + Values.Length + ")");
                        var functionArgsCount = Values.Length - 1;
                        if (Values.Where(x => x == "$Function").Count() == 1 && Values.Where(x => x == "@FUNCARGS").Count() == 1)
                            if (IsFunction(Words[Values.ToList().IndexOf("$Function")]))
                                functionArgsCount += GetVerbFromSyntax(Functions, Words[Values.ToList().IndexOf("$Function")]).Arguments.Length;
                        if (Words.Length < functionArgsCount)
                            return new TestResult(false, -1, "Wrong structure length (#ARG : " + Words.Length + ", #EXPECTED : " + functionArgsCount + ")");
                    }
                }
            }

            bool isFuncArgsEncountered = false;
            int funcArgsEncountered = 0;
            string value;

            for(int i=0; i< Words.Length; i++)
            {
                if (i >= Values.Length)
                {
                    if (isFuncArgsEncountered)
                        value = "@FUNCARGS";
                    else
                        return new TestResult(false, -1, "Wrong syntax (too much arguments than expected)");
                }
                else
                    value = Values[i];

                if (value == "@VAL@")
                {
                    if (GetTypeFromRaw(Words[i]) != KeywordType.Value && GetTypeFromRaw(Words[i]) != KeywordType.Subject && !IsVariable(Words[i]) && !IsPropertyStructure(Words[i]))
                        return new TestResult(false, i, "Wrong Keyword type (#ARG : " + GetTypeFromRaw(Words[i]).ToString() + ", #EXPECTED : Value|Subject|Variable)");
                }
                else
                {
                    if (value[0] == '$')
                    {
                        if (GetTypeFromRaw(Words[i]) != (KeywordType)Enum.Parse(typeof(KeywordType), string.Concat(value.Skip(1)).Split(':')[0]))
                            return new TestResult(false, i, "Wrong Keyword type (#ARG : " + GetTypeFromRaw(Words[i]).ToString() + ", #EXPECTED : " + string.Concat(value.Skip(1)) + ")");
                    }
                    else
                    {
                        if (value == "@FUNCARGS")
                            isFuncArgsEncountered = true;

                        if(isFuncArgsEncountered)
                        {
                            var argsLength = GetVerbFromSyntax(Functions, Words[Values.ToList().IndexOf("$Function")]).Arguments.Length;
                            if (funcArgsEncountered >= argsLength)
                                return new TestResult(false, i, "Wrong number of arguments (#ARG_N : " + (funcArgsEncountered + 1) + ", #EXPECTED : " + argsLength + ")");
                            var type = GetVerbFromSyntax(Functions, Words[Values.ToList().IndexOf("$Function")]).Arguments[funcArgsEncountered].Split(':')[0];
                            if (GetTypeFromRaw(Words[i], Variables) != (KeywordType)Enum.Parse(typeof(KeywordType), type))
                                return new TestResult(false, i, "The function expected another agrument type (#ARG_N : " + (funcArgsEncountered + 1) + ", #ARG_TYPE : " + GetTypeFromRaw(Words[i]).ToString() + ", #EXPECTED_TYPE : " + type + ")");
                            funcArgsEncountered++;
                        }
                        else
                        {
                            if (Words[i] != value)
                                return new TestResult(false, i, "The verb (" + Words[i] + ") brokes the structure. (#EXPECTED : " + value + ")");
                        }
                    }
                }
            }

            return new TestResult(true, -1, "");
        }
        private void HighlightThatError(int wordsIndex, string reason)
        {
            RawInfo info;
            if (wordsIndex >= 0)
            {
                info = GetRawInfoFromWordIndex(wordsIndex);

                if (string.IsNullOrWhiteSpace(info.Text))
                    return;

                if (!errors.Keys.Contains(info.WordIndex))
                    errors[info.WordIndex] = (info, reason);
            }
            else// wordsIndex is negative, it means that's a complete lineID, not a wordID
            {
                var lines = rtbScript.Text.Split(new char[] { '\n' }, StringSplitOptions.None);
                int id = 0;
                for (int y = 0; y < -wordsIndex; y++)
                    id += lines[y].Split(new char[] { ' ' }, StringSplitOptions.None).Length;

                info = GetRawInfoFromWordIndex(id);
                info.Text = lines[-wordsIndex];
                info.Type = KeywordType.Unknown;
                info.Length = lines[-wordsIndex].Length;

                if (!errors.Keys.Contains(info.WordIndex))
                    errors[info.WordIndex] = (info, reason);
            }
        }
        private void GetVariables()
        {
            Variables.Clear();
            
            foreach (var val in PublicVariables.Keys)
                Variables[val] = val;

            string line = "";

            var lines = rtbScript.Text.Split(new char[] { '\n' }, StringSplitOptions.None);
            for (int y = 0; y < lines.Length; y++)
            {
                line = lines[y];
                var words = line.Split(new char[] { ' ' }, StringSplitOptions.None);

                int firstWordNotModifier = 0;
                for (int w = 0; w < words.Length; w++)
                    if (IsModifier(words[w])) firstWordNotModifier++;
                    else break;

                if (words.Length >= 4)
                    if (words[firstWordNotModifier] == "var")
                        if (!Variables.Keys.Contains(words[firstWordNotModifier + 1]))
                            if (GetTypeFromRaw(words[firstWordNotModifier + 1]) == KeywordType.Unknown)
                                if (IsAssignator(words[firstWordNotModifier + 2]))
                                    if (GetTypeFromRaw(words[firstWordNotModifier + 3]) == KeywordType.Value
                                     || GetTypeFromRaw(words[firstWordNotModifier + 3]) == KeywordType.Subject
                                     || Variables.ContainsKey(words[firstWordNotModifier + 3])
                                     || IsPropertyStructure(words[firstWordNotModifier + 3]))
                                    {
                                        if (GetTypeFromRaw(words[firstWordNotModifier + 3]) == KeywordType.Value)
                                        {
                                            switch (words[firstWordNotModifier + 2])
                                            {
                                                case "=":
                                                    Variables[words[firstWordNotModifier + 1]] = words[firstWordNotModifier + 3];
                                                    break;
                                                case "+=":
                                                    Variables[words[firstWordNotModifier + 1]] = $"{int.Parse(Variables[words[firstWordNotModifier + 1]]) + int.Parse(words[firstWordNotModifier + 3])}";
                                                    break;
                                                case "-=":
                                                    Variables[words[firstWordNotModifier + 1]] = $"{int.Parse(Variables[words[firstWordNotModifier + 1]]) - int.Parse(words[firstWordNotModifier + 3])}";
                                                    break;
                                            }
                                        }
                                        else if(words[firstWordNotModifier + 3] == "=")
                                        {
                                            Variables[words[firstWordNotModifier + 1]] = words[firstWordNotModifier + 3];
                                        }
                                    }
            }
        }
        public bool IsVariable(string word)
        {
            return Variables.ContainsKey(word);
        }
        static public bool IsPropertyStructure(string word)
        {
            if (word.Contains('.'))
            {
                var split = word.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                if (split.Length == 2 && (IsPublicVariable(split[0]) || IsSubject(split[0])))
                    return IsProperty(split[1]);
            }
            return false;
        }


        private void rtbScript_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var raw = AutoFontCurrentModification();

                GetVariables();
                AutoFontVariables();

                HighlightErrors();

                rtbScript.Refresh();
            }
            catch(Exception)
            {

            }
        }
        private void rtbScript_OnDraw(object sender, RichTextBoxExtended.DrawFieldEventArgs e)
        {
            foreach(var error in errors)
            {
                var size = rtbScript.CreateGraphics().MeasureString(error.Value.Info.Text, rtbScript.Font);
                var OneCharSize = rtbScript.CreateGraphics().MeasureString(" ", rtbScript.Font);
                OneCharSize.Width = 7.0F;

                var line = rtbScript.GetLineFromCharIndex(error.Value.Info.StartCharIndex);
                var offset = error.Value.Info.StartCharIndex - line;
                {
                    if (rtbScript.Text.Length > error.Value.Info.StartCharIndex)
                    {
                        var list = rtbScript.Text.Substring(0, error.Value.Info.StartCharIndex).Split('\n').ToList();
                        list.Remove(list.Last());
                        offset -= string.Join("", list).Length;
                    }
                }
                offset *= (int)OneCharSize.Width;

                var points = new List<Point>();
                for (float x = 0; x < (int)size.Width - 0.4F; x+=1F)
                    points.Add(new Point(   (int)(offset + x),
                                            (int)(line * OneCharSize.Height + size.Height + 5 + Math.Sin(x * 4 / Math.PI))));

                e.graphics.DrawCurve(Pens.Red, points.ToArray());
            }
        }

        private void timerErrorHint_Tick(object sender, EventArgs e)
        {
            rtbErrorReason.Text = "";
            foreach (var error in errors)
            {
                var size = rtbScript.CreateGraphics().MeasureString(error.Value.Info.Text, rtbScript.Font);
                var OneCharSize = rtbScript.CreateGraphics().MeasureString(" ", rtbScript.Font);
                OneCharSize.Width = 7.0F;

                var line = rtbScript.GetLineFromCharIndex(error.Value.Info.StartCharIndex);
                var offset = error.Value.Info.StartCharIndex - line;
                {
                    var list = rtbScript.Text.Substring(0, error.Value.Info.StartCharIndex).Split('\n').ToList();
                    list.Remove(list.Last());
                    offset -= string.Join("", list).Length;
                }
                offset *= (int)OneCharSize.Width;

                var wordBound = new Rectangle(  offset,
                                                (int)(line * OneCharSize.Height),
                                                (int)size.Width,
                                                (int)size.Height);
                if (wordBound.Contains(rtbScript.PointToClient(Cursor.Position)))
                    rtbErrorReason.Text = error.Value.Reason;
            }
        }


        public void Execute(object self)
        {
            Script.ScriptMain(self);
        }
    }
}
