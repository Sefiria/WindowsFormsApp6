using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using static Script.StructuresAndCells;

namespace Script
{
    [Serializable]
    public class InputScript
    {
        static _Main Global = _Main.Instance;

        public class ScriptEntitySubject
        {
            public int X, Y;
            public int ID;
            public float Tension;

            public ScriptEntitySubject(
                int _X,
                int _Y,
                int _ID = 0,
                float _Tension = 0F)
            {
                X = _X; Y = _Y;
                ID = _ID;
                Tension = _Tension;
            }

            public static ScriptEntitySubject Empty = new ScriptEntitySubject(0, 0);

            public string GetProperty(string PropertyName)
            {
                switch(PropertyName)
                {
                    default: return "";
                    case "X": return X.ToString();
                    case "Y": return Y.ToString();
                    case "ID": return ID.ToString();
                    case "Tension": return Tension.ToString();
                }
            }
        }

        public enum KeywordType
        {
            Normal,
            Function,
            Condition,
            Subject,
            Operator,
            Assignator,
            Comparator,
            Value,
            LogicalOperator,
            Modifier,
            Variable
        }

        public Cell cell = null;
        public int Counter1 = 0, Counter2 = 0, Counter3 = 0;

        static public List<string> Functions => GetFunctions().Keys.ToList();
        static public List<(string[] args, string Description)> FunctionsArgumentsDescription => GetFunctions().Values.ToList();
        static public List<string> Conditions => GetConditions().Keys.ToList();
        static public List<string> ConditionsDescription => GetConditions().Values.ToList();
        static public List<string> Subjects => GetSubjects().Keys.ToList();
        static public List<string> SubjectsDescription => GetSubjects().Values.ToList();
        static public List<string> Operators => GetOperators().Keys.ToList();
        static public List<string> OperatorsDescription => GetOperators().Values.ToList();
        static public List<string> Assignators => GetAssignators().Keys.ToList();
        static public List<string> AssignatorsDescription => GetAssignators().Values.ToList();
        static public List<string> Comparators => GetComparators().Keys.ToList();
        static public List<string> ComparatorsDescription => GetComparators().Values.ToList();
        static public List<string> LogicalOperators => GetLogicalOperators().Keys.ToList();
        static public List<string> LogicalOperatorsDescription => GetLogicalOperators().Values.ToList();
        static public List<string> Modifiers => GetModifiers().Keys.ToList();
        static public List<string> ModifiersDescription => GetModifiers().Values.ToList();
        static public Dictionary<string, (string[] args, string Description)> GetFunctions()
        {
            var result = new Dictionary<string, (string[] args, string Description)>();
            result.Add("GetTension", (new string[] {  }, "Get the current tension of a target subject"));
            result.Add("SetTension", (new string[] { "Value:Tension" }, "Set the tension of a target subject"));
            result.Add("AddTension", (new string[] { "Value:Tension" }, "Add tension of a target subject"));
            result.Add("SubstractTension", (new string[] { "Value:Tension" }, "Substract tension of a target subject"));
            result.Add("Set", (new string[] { "Variable:var", "Value:val" }, "Set variable value"));
            result.Add("SetExecutionRate", (new string[] { "Value:val" }, "In MS"));
            return result;
        }
        static public Dictionary<string, string> GetConditions()
        {
            var result = new Dictionary<string, string>();
            result.Add("if", "");
            result.Add("ifnot", "");
            result.Add("then", "");
            result.Add("else", "");
            return result;
        }
        static public Dictionary<string, string> GetSubjects()
        {
            var result = new Dictionary<string, string>();
            result.Add("self", "Target oneself");
            result.Add("up", "Target up cell");
            result.Add("down", "Target down cell");
            result.Add("left", "Target left cell");
            result.Add("right", "Target right cell");
            result.Add("random", "Target random cell around (up/down/left/right)");
            return result;
        }
        static public Dictionary<string, string> GetOperators()
        {
            var result = new Dictionary<string, string>();
            result.Add("+", "Add");
            result.Add("-", "Substract");
            result.Add("*", "Multiply");
            result.Add("/", "Divide");
            result.Add("++", "Increment by 1");
            result.Add("--", "Decrement by 1");
            return result;
        }
        static public Dictionary<string, string> GetAssignators()
        {
            var result = new Dictionary<string, string>();
            result.Add("=", "Equal (assignation)");
            result.Add("+=", "Add (assignation)");
            result.Add("-=", "Substract (assignation)");
            return result;
        }
        static public Dictionary<string, string> GetComparators()
        {
            var result = new Dictionary<string, string>();
            result.Add("==", "Is Equal");
            result.Add("!=", "Is Not Equal");
            result.Add(">", "Is Superior");
            result.Add(">=", "Is Superior or Equal");
            result.Add("<", "Is Inferior");
            result.Add("<=", "Is Inferior or Equal");
            result.Add("><", "Is Null");
            return result;
        }
        static public Dictionary<string, string> GetLogicalOperators()
        {
            var result = new Dictionary<string, string>();
            result.Add("and", "Logical operator 'and'");
            result.Add("or", "Logical operator 'or'");
            return result;
        }
        static public Dictionary<string, string> GetModifiers()
        {
            var result = new Dictionary<string, string>();
            result.Add("keep", "Reusable variable, kept while all frames");
            result.Add("share", "Share variable to others BehaviorScripts");
            return result;
        }
        static public bool IsFunction(string word)
        {
            return GetFunctions().Keys.Contains(word);
        }
        static public bool IsCondition(string word)
        {
            return GetConditions().Keys.Contains(word);
        }
        static public bool IsSubject(string word)
        {
            return GetSubjects().Keys.Contains(word);
        }
        static public bool IsOperator(string word)
        {
            return GetOperators().Keys.Contains(word);
        }
        static public bool IsAssignator(string word)
        {
            return GetAssignators().Keys.Contains(word);
        }
        static public bool IsComparator(string word)
        {
            return GetComparators().Keys.Contains(word);
        }
        static public bool IsLogicalOperator(string word)
        {
            return GetLogicalOperators().Keys.Contains(word);
        }
        static public bool IsModifier(string word)
        {
            return GetModifiers().Keys.Contains(word);
        }


