using Script;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Script.InputScript;

namespace ConsoleApp1
{
    class Program
    {
        public class Entity
        {
            public int a { get; set; }
            public int b { get; set; }
            public int c { get; set; }

            public Entity(int a, int b, int c)
            {
                this.a = a;
                this.b = b;
                this.c = c;
            }

            public (bool, string) Set(params object[] args)
            {
                try
                {
                    object[] p = (object[])args[0];

                    if ((string) p[0] != "@")
                        a = int.Parse((string)p[0]);
                    if ((string)p[1] != "@")
                        b = int.Parse((string)p[1]);
                    if ((string)p[2] != "@")
                        c = int.Parse((string)p[2]);
                }
                catch(Exception)
                { }

                return (true, "");
            }

            public override string ToString()
            {
                return $"({a}, {b}, {c})";
            }
        }

        static void Main(string[] args)
        {
            Entity Me = new Entity(0, 0, 0);

            Dictionary<string, PVarType> PublicVariables = new Dictionary<string, PVarType>()
            {
                ["A"] = new PVarType(new[] { 1, 2, 3 }),
                ["B"] = null,
                ["C"] = null
            };
            List<Verb<DFunction>> VerbsFunctions = new List<Verb<DFunction>>()
            {
                new Verb<DFunction>("Test", (x) => (true, ""), "Test", new []{ "Value:val" }),
                new Verb<DFunction>("Set", Me.Set, "Test", new []{ "Value:a", "Value:b", "Value:c" })
            };

            InputScript script = new InputScript();
            Initialize(PublicVariables, VerbsFunctions);
            InputScriptEditor editor = new InputScriptEditor(script, typeof(Entity), () => { });

            editor.ShowDialog();
            editor.Execute(Me);

            Console.WriteLine(Me);
            Console.ReadKey(true);
        }
    }
}
