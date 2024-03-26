using Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Framework.BehaviorScript;

namespace Editor.BehaviorEdit
{
    public partial class BehaviorEditor : Form
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

        BehaviorEditor_HelpList HelpListForm = null;
        EntityProperties entity = null;
        BehaviorScript behavior = new BehaviorScript();
        Action<BehaviorScript> Callback_UpdateScript = null;
        Dictionary<string, string> Variables = new Dictionary<string, string>();
        Dictionary<int, (RawInfo Info, string Reason)> errors = new Dictionary<int, (RawInfo Info, string Reason)>();

        public BehaviorEditor(ref EntityProperties _entity)
        {
            InitializeComponent();

            entity = _entity;
            behavior = entity.behaviorScript;
            rtbScript.Text = behavior.script;
            rtbScript.SelectionStart = rtbScript.Text.Length;
            rtbScript.Focus();
            AutoFontAll();
            GetVariables();
            AutoFontVariables();
            ActiveControl = rtbScript;
        }
    
        public BehaviorEditor(BehaviorScript _behavior, Action<BehaviorScript> _Callback_UpdateScript)
        {
            InitializeComponent();

            behavior = _behavior;
            Callback_UpdateScript = _Callback_UpdateScript;
            rtbScript.Text = behavior.script;
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
                HelpListForm = new BehaviorEditor_HelpList(ColorFromKeywordType, Callback_AddStringFromHelpList);
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
                behavior.script = rtbScript.Text;
                if (entity != null) { entity.behaviorScript = behavior; entity.Save(); }
                else { Callback_UpdateScript(behavior); }
                MessageBox.Show("Saved.");
            }
            else
            {
                MessageBox.Show("Save failed : the test failed.");
                if(MessageBox.Show("Force Save ?", "Force Save", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (entity != null) entity.behaviorScript = behavior;
                    else Callback_UpdateScript(behavior);
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
                case KeywordType.Normal: return Color.White;
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

            if (errors.Count > 0)
                MessageBox.Show("There's some errors : the test failed.");
            else
                MessageBox.Show("No error : the test is successful.");

            return errors.Count == 0;
        }

        private void rtbScript_TextChanged(object sender, EventArgs e)
        {
            var raw = AutoFontCurrentModification();

            GetVariables();
            AutoFontVariables();

            HighlightErrors();

            rtbScript.Refresh();
        }

        private void AutoFontAll()
        {
            var words = rtbScript.Text.Split(new char[] { ' ', '\n' }, StringSplitOptions.None);

            for(int i=0; i<words.Length; i++)
            {

                var info = GetRawInfoFromWordIndex(i);
                if (string.IsNullOrWhiteSpace(info.Text))
                    continue;

                var color = GetListNonErrorsWords().Contains(words[i]) ? Color.Magenta : ColorFromKeywordType(info.Type);
                var previousCursorPosition = rtbScript.SelectionStart;

                rtbScript.TextChanged -= rtbScript_TextChanged;
                rtbScript.SuspendLayout();
                rtbScript.SelectionStart = info.StartCharIndex;
                rtbScript.SelectionLength = info.Length;
                rtbScript.SelectionColor = color;
                rtbScript.SelectionFont = new Font(rtbScript.SelectionFont, (info.Type != KeywordType.Normal ? FontStyle.Bold : FontStyle.Regular));

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

            var color = GetListNonErrorsWords().Contains(info.Text) ? Color.Magenta : ColorFromKeywordType(info.Type);
            var previousCursorPosition = rtbScript.SelectionStart;

            rtbScript.SuspendLayout();
            rtbScript.SelectionStart = info.StartCharIndex;
            rtbScript.SelectionLength = info.Length;
            rtbScript.SelectionColor = color;
            rtbScript.SelectionFont = new Font(rtbScript.SelectionFont, (info.Type != KeywordType.Normal ? FontStyle.Bold : FontStyle.Regular));

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

                if (info.Type == KeywordType.Normal && (IsVariable(words[i]) || IsProperty(words[i])))
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

            /*if (WordIndex == -1)
            {
                int spaceCount = 0;
                WordIndex = 0;
                for (; WordIndex < rtbScript.TextLength && spaceCount < WordIndex; WordIndex++)
                {
                    if (rtbScript.Text[WordIndex] == ' ' || rtbScript.Text[WordIndex] == '\n')
                        spaceCount++;
                }
            }*/

            return new RawInfo(WordIndex, CharIndex, Length, Type, Text);
        }
        private void HighlightErrors()
        {
            errors.Clear();

            string line = "";
            string word = "";
            string[] words;
            var NonErrorsWords = GetListNonErrorsWords();

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
                if (firstWordNotModifier > 0 && words[firstWordNotModifier] != "var")
                    TestCondition = new TestResult(false, 0, "Modifier is only expected for variable declaration (#ARG : " + words[0] + ", #TYPE : " + KeywordType.Modifier.ToString() + ")");

                if (TestCondition.TestPass)
                {
                    if(IsFunction(words[firstWordNotModifier]))
                        TestCondition = TestStructure(words, "$Function", "@VAL@", "@FUNCARGS");
                    else
                    switch (words[firstWordNotModifier])
                    {
                        case "if":
                            TestCondition = TestStructure(words, "if", "@VAL@", "$Comparator", "@VAL@", "then", "$Function", "$Subject", "@FUNCARGS");
                            break;
                        case "var":
                            TestCondition = TestStructure(words, "var", "$Normal", "$Assignator", "@VAL@");
                            break;
                    }
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

                if (GetTypeFromRaw(word) == KeywordType.Normal)
                {
                    if (!IsProperty(word) && !NonErrorsWords.Contains(word) && !Variables.Keys.Contains(word))
                    {
                        HighlightThatError(i, "Unrecognized verb");
                    }
                }
            }
        }
        private TestResult TestStructure(string[] _Words, params string[] Values)
        {
            var Words = _Words.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
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
                if (GetFunctions().ContainsKey(Words[firstWordNonModifierID]))
                {
                    if (GetFunctions()[Words[firstWordNonModifierID]].args.Length == 0)
                        Values = Values.Take(Values.Count() - 1).ToArray();
                }
                else
                {
                    if(Words.Length > 5 + firstWordNonModifierID)
                        if (GetFunctions().ContainsKey(5 + Words[firstWordNonModifierID]))
                            if (GetFunctions()[Words[5 + firstWordNonModifierID]].args.Length == 0)
                                Values = Values.Take(Values.Count() - 1).ToArray();
                }
            }

            if (Values.ToList().IndexOf("$Function") > -1)
            {
                if (GetFunctions().ContainsKey(Words[Values.ToList().IndexOf("$Function")]))
                {
                    if (GetFunctions()[Words[Values.ToList().IndexOf("$Function")]].args.Length > 0)
                    {
                        if (Words.Length < Values.Length)
                            return new TestResult(false, -1, "Wrong structure length (#ARG : " + Words.Length + ", #EXPECTED : " + Values.Length + ")");
                        var functionArgsCount = Values.Length - 1;
                        if (Values.Where(x => x == "$Function").Count() == 1 && Values.Where(x => x == "@FUNCARGS").Count() == 1)
                            if (GetFunctions().ContainsKey(Words[Values.ToList().IndexOf("$Function")]))
                                functionArgsCount += GetFunctions()[Words[Values.ToList().IndexOf("$Function")]].args.Length;
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
                    if (GetTypeFromRaw(Words[i]) != KeywordType.Value && GetTypeFromRaw(Words[i]) != KeywordType.Subject && !IsVariable(Words[i]) && !IsProperty(Words[i]))
                        return new TestResult(false, i, "Wrong Keyword type (#ARG : " + GetTypeFromRaw(Words[i]).ToString() + ", #EXPECTED : Value|Subject|Variable)");
                }
                else
                {
                    if (value[0] == '$')
                    {
                        if (GetTypeFromRaw(Words[i]) != (KeywordType)Enum.Parse(typeof(KeywordType), string.Concat(value.Skip(1))))
                            return new TestResult(false, i, "Wrong Keyword type (#ARG : " + GetTypeFromRaw(Words[i]).ToString() + ", #EXPECTED : " + string.Concat(value.Skip(1)) + ")");
                    }
                    else
                    {
                        if (value == "@FUNCARGS")
                            isFuncArgsEncountered = true;

                        if(isFuncArgsEncountered)
                        {
                            var argsLength = GetFunctions()[Words[Values.ToList().IndexOf("$Function")]].args.Length;
                            if (funcArgsEncountered >= argsLength)
                                return new TestResult(false, i, "Wrong number of arguments (#ARG_N : " + (funcArgsEncountered + 1) + ", #EXPECTED : " + argsLength + ")");
                            var type = GetFunctions()[Words[Values.ToList().IndexOf("$Function")]].args[funcArgsEncountered].Split(':')[0];
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
                info.Type = KeywordType.Normal;
                info.Length = lines[-wordsIndex].Length;

                if (!errors.Keys.Contains(info.WordIndex))
                    errors[info.WordIndex] = (info, reason);
            }
        }
        private void GetVariables()
        {
            Variables.Clear();

            var PublicVariables = GetPublicVariables();
            foreach (var val in PublicVariables)
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
                        if (!Variables.Keys.Contains(words[firstWordNotModifier+1]))
                            if (GetTypeFromRaw(words[firstWordNotModifier+1]) == KeywordType.Normal)
                                if (IsAssignator(words[firstWordNotModifier+2]))
                                    if (GetTypeFromRaw(words[firstWordNotModifier + 3]) == KeywordType.Value || GetTypeFromRaw(words[firstWordNotModifier+3]) == KeywordType.Subject || Variables.ContainsKey(words[firstWordNotModifier+3]) || IsProperty(words[firstWordNotModifier+3]))
                                        Variables[words[firstWordNotModifier + 1]] = words[firstWordNotModifier + 3];
            }
        }
        public bool IsVariable(string word)
        {
            return Variables.ContainsKey(word);
        }
        public bool IsProperty(string word)
        {
            if (word.Contains('.'))
            {
                var split = word.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                if (split.Length == 2 && IsVariable(split[0]))
                    return IsValidProperty(split[1]);
            }
            return false;
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
    }
}