        public string script;

        public InputScript()
        {
            script = "";
        }
        public InputScript(InputScript inputScript)
        {
            script = inputScript.script;
            cell = inputScript.cell;
            Counter1 = inputScript.Counter1;
            Counter2 = inputScript.Counter2;
            Counter3 = inputScript.Counter3;
        }

        static public List<string> GetProperties()
        {
            return new string[]
            {
                "X",
                "Y",
                "Layer",
                "ID",
                "Name",
                "VelocityX",
                "VelocityY",
                "ScriptEnabled",
                "IsCharged"
            }.ToList();
        }
        static public bool IsValidProperty(string property)
        {
            return GetProperties().Contains(property);
        }
        static public List<string> GetPublicVariables()
        {
            return new string[]
            {
                "Self",
                "Counter1",
                "Counter2",
                "Counter3",
            }.ToList();
        }
        static public bool IsPublicVariables(string word)
        {
            return GetPublicVariables().Contains(word.Replace(" ", ""));
        }
        static public List<string> GetListNonErrorsWords()
        {
            return new string[]
            {
                "var"
            }.ToList();
        }
        static public KeywordType GetTypeFromRaw(string raw, Dictionary<string, string> vars = null)
        {
            if(vars != null)
                if (vars.ContainsKey(raw))
                    return KeywordType.Variable;

            if (IsFunction(raw))
                return KeywordType.Function;

            if (IsCondition(raw))
                return KeywordType.Condition;

            if (IsSubject(raw))
                return KeywordType.Subject;

            if (IsOperator(raw))
                return KeywordType.Operator;

            if (IsAssignator(raw))
                return KeywordType.Assignator;

            if (IsComparator(raw))
                return KeywordType.Comparator;

            if (IsLogicalOperator(raw))
                return KeywordType.LogicalOperator;

            if (IsModifier(raw))
                return KeywordType.Modifier;

            if (int.TryParse(raw, out int unused))
                return KeywordType.Value;

            return KeywordType.Normal;
        }

        #region ScriptExecution

        Dictionary<string, string> Variables = new Dictionary<string, string>();
        bool ScriptError = false;
        public int ExecutionRate = 500;

