using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Script
{
    [Serializable]
    public class InputScript
    {
        /// <summary>
        /// </summary>
        /// <typeparam name="T">'Action' of 'Function which return something'</typeparam>
        public class Verb<T>
        {
            public string Syntax;
            public string Description;
            public T Method;
            public string[] Arguments;

            public Verb(string Syntax, T Method, string Description = "", string[] Arguments = null)
            {
                this.Syntax = Syntax;
                this.Method = Method;
                this.Description = Description;
                this.Arguments = Arguments;
            }
        }
        /// <summary>
        /// Default Template of Verb, that is Action (no arg, no return)
        /// </summary>
        public class Verb : Verb<DMethod> { public Verb(string Syntax, DMethod Method, string Description = "", string[] Arguments = null) : base(Syntax, Method, Description, Arguments) {} }
        public class PVarType
        {
            public Dictionary<string, int> Properties;
            public PVarType(Dictionary<string, int> Properties = null)
            {
                this.Properties = Properties ?? new Dictionary<string, int>();
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


        #region Core
        public delegate void DMethod(object[] args);
        public delegate (bool Success, string Error) DFunction(object[] args);
        public delegate bool DComparator(object[] args);

        static public Dictionary<string, PVarType> PublicVariables = new Dictionary<string, PVarType>()
        {
            ["A"] = null,
            ["B"] = null,
            ["C"] = null,
            ["D"] = null,
            ["E"] = null,
            ["F"] = null
        };
        static public List<Verb> VerbsConditions = new List<Verb>()
        {
            new Verb("if", (x) => {}),
            new Verb("ifnot", (x) => {}),
            new Verb("then", (x) => {}),
            new Verb("else", (x) => {})
        };
        static public List<Verb<Func<object>>> VerbsSubjects = new List<Verb<Func<object>>>()
        {
            new Verb<Func<object>>("self", () => null)
        };
        static public List<Verb> VerbsOperators = new List<Verb>()
        {
            new Verb("+", (x) => {}, "Add"),
            new Verb("-", (x) => {}, "Substract"),
            new Verb("*", (x) => {}, "Multiply"),
            new Verb("/", (x) => {}, "Divide"),
            new Verb("++", (x) => {if(GetEntity((string)x[0]) is int) {object v = GetEntity((string)x[0]); v = (int)v + 1; }}, "Increment by 1"),
            new Verb("--", (x) => {if(GetEntity((string)x[0]) is int) {object v = GetEntity((string)x[0]); v = (int)v - 1; }}, "Decrement by 1")
        };
        static public List<Verb> VerbsAssignators = new List<Verb>()
        {
            new Verb("=", (x) =>
            {
                string[] words = (string[])x[0];
                int firstWordNotModifier = (int)x[1];
                string word1 = words[firstWordNotModifier];
                object v = GetEntity(word1);
                if(v is int)
                {
                    v = int.Parse(ComputeExpression(GetExpression(words, firstWordNotModifier + 2, "@END").Expression));
                }
                else
                {
                    (object Entity, int prevVal, int val) = AssignatorsGetEntityAnValue(words, firstWordNotModifier);
                    (Verb<Func<object>> Subject, string Property)? SubjectProp = Entity as (Verb<Func<object>>, string)?;
                    if(SubjectProp != null)
                        SelfType.GetProperty(SubjectProp.Value.Property).SetValue(SubjectProp.Value.Subject.Method(), val);
                    else
                        Entity = val;
                }
            }, "Equal"),
            new Verb("+=", (x) =>
            {
                string[] words = (string[])x[0];
                int firstWordNotModifier = (int)x[1];
                    (object Entity, int prevVal, int val) = AssignatorsGetEntityAnValue(words, firstWordNotModifier);
                    (Verb<Func<object>> Subject, string Property)? SubjectProp = Entity as (Verb<Func<object>>, string)?;
                    if(SubjectProp != null)
                        SelfType.GetProperty(SubjectProp.Value.Property).SetValue(SubjectProp.Value.Subject.Method(), prevVal + val);
                    else
                        Entity = prevVal + val;
            }, "Add"),
            new Verb("-=", (x) =>
            {
                string[] words = (string[])x[0];
                int firstWordNotModifier = (int)x[1];
                    (object Entity, int prevVal, int val) = AssignatorsGetEntityAnValue(words, firstWordNotModifier);
                    (Verb<Func<object>> Subject, string Property)? SubjectProp = Entity as (Verb<Func<object>>, string)?;
                    if(SubjectProp != null)
                        SelfType.GetProperty(SubjectProp.Value.Property).SetValue(SubjectProp.Value.Subject.Method(), prevVal - val);
                    else
                        Entity = prevVal - val;
            }, "Substract")
        };
        static public List<Verb<DComparator>> VerbsComparators = new List<Verb<DComparator>>()
        {
            new Verb<DComparator>("==", (x) => (int)x[0] == (int)x[1], "Is equal"),
            new Verb<DComparator>("!=", (x) => (int)x[0] != (int)x[1], "Is not equal"),
            new Verb<DComparator>(">", (x) => (int)x[0] > (int)x[1], "Is superior"),
            new Verb<DComparator>(">=", (x) => (int)x[0] >= (int)x[1], "Is superior or equal"),
            new Verb<DComparator>("<", (x) => (int)x[0] < (int)x[1], "Is inferior"),
            new Verb<DComparator>("<=", (x) => (int)x[0] <= (int)x[1], "Is inferior or equal"),
            new Verb<DComparator>("><", (x) => ReferenceEquals(x[0], null), "Is null")
        };
        static public List<Verb> VerbsLogicalOperators = new List<Verb>()
        {
            new Verb("and", (x) => {}, "Logical operator 'and'"),
            new Verb("or", (x) => {}, "Logical operator 'or'")
        };
        static public List<Verb> VerbsModifiers = new List<Verb>();
        static public List<Verb<DFunction>> VerbsFunctions = new List<Verb<DFunction>>()
        {
            new Verb<DFunction>("SetExecutionRate", (x) => (true, ""), "Set the execution rate in MS", new string[] { "Value:val" })
        };
        static public List<string> Properties = new List<string>();
        static public void Initialize(
            Dictionary<string, PVarType> PublicVariables,
            List<Verb<DFunction>> VerbsFunctions,
            List<string> Properties
            )
        {
            SetPublicVariables(PublicVariables);
            SetFunctions(VerbsFunctions);
            SetProperties(Properties);
        }

        static public void SetPublicVariables(Dictionary<string, PVarType> publicVariables) => PublicVariables = publicVariables;
        static public void SetConditions(List<Verb> conditions) => VerbsConditions = conditions;
        static public void SetSubjects(List<Verb<Func<object>>> subjects) => VerbsSubjects = subjects;
        static public void SetOperators(List<Verb> operators) => VerbsOperators = operators;
        static public void SetAssignators(List<Verb> assignators) => VerbsAssignators = assignators;
        static public void SetComparators(List<Verb<DComparator>> comparators) => VerbsComparators = comparators;
        static public void SetLogicalOperators(List<Verb> logicalOperators) => VerbsLogicalOperators = logicalOperators;
        static public void SetModifiers(List<Verb> modifiers) => VerbsModifiers = modifiers;
        static public void SetFunctions(List<Verb<DFunction>> functions) => VerbsFunctions = functions;
        static public void SetProperties(List<string> properties)
        {
            Properties = properties;

            List<string> PublicVariablesNames = PublicVariables.Keys.ToList();
            foreach (string vN in PublicVariablesNames)
            {
                Dictionary<string, int> d = new Dictionary<string, int>();
                foreach (string prop in Properties)
                    d[prop] = 0;
                PublicVariables[vN] = new PVarType(d);
            }
        }

        static public List<string> SyntaxesOf(List<Verb> verbs) => verbs.Select(x => x.Syntax).ToList();
        static public List<string> SyntaxesOf(List<Verb<Func<object>>> verbs) => verbs.Select(x => x.Syntax).ToList();
        static public List<string> SyntaxesOf(List<Verb<DFunction>> verbs) => verbs.Select(x => x.Syntax).ToList();
        static public List<string> SyntaxesOf(List<Verb<DComparator>> verbs) => verbs.Select(x => x.Syntax).ToList();
        static public List<DMethod> MethodsOf(List<Verb> verbs) => verbs.Select(x => x.Method).ToList();
        static public List<Func<object>> MethodsOf(List<Verb<Func<object>>> verbs) => verbs.Select(x => x.Method).ToList();
        static public List<DFunction> MethodsOf(List<Verb<DFunction>> verbs) => verbs.Select(x => x.Method).ToList();
        static public List<DComparator> MethodsOf(List<Verb<DComparator>> verbs) => verbs.Select(x => x.Method).ToList();
        static public List<string> DescriptionsOf(List<Verb> verbs) => verbs.Select(x => x.Description).ToList();
        static public List<string> DescriptionsOf(List<Verb<Func<object>>> verbs) => verbs.Select(x => x.Description).ToList();
        static public List<string> DescriptionsOf(List<Verb<DFunction>> verbs) => verbs.Select(x => x.Description).ToList();
        static public List<string> DescriptionsOf(List<Verb<DComparator>> verbs) => verbs.Select(x => x.Description).ToList();
        static public List<string[]> ArgumentsOf(List<Verb> verbs) => verbs.Select(x => x.Arguments).ToList();
        static public List<string[]> ArgumentsOf(List<Verb<Func<object>>> verbs) => verbs.Select(x => x.Arguments).ToList();
        static public List<string[]> ArgumentsOf(List<Verb<DFunction>> verbs) => verbs.Select(x => x.Arguments).ToList();
        static public List<string[]> ArgumentsOf(List<Verb<DComparator>> verbs) => verbs.Select(x => x.Arguments).ToList();
        static public List<string> PublicVariablesNames => PublicVariables.Keys.ToList();
        static public Verb GetVerbFromSyntax(List<Verb> verbs, string syntax) => verbs.First(x => x.Syntax == syntax);
        static public Verb<Func<object>> GetVerbFromSyntax(List<Verb<Func<object>>> verbs, string syntax) => verbs.First(x => x.Syntax == syntax);
        static public Verb<DFunction> GetVerbFromSyntax(List<Verb<DFunction>> verbs, string syntax) => verbs.First(x => x.Syntax == syntax);
        static public Verb<DComparator> GetVerbFromSyntax(List<Verb<DComparator>> verbs, string syntax) => verbs.First(x => x.Syntax == syntax);

        static public List<Verb<DFunction>> Functions => VerbsFunctions;
        static public List<Verb> Conditions => VerbsConditions;
        static public List<Verb<Func<object>>> Subjects => VerbsSubjects;
        static public List<Verb> Operators => VerbsOperators;
        static public List<Verb> Assignators => VerbsAssignators;
        static public List<Verb<DComparator>> Comparators => VerbsComparators;
        static public List<Verb> LogicalOperators => VerbsLogicalOperators;
        static public List<Verb> Modifiers => VerbsModifiers;

        static public bool IsPublicVariable(string word)
        {
            return PublicVariablesNames.Contains(word.Replace(" ", ""));
        }
        static public bool IsFunction(string word)
        {
            return SyntaxesOf(Functions).Contains(word.Replace(" ", ""));
        }
        static public bool IsCondition(string word)
        {
            return SyntaxesOf(Conditions).Contains(word.Replace(" ", ""));
        }
        static public bool IsSubject(string word)
        {
            return SyntaxesOf(Subjects).Contains(word.Replace(" ", ""));
        }
        static public bool IsOperator(string word)
        {
            return SyntaxesOf(Operators).Contains(word.Replace(" ", ""));
        }
        static public bool IsAssignator(string word)
        {
            return SyntaxesOf(Assignators).Contains(word.Replace(" ", ""));
        }
        static public bool IsComparator(string word)
        {
            return SyntaxesOf(Comparators).Contains(word.Replace(" ", ""));
        }
        static public bool IsLogicalOperator(string word)
        {
            return SyntaxesOf(LogicalOperators).Contains(word.Replace(" ", ""));
        }
        static public bool IsModifier(string word)
        {
            return SyntaxesOf(Modifiers).Contains(word.Replace(" ", ""));
        }
        static public bool IsProperty(string property)
        {
            return Properties.Contains(property);
        }

        static public (object Entity, int prevValue, int Value) AssignatorsGetEntityAnValue(string[] words, int firstWordNotModifier)
        {
            string word = words[firstWordNotModifier];
            if (word.Contains('.'))
            {
                string variable = word.Split('.')[0];
                string property = word.Split('.')[1];
                string assign = words[firstWordNotModifier + 1];
                string val = words[firstWordNotModifier + 2];
                if (GetTypeFromRaw(val, Variables) == KeywordType.Value && IsAssignator(assign) && IsProperty(property))
                {
                    if(IsPublicVariable(variable))
                        return (PublicVariables[variable].Properties[property], 0, int.Parse(val));
                    if(IsSubject(variable))
                        return ((GetVerbFromSyntax(Subjects, variable), property), 0, int.Parse(val));
                }
            }
            else
            {
                string wordValue = words[firstWordNotModifier + 2];
                int previousValue = 0;
                if (IsVariable(wordValue))
                    previousValue = int.Parse(Variables[wordValue]);
                else if (wordValue.Contains('.') && IsProperty(wordValue.Split('.')[1]))
                    previousValue = (GetEntity(wordValue.Split('.')[0]) as PVarType).Properties[wordValue.Split('.')[1]];
                if (!(GetEntity(word) is int))
                    return (null, 0, 0);
                object v = GetEntity(word);
                return (v, previousValue, int.Parse(ComputeExpression(GetExpression(words, firstWordNotModifier + 2, "@END").Expression)));
            }
            return (null, 0, 0);
        }
        #endregion


        public string script;

        public InputScript()
        {
            script = "";
        }
        public InputScript(InputScript inputScript)
        {
            script = inputScript.script;
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

            if (InputScriptEditor.IsProperty(raw))
                return KeywordType.Variable;

            return KeywordType.Normal;
        }

        #region ScriptExecution

        Dictionary<string, string> InstanceVariables = new Dictionary<string, string>();
        static Dictionary<string, string> Variables = new Dictionary<string, string>();
        bool ScriptError = false;
        public int ExecutionRate = 500;

        static public bool IsVariable(string word)
        {
            return Variables.ContainsKey(word);
        }
        static public string GetVariableValue(string word)
        {
            return IsVariable(word) ? Variables[word.Replace(" ", "")] : "0";
        }
        static public object GetEntity(string SUBJ)
        {
            if (IsPublicVariable(SUBJ))
            {
                return PublicVariables[SUBJ];
            }
            else if (IsVariable(SUBJ))
            {
                return Variables[SUBJ];
            }
            else if (IsSubject(SUBJ))
            {
                return GetVerbFromSyntax(Subjects, SUBJ);
            }
            return null;
        }

        Type InstanceSelfType;
        static Type SelfType;
        public void ScriptMain<T>(T self)
        {
            InstanceSelfType = self.GetType();
            GetVerbFromSyntax(Subjects, "self").Method = () => self;
            if (!IsFunction("SetExecutionRate"))
                Functions.Add(new Verb<DFunction>("SetExecutionRate", (x) => (true, ""), "Set the execution rate in MS", new string[] { "Value:val" }));
            GetVerbFromSyntax(Functions, "SetExecutionRate").Method = (x) =>
            {
                int rate = ExecutionRate;
                if (!int.TryParse((string)x[0], out rate))
                    return (false, "The argument 1 '" + x[0] + "' should be a number (in ms).");
                ExecutionRate = rate;
                return (true, "");
            };

            SelfType = InstanceSelfType;
            Variables = InstanceVariables;

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
                if (IsPublicVariable(word1) || IsSubject(word1) || InputScriptEditor.IsProperty(word1))
                {
                    if (IsOperator(word2))
                    {
                        GetVerbFromSyntax(Operators, word2).Method.Invoke(new object[] { word1 });
                    }
                    else if (IsAssignator(word2))
                    {
                        GetVerbFromSyntax(Assignators, word2).Method.Invoke(new object[] { words, firstWordNotModifier });
                    }
                }

                return (true, "");
            }
            catch (Exception e)
            {
                return (false, $"[ModifyGlobalVariable] : {e.Message}");
            }
        }

        static private (string Expression, int NextIndex) GetExpression(string[] words, int StartIndex, string EndToken)
        {
            (string Expression, int NextIndex) result = ("", 0);
            for (int w = StartIndex; w < words.Length; w++)
            {
                if (EndToken[0] == '$')
                {
                    if (GetTypeFromRaw(words[w]) != (KeywordType)Enum.Parse(typeof(KeywordType), string.Concat(EndToken.Skip(1))))
                    {
                        result.Expression += GetBindedWord(words[w]) + " ";
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
                        continue;
                    }
                }
                else
                {
                    if (words[w] != EndToken)
                    {
                        result.Expression += GetBindedWord(words[w]) + " ";
                        continue;
                    }
                }

                result.NextIndex = w;
                return result;
            }

            return result;
        }
        static private string ComputeExpression(string EXP)
        {
            return new System.Data.DataTable().Compute(EXP, "").ToString();
        }
        private (bool Success, string Error) Function(string FUNC, string SUBJ, string[] ARGS)
        {
            object target = GetEntity(SUBJ);
            if (target == null)
                return (false, "The target subject '" + SUBJ + "' doesn't exist.");
            if (target == null)
                return (true, "");

            return GetVerbFromSyntax(Functions, FUNC).Method.Invoke(new object[] { target, ARGS });
        }
        private (bool Success, string Error) Condition(string EXP1, string EXP2, string COMP, string FUNC, string SUBJ, string[] ARGS)
        {
            int VAL1 = 0, VAL2 = 0;
            bool VAL1_ISINT = int.TryParse(EXP1, out VAL1);
            if (!VAL1_ISINT && IsPublicVariable(EXP1))
            {
                object obj = GetEntity(EXP1.Replace(" ", ""));
                if (obj is int)
                {
                    VAL1 = (int)GetEntity(EXP1.Replace(" ", ""));
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

            if(!IsComparator(COMP))
                return (false, $"The comparator verb has to be a comparator within the syntax (#EXP:@COMP@, #CUR:'{COMP}'");

            bool RSLT = (bool)GetVerbFromSyntax(Comparators, COMP).Method.Invoke(new object[] { VAL1, VAL2 });

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
        static private string GetBindedWord(string word)
        {
            if (int.TryParse(word, out int unused))
                return word;

            if (IsVariable(word))
                return GetVariableValue(word);

            if (word.Split('.').Length == 2)
                if (IsPublicVariable(word.Split('.')[0]))
                    if (IsProperty(word.Split('.')[1]))
                    {
                        object entity = GetEntity(word.Split('.')[0]);
                        if (Convert.ChangeType(entity, SelfType) != null)
                            return GetSubjectProperty(entity, word.Split('.')[1]);
                        if (entity is int)
                            return entity.ToString();
                        return null;
                    }
            return word;
        }

        static private string GetSubjectProperty(object entity, string propertyName)
        {
            return SelfType.GetProperty(propertyName).GetValue(entity).ToString();
        }

        static private void SetPublicVariableValue(string variable, PVarType value)
        {
            PublicVariables[variable] = value;
        }
        static private PVarType GetPublicVariableValue(string variable)
        {
            return PublicVariables[variable];
        }

        #endregion
    }
}
