using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace DOSBOX2.Common
{
    internal class AIBehavior
    {
        public List<string> Prios;
        public List<string> Commands;

        string CurrentCommand = null;
        bool loop = false;
        Action CurrentBehavior = null;
        List<object> args = new List<object>();

        public AIBehavior(List<string> behavior = null)
        {
            Commands = behavior ?? new List<string>();
        }

        public virtual void Behave(Entity self, EntityManager manager)
        {
            string cmd,key,val;
            string[] words;
            int range = 50;
            List<Entity> entities = null;
            for(int i = 0; i < Prios.Count; i++)
            {
                cmd = Prios[i];
                words = cmd.Split(' ');
                switch(words[0])
                {
                    case "entity_visible":
                        int k = 0;
                        while (cmd_IsPair(words[k]))
                        {
                            (key, val) = cmd_ParsePair(words[k]);
                            switch (key)
                            {
                                case "range": if(!int.TryParse(val, out range)) range = 50; break;
                                case "entity":
                                    var (arg_key, arg_val) = cmd_ParseArgPair(val);
                                    entities = manager.Entities.Where(e => e.GetType().GetField(arg_key, System.Reflection.BindingFlags.Public)?.GetValue(e)?.ToString() == arg_val).ToList();
                                    break;
                            }
                            k++;
                        }
                        if (entities.FirstOrDefault()?.center.Distance(self.center) < range)
                        {
                            Do_Command(k);
                        }
                        break;
                }
            }

            void Do_Command(int index)
            {
                int k = 0;
                while (index + k < words.Length)
                {
                    switch (words[index + k++])
                    {

                        //case "range": if (!int.TryParse(val, out range)) range = 50; break;
                        //case "entity":
                        //    var (arg_key, arg_val) = cmd_ParseArgPair(val);
                        //    entities = manager.Entities.Where(e => e.GetType().GetField(arg_key, System.Reflection.BindingFlags.Public)?.GetValue(e)?.ToString() == arg_val).ToList();
                        //    break;

                        case "wait":
                            (key, val) = cmd_ParsePair(words[index + k]);
                            if (key == "time")
                            {
                                if (!int.TryParse(val, out int time)) time = 500;
                                var timer = new Stopwatch();
                                timer.Start();
                                var behavior = new Action(() => { if (timer.ElapsedMilliseconds >= time) Reset(); });
                                var args = new List<object>() { timer };
                                Set(behavior, args);
                            }
                            break;
                        case "go":
                            (key, val) = cmd_ParsePair(words[index + k]);
                            switch(key)
                            {
                                case "left": break;
                                case "right": break;
                                case "forward": break;
                            }
                            break;
                        case "turn":
                            (key, val) = cmd_ParsePair(words[index + k]);
                            switch (key)
                            {
                                case "left": break;
                                case "right": break;
                                case "opposit": break;
                            }
                            break;
                        case "shot":
                            break;
                    }
                    k++;
                }
            }
            void Set(Action behavior, List<object> args)
            {
                Reset();
                CurrentBehavior = behavior;
                this.args = args;
            }
            void Reset()
            {
                CurrentBehavior = null;
                args.Clear();
            }
        }
        private bool cmd_IsPair(string word) => word.Split(':').Length > 1;
        private (string key, string val) cmd_ParsePair(string pair)
        {
            string[] elements = pair.Split(':');
            return elements.Length != 2 || !elements[1].StartsWith("\"") || !elements[1].EndsWith("\"") ? (null, null) : (elements[0], elements[1]);
        }
        private (string key, string val) cmd_ParseArgPair(string arg)
        {
            string[] elements = arg.Split(',');
            return elements.Length != 2 ? (null, null) : (elements[0], elements[1]);
        }
    }
}

/*

        There is Prios and Commands :

            Prios : do command in prio if condition matches

                    entity_visible (arg: entity info, range in pixels)

            Commands : all cmds repeated in loop : with [A][B][C] do [A] then [B] then [C] then [A] then [B] etc...

                    wait
                        time
                    go
                        left
                        right
                        forward
                    turn
                        left
                        right
                        opposit
                    shot

Exemples :
    entity_visible entity:"Name,Player" range:"50" wait time:"500" shot

*/