        public bool IsVariable(string word)
        {
            return Variables.ContainsKey(word);
        }
        public string GetVariableValue(string word)
        {
            return IsVariable(word) ? Variables[word] : "0";
        }
        public object GetScriptEntitySubjectFromSubject(string SUBJ)
        {
            if (IsPublicVariables(SUBJ))
            {
                switch (SUBJ)
                {
                    case "Self": return cell;
                    case "Counter1": return Counter1;
                    case "Counter2": return Counter2;
                    case "Counter3": return Counter3;
                }
            }
            else if (IsVariable(SUBJ))
                return null;
            else if (IsSubject(SUBJ))
            {
                /*
                    result.Add("self", "Target oneself");
                    result.Add("up", "Target up cell");
                    result.Add("down", "Target down cell");
                    result.Add("left", "Target left cell");
                    result.Add("right", "Target right cell");
                    result.Add("random", "Target random cell around (up/down/left/right)");
                */
                //Cell tcell;
                Point point;
                switch (SUBJ)
                {
                    case "self":
                        return cell;// new ScriptEntitySubject(cell.X, cell.Y, cell.Id, cell.Tension);

                    case "up":
                        point = new Point(cell.X, cell.Y - 1);
                        if (!Global.MapBounds.Contains(point))
                            return new Cell(-1, -1);//ScriptEntitySubject.Empty;
                        //tcell = Global.map[point.X][point.Y];
                        return Global.map[point.X][point.Y];//new ScriptEntitySubject(tcell.X, tcell.Y, tcell.Id, tcell.Tension);

                    case "down":
                        point = new Point(cell.X, cell.Y + 1);
                        if (!Global.MapBounds.Contains(point))
                            return new Cell(-1, -1);//ScriptEntitySubject.Empty;
                        //tcell = Global.map[point.X][point.Y];
                        return Global.map[point.X][point.Y];//new ScriptEntitySubject(tcell.X, tcell.Y, tcell.Id, tcell.Tension);

                    case "left":
                        point = new Point(cell.X - 1, cell.Y);
                        if (!Global.MapBounds.Contains(point))
                            return new Cell(-1, -1);//ScriptEntitySubject.Empty;
                        //tcell = Global.map[point.X][point.Y];
                        return Global.map[point.X][point.Y];//new ScriptEntitySubject(tcell.X, tcell.Y, tcell.Id, tcell.Tension);

                    case "right":
                        point = new Point(cell.X + 1, cell.Y);
                        if (!Global.MapBounds.Contains(point))
                            return new Cell(-1, -1);//ScriptEntitySubject.Empty;
                        //tcell = Global.map[point.X][point.Y];
                        return Global.map[point.X][point.Y];//new ScriptEntitySubject(tcell.X, tcell.Y, tcell.Id, tcell.Tension);

                    case "random":
                        Random rnd = new Random((int)DateTime.Now.Ticks);
                        point = new Point(cell.X + rnd.Next(-1, 2), cell.Y + rnd.Next(-1, 2));
                        if (!Global.MapBounds.Contains(point))
                            return new Cell(-1, -1);//ScriptEntitySubject.Empty;
                        //tcell = Global.map[point.X][point.Y];
                        return Global.map[point.X][point.Y];//new ScriptEntitySubject(tcell.X, tcell.Y, tcell.Id, tcell.Tension);
                }
            }
            return null;
        }

        public void ScriptMain(Cell cell)
        {
            this.cell = cell;

            if (!ScriptError)
            {
                Variables = new Dictionary<string, string>();

                try
                {
                    var lines = script.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    if (lines.Length == 0)
                        return;
                    string line = "";
                    string[] words;
                    List<(int Line, string Message)> Errors = new List<(int Line, string Message)>();

                    for (int y = 0; y < lines.Length; y++)
                    {
                        (bool Success, string Error) result = (true, "");

                        line = lines[y];
                        words = line.Split(new char[] { ' ' }, StringSplitOptions.None);

                        int firstWordNotModifier = 0;
                        for (int w = 0; w < words.Length; w++)
                            if (IsModifier(words[w])) firstWordNotModifier++;
                            else break;

                        if (GetTypeFromRaw(words[firstWordNotModifier]) == KeywordType.Function)
                        {
                            var FUNC = words[firstWordNotModifier];
                            var SUBJ = words[firstWordNotModifier + 1];
                            var ARGS = GetExpression(words, firstWordNotModifier + 2, "@END").Expression.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                            result = Function(FUNC, SUBJ, ARGS);
                        }
                        else
                        {
                            switch (words[firstWordNotModifier])
                            {
                                case "if":
                                    //TestCondition = TestStructure(words, "if", "@VAL@", "$Comparator", "@VAL@", "then", "$Function", "$Subject", "@FUNCARGS");
                                    string EXP1, EXP2, COMP, FUNC, SUBJ;
                                    string[] ARGS;
                                    (string Expression, int NextIndex) Expression;

                                    Expression = GetExpression(words, firstWordNotModifier + 1, "$Comparator");
                                    EXP1 = Expression.Expression;
                                    COMP = words[Expression.NextIndex++];
                                    Expression = GetExpression(words, Expression.NextIndex, "then");
                                    EXP2 = Expression.Expression;
                                    FUNC = words[Expression.NextIndex + 1];
                                    SUBJ = words[Expression.NextIndex + 2];
                                    ARGS = GetExpression(words, Expression.NextIndex + 3, "@END").Expression.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                                    result = Condition(EXP1, EXP2, COMP, FUNC, SUBJ, ARGS);
                                    break;
                                case "var":
                                    AddVariable(words, firstWordNotModifier);
                                    break;
                                default:
                                    ModifyGlobalVariable(words, firstWordNotModifier);
                                    break;
                            }
                        }

                        if (!result.Success)
                            Errors.Add((y, result.Error));
                    }

                    if (Errors.Count > 0)
                    {
                        string message = "";
                        foreach (var error in Errors)
                            message += "\t- Error Line " + error.Line + " :\n" + error.Message + "\n";
                        throw new Exception(message);
                    }
                }
                catch (Exception e)
                {
                    ScriptError = true;
                    MessageBox.Show("The InputScript has some error(s) (#ID : " + "@NA@" + ") :\n\n" + e);
                }
            }
        }

