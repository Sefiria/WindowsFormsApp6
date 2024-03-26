using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Framework
{
    [Serializable]
    public class BehaviorScript
    {
        public class ScriptEntitySubject
        {
            public int X, Y, VelocityX, VelocityY;
            public int Layer;
            public int ID;
            public string Name;
            public bool ScriptEnabledpublic;
            public bool Charged;

            public Action<int> SetX, SetY, SetVelocityX, SetVelocityY;
            public Action Destroy;
            public Action<int> Charge;

            public ScriptEntitySubject(
                int _X,
                int _Y,
                int _VelocityX,
                int _VelocityY,
                Action<int> _SetX, Action<int> _SetY, Action<int> _SetVelocityX, Action<int> _SetVelocityY,
                Action _Destroy,
                Action<int> _Charge,
                bool _Charged = false,
                int _Layer = 0,
                int _ID = 0,
                string _Name = "",
                bool _ScriptEnabledpublic = false)
            {
                X = _X; Y = _Y; VelocityX = _VelocityX; VelocityY = _VelocityY;
                SetX = _SetX; SetY = _SetY; SetVelocityX = _SetVelocityX; SetVelocityY = _SetVelocityY;
                Destroy = _Destroy;
                Charge = _Charge;
                Charged = _Charged;
                Layer = _Layer;
                ID = _ID;
                Name = _Name;
                ScriptEnabledpublic = _ScriptEnabledpublic;
            }

            public static ScriptEntitySubject Empty = new ScriptEntitySubject(0, 0, 0, 0, (n) => { }, (n) => { }, (n) => { }, (n) => { }, () => { }, (n) => { });

            public string GetProperty(string PropertyName)
            {
                switch(PropertyName)
                {
                    default: return "";
                    case "X": return X.ToString();
                    case "Y": return Y.ToString();
                    case "Layer": return Layer.ToString();
                    case "ID": return ID.ToString();
                    case "Name": return Name;
                    case "ScriptEnabledpublic": return ScriptEnabledpublic ? "1" : "0";
                    case "IsCharged": return Charged ? "1" : "0";
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

        public EntityProperties Entity = null;

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
            result.Add("move", (new string[] { "Value:X", "Value:Y" }, "Move the subject S to X and Y pixels"));
            result.Add("position", (new string[] { "Value:X", "Value:Y" }, "Set the position of the subject S as (X;Y)"));
            result.Add("shot", (new string[0], "Shot subject S with bullet B on direction (X, Y)"));
            result.Add("shottarget", (new string[] { "Subject:Target" }, "Shot subject S with bullet B on direction of target T"));
            result.Add("jump", (new string[0], "Jump subject S"));
            result.Add("wait", (new string[] { "Value:Milliseconds" }, "Wait N milliseconds"));
            result.Add("waitseconds", (new string[] { "Value:Seconds" }, "Wait N seconds"));
            result.Add("waitminutes", (new string[] { "Value:Minutes" }, "Wait N minutes"));
            result.Add("destroy", (new string[0], "Destroy the target"));
            result.Add("charge", (new string[] { "Value:Enabled" }, "Charge or not"));
            result.Add("set", (new string[] { "Variable:Var", "Assignator:Ass", "Value:Val" }, "Set variable value"));
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
            result.Add("closest", "Target closest subject");
            result.Add("farest", "Target farest subject");
            result.Add("random", "Target random subject excluding oneself");
            result.Add("randomself", "Target random subject including oneself");
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
        Level level;
        EntityProperties entity;
        int X, Y;

        public BehaviorScript()
        {
            script = "";
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
                "Camera",
                "Player",
                "Self"
            }.ToList();
        }
        static public bool IsPublicVariables(string word)
        {
            return GetPublicVariables().Contains(word);
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

        public bool IsVariable(string word)
        {
            return Variables.ContainsKey(word);
        }
        public string GetVariableValue(string word)
        {
            return IsVariable(word) ? Variables[word] : "0";
        }
        public ScriptEntitySubject GetScriptEntitySubjectFromSubject(string SUBJ)
        {
            if (IsPublicVariables(SUBJ))
            {
                if (SUBJ == "Camera")
                    return new ScriptEntitySubject( level.InGameCamera.X, level.InGameCamera.Y, 0, 0,
                                                    (value) => level.InGameCamera.X = value, (value) => level.InGameCamera.Y = value, (value) => { }, (value) => { },
                                                    () => { },
                                                    (n) => { },
                                                    false,
                                                    entity.layer, entity.ID, entity.Name, false);
                if (SUBJ == "Player")
                {
                    if (level.player == null)
                        return ScriptEntitySubject.Empty;
                    else
                        return new ScriptEntitySubject( (int)(level.player.position.X / Tools.TileSize), (int)(level.player.position.Y / Tools.TileSize), 0, 0,
                                                        (value) => level.player.position.X = value * Tools.TileSize, (value) => level.player.position.Y = value * Tools.TileSize, (value) => { }, (value) => { },
                                                        () => { level.player.HP = 0; },
                                                        (n) => { },
                                                        false,
                                                        entity.layer, entity.ID, entity.Name, false);
                }
                if (SUBJ == "Self")
                {
                    return new ScriptEntitySubject(X, Y, 0, 0,
                                                    (value) => X = value, (value) => Y = value, (value) => { }, (value) => { },
                                                    () => { level.DeleteTile(entity.layer, new Point(X, Y), false); },
                                                    (n) => level.Charge(X, Y, n == 0 ? false : true, true, true),
                                                    level.ChargeCells[X, Y] == 1,
                                                    entity.layer, entity.ID, entity.Name, false);
                }
            }
            else if (IsVariable(SUBJ))
                return null;
            else if (IsSubject(SUBJ))
            {
                /*
                    "self",         "Target oneself"
                    "closest",      "Target closest subject"
                    "farest",       "Target farest subject"
                    "random",       "Target random subject excluding oneself"
                    "randomself",   "Target random subject including oneself"
                */
                switch(SUBJ)
                {
                    case "self":
                        return new ScriptEntitySubject( X, Y, 0, 0,
                                                        (value) => X = value * Tools.TileSize, (value) => Y = value * Tools.TileSize, (value) => { }, (value) => { },
                                                        () => { level.DeleteTile(entity.layer, new Point(X, Y), false); },
                                                        (n) => level.Charge(X, Y, n == 0 ? false : true, true, true),
                                                        level.ChargeCells[X, Y] == 1,
                                                        entity.layer, entity.ID, entity.Name, false);
                    case "closest":
                        break;
                    case "farest":
                        break;
                    case "random":
                        break;
                    case "randomself":
                        break;
                }
            }

            return null;
        }

        public void ScriptMain(Level _level, EntityProperties _entity, int _X, int _Y)
        {
            level = _level;
            entity = _entity;
            X = _X;
            Y = _Y;

            if (!ScriptError)
            {
                Variables = new Dictionary<string, string>();
                Entity = entity;

                while (Thread.CurrentThread.Name != "Abort")
                {
                    if (!level.IsPositionVisible(X, Y))
                    {
                        Thread.CurrentThread.Name = "Abort";
                        return;
                    }

                    try
                    {
                        var lines = script.Split(new char[] { '\n' }, StringSplitOptions.None);
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
                        MessageBox.Show("The BehaviorScript has some error(s) (#ID : " + entity.ID + ") :\n\n" + e);
                    }
                }
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
                "move",         "Value:X", "Value:Y",               "Move the subject S to X and Y pixels"
                "position",     "Value:X", "Value:Y",               "Set the position of the subject S as (X;Y)"
                "shot",                                             "Shot subject S with bullet B on direction (X, Y)"
                "shottarget",   "Subject:Target",                   "Shot subject S with bullet B on direction of target T"
                "jump",                                             "Jump subject S"
                "wait",         "Value:Milliseconds",               "Wait N milliseconds"
                "waitseconds",  "Value:Seconds",                    "Wait N seconds"
                "waitminutes",  "Value:Minutes",                    "Wait N minutes"
                "destroy",                                          "Destroy the target"
                "charge",       "Value:Enabled"                     "Charge or not"
                "set",          "Value:Variable", "Value:Value"     "Set variable value"
            */

            var target = GetScriptEntitySubjectFromSubject(SUBJ);
            if (target == null)
                return (false, "The target subject '" + SUBJ + "' doesn't exist.");
            if (target == ScriptEntitySubject.Empty)
                return (true, "");

            int arg0 = 0, arg1 = 0;
            switch (FUNC)
            {
                case "move":
                    {
                        if (!int.TryParse(ARGS[0], out arg0))
                            return (false, "The argument 1 '" + ARGS[0] + "' should be a number.");
                        if (!int.TryParse(ARGS[1], out arg1))
                            return (false, "The argument 2 '" + ARGS[1] + "' should be a number.");
                        target.SetX(target.X + arg0);
                        target.SetY(target.Y + arg1);
                    }
                    break;

                case "position":
                    {
                        if (!int.TryParse(ARGS[0], out arg0))
                            return (false, "The argument 1 '" + ARGS[0] + "' should be a number.");
                        if (!int.TryParse(ARGS[1], out arg1))
                            return (false, "The argument 2 '" + ARGS[1] + "' should be a number.");
                        target.SetX(arg0);
                        target.SetY(arg1);
                    }
                    break;

                case "shot":
                    break;

                case "shottarget":
                    break;

                case "jump":
                    break;

                case "wait":
                    {
                        if (!int.TryParse(ARGS[0], out arg0))
                            return (false, "The argument 1 '" + ARGS[0] + "' should be a number.");
                        Thread.Sleep(arg0);
                    }
                    break;

                case "waitseconds":
                    {
                        if (!int.TryParse(ARGS[0], out arg0))
                            return (false, "The argument 1 '" + ARGS[0] + "' should be a number.");
                        Thread.Sleep(arg0 * 1000);
                    }
                    break;

                case "waitminutes":
                    {
                        if (!int.TryParse(ARGS[0], out arg0))
                            return (false, "The argument 1 '" + ARGS[0] + "' should be a number.");
                        Thread.Sleep(arg0 * 1000 * 60);
                    }
                    break;

                case "destroy":
                    target.Destroy();
                    break;

                case "charge":
                    if (!int.TryParse(ARGS[0], out arg0))
                        return (false, "The argument 1 '" + ARGS[0] + "' should be a number.");
                    if (arg0 != 0 && arg0 != 1)
                        return (false, "The argument 1 '" + ARGS[0] + "' and 2 '" + ARGS[2] + "' can only be 0 or 1.");
                    target.Charge(arg0);
                    break;

                case "set":
                    string variable = ARGS[0];
                    string value = ARGS[2];
                    if (!IsVariable(variable))
                        return (false, "The argument 1 '" + variable + "' should be a variable.");
                    /*if (!IsVariable(value))
                        value = GetVariableValue(value);
                    else*/
                        if (!int.TryParse(value, out arg0))
                            return (false, "The argument 3 '" + value + "' should be a variable or a number.");
                    Variables[variable] = value;
                    break;
            }

            return (true, "");
        }
        private (bool Success, string Error) Condition(string EXP1, string EXP2, string COMP, string FUNC, string SUBJ, string[] ARGS)
        {
            int VAL1 = 0, VAL2 = 0;
            bool VAL1_ISINT = int.TryParse(EXP1, out VAL1);
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
            }

            if (!RSLT)
                return (true, "");

            return Function(FUNC, SUBJ, ARGS);
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
                        return GetScriptEntitySubjectFromSubject(word.Split('.')[0]).GetProperty(word.Split('.')[1]);

            return word;
        }

        #endregion
    }
}