        private (bool Success, string Error) ModifyGlobalVariable(string[] words, int firstWordNotModifier)
        {
            try
            {
                string word1 = words[firstWordNotModifier];
                string word2 = words[firstWordNotModifier + 1];
                if (IsPublicVariables(word1))
                {
                    if (IsOperator(word2))
                    {
                        switch (words[firstWordNotModifier + 1])
                        {
                            case "++": SetPublicVariableValue(word1, GetPublicVariableValue(word1) + 1); break;
                            case "--": SetPublicVariableValue(word1, GetPublicVariableValue(word1) - 1); break;
                        }
                    }
                    else if (IsAssignator(word2))
                    {
                        switch (word2)
                        {
                            case "=":
                                SetPublicVariableValue(word1, int.Parse(ComputeExpression(GetExpression(words, firstWordNotModifier + 2, "@END").Expression)));
                                break;
                            case "+=":
                                SetPublicVariableValue(word1, GetPublicVariableValue(word1) + int.Parse(ComputeExpression(GetExpression(words, firstWordNotModifier + 2, "@END").Expression)));
                                break;
                            case "-=":
                                SetPublicVariableValue(word1, GetPublicVariableValue(word1) - int.Parse(ComputeExpression(GetExpression(words, firstWordNotModifier + 2, "@END").Expression)));
                                break;
                        }
                    }
                }

                return (true, "");
            }
            catch (Exception e)
            {
                return (false, $"[ModifyGlobalVariable] : {e.Message}");
            }
        }

        private (string Expression, int NextIndex) GetExpression(string[] words, int StartIndex, string EndToken)
        {
            (string Expression, int NextIndex) result = ("", 0);
            for (int w = StartIndex; w < words.Length; w++)
            {
                if (EndToken[0] == '$')
                {
                    if (GetTypeFromRaw(words[w]) != (KeywordType)Enum.Parse(typeof(KeywordType), string.Concat(EndToken.Skip(1))))
                    {
                        result.Expression += GetBindedWord(words[w]) + " ";
                        //result.Expression += words[w] + " ";
                        continue;
                    }
                }
                else if (EndToken[0] == '@')
                {
                    if (EndToken == "@END")
                    {
                        if(w == words.Length - 3)
                            result.Expression += words[w] + " ";
                        else
                            result.Expression += GetBindedWord(words[w]) + " ";
                        //result.Expression += words[w] + " ";
                        continue;
                    }
                }
                else
                {
                    if (words[w] != EndToken)
                    {
                        result.Expression += GetBindedWord(words[w]) + " ";
                        //result.Expression += words[w] + " ";
                        continue;
                    }
                }

                result.NextIndex = w;
                return result;
            }

            return result;
        }
        private string ComputeExpression(string EXP)
        {
            return new System.Data.DataTable().Compute(EXP, "").ToString();
        }
        private (bool Success, string Error) Function(string FUNC, string SUBJ, string[] ARGS)
        {
            /*
                "GetTension",                     "Get the current tension of a target subject"
                "SetTension",   "Value:Tension",  "Set the tension of a target subject"
            */

            var target = GetScriptEntitySubjectFromSubject(SUBJ);
            if (target == null)
                return (false, "The target subject '" + SUBJ + "' doesn't exist.");
            if (target == new Cell(-1, -1))// ScriptEntitySubject.Empty)
                return (true, "");

            switch (FUNC)
            {
                case "GetTension":
                    break;

                case "SetTension":
                    float Tension = 0F;
                    if (!float.TryParse(ARGS[0], out Tension))
                        return (false, "The argument 1 '" + ARGS[0] + "' should be a number.");
                    if(Tension < 0F || Tension > 1F)
                        return (false, "The argument 1 '" + ARGS[0] + "' should be between 0F and 1F.");
                    if (!(target is Cell))
                        return (false, "The target should be a Cell.");
                    (target as Cell).Tension = Tension;
                    break;

                case "Set":
                    string variable = ARGS[0];
                    string value = ARGS[2];
                    if (!IsVariable(variable))
                        return (false, "The argument 1 '" + variable + "' should be a variable.");
                    if (!int.TryParse(value, out int unused))
                        return (false, "The argument 3 '" + value + "' should be a variable or a number.");
                    Variables[variable] = value;
                    break;

                case "SetExecutionRate":
                    int rate = ExecutionRate;
                    if (!int.TryParse(ARGS[0], out rate))
                        return (false, "The argument 1 '" + ARGS[0] + "' should be a number (in ms).");
                    ExecutionRate = rate;
                    break;
            }

            return (true, "");
        }
        private (bool Success, string Error) Condition(string EXP1, string EXP2, string COMP, string FUNC, string SUBJ, string[] ARGS)
        {
            int VAL1 = 0, VAL2 = 0;
            bool VAL1_ISINT = int.TryParse(EXP1, out VAL1);
            if (!VAL1_ISINT && IsPublicVariables(EXP1))
            {
                object obj = GetScriptEntitySubjectFromSubject(EXP1.Replace(" ", ""));
                if (obj is int)
                {
                    VAL1 = (int)GetScriptEntitySubjectFromSubject(EXP1.Replace(" ", ""));
                    VAL1_ISINT = true;
                }
            }
            try
            {
                if (!VAL1_ISINT)
                    VAL1 = (int)new System.Data.DataTable().Compute(EXP1, "");
                bool VAL2_ISINT = int.TryParse(EXP2, out VAL2);
                if (!VAL2_ISINT)
                    VAL2 = (int)new System.Data.DataTable().Compute(EXP2, "");
            }
            catch (Exception e)
            {
                throw new Exception("Script Error : One of the both expression in the condition was not an integer");
            }

            bool RSLT = false;
            switch(COMP)
            {
                case "==": RSLT = VAL1 == VAL2; break;
                case "!=": RSLT = VAL1 != VAL2; break;
                case "<": RSLT = VAL1 < VAL2; break;
                case ">": RSLT = VAL1 > VAL2; break;
                case "<=": RSLT = VAL1 <= VAL2; break;
                case ">=": RSLT = VAL1 >= VAL2; break;
            }

            if (!RSLT)
                return (true, "");

            if (IsFunction(FUNC))
            {
                return Function(FUNC, SUBJ, ARGS);
            }
            else
            {
                string[] array1 = new string[] { FUNC, SUBJ };
                string[] array2 = ARGS;
                string[] words = new string[array1.Length + array2.Length];
                Array.Copy(array1, words, array1.Length);
                Array.Copy(array2, 0, words, array1.Length, array2.Length);
                return ModifyGlobalVariable(words, 0);
            }

        }
        private void AddVariable(string[] words, int firstWordNotModifier)
        {
            if (!Variables.ContainsKey(words[firstWordNotModifier + 1]))
                Variables[words[firstWordNotModifier + 1]] = ComputeExpression(GetExpression(words, firstWordNotModifier + 3, "@END").Expression);
        }
        private string GetBindedWord(string word)
        {
            if (int.TryParse(word, out int unused))
                return word;

            if (IsVariable(word))
                return GetVariableValue(word);

            if (word.Split('.').Length == 2)
                if (IsPublicVariables(word.Split('.')[0]))
                    if (IsValidProperty(word.Split('.')[1]))
                    //return GetScriptEntitySubjectFromSubject(word.Split('.')[0]).GetProperty(word.Split('.')[1]);
                    {
                        object entity = GetScriptEntitySubjectFromSubject(word.Split('.')[0]);
                        if (entity is Cell)
                            return GetSubjectProperty(GetScriptEntitySubjectFromSubject(word.Split('.')[0]) as Cell, word.Split('.')[1]);
                        if (entity is int)
                            return entity.ToString();
                        return null;
                    }
            return word;
        }

        private string GetSubjectProperty(Cell cell, string propertyName)
        {
            return typeof(Cell).GetProperty(propertyName).GetValue(cell).ToString();
        }

        private void SetPublicVariableValue(string variable, int value)
        {
            switch(variable)
            {
                case "Counter1": Counter1 = value; break;
                case "Counter2": Counter2 = value; break;
                case "Counter3": Counter3 = value; break;
            }
        }
        private int GetPublicVariableValue(string variable)
        {
            switch (variable)
            {
                default: return -1;
                case "Counter1": return Counter1;
                case "Counter2": return Counter2;
                case "Counter3": return Counter3;
            }
        }

        #endregion
    }
}
