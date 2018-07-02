// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 2:25 PM 2018/1/24
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TestingApplication
{
    public class KeyboardSpecUserAction : AbstractSpecUserAction
    {
        public override ScriptsExpression GenRawScripts(IScriptGenerationParams scriptGenerationParams)
        {
            string re = "";
            var p = ParseExpression(this.expression);
            int indi = p.Item1;
            if (indi == 1)
            {
                string[] keys = p.Item2;
                foreach (string _key in keys)
                {
                    if (re != "")
                        re += NEW_LINE;
                    re += "Keyboard.SendSingleKey(\"" + _key + "\");";
                }
            }
            else if (indi == 2)
            {         
                string k1 = p.Item2[0];
                string k2 = p.Item2[1];
                re += "Keyboard.SendCombinedKeys(\"" + k1 + "\", \"" + k2 + "\");";
            }
            else
            {
                LogError(logger, expression);
                return null;
            }
            return new ScriptsExpression(re);
        }

        public override ScriptsExpression GenScripts(IScriptGenerationParams scriptGenerationParams)
        {
            return base.GenScripts(scriptGenerationParams);
        }

        public override ScriptsExpression GenRanorexScripts(IRanorexScriptGenerationParams scriptGenerationParams)
        {
            return base.GenRanorexScripts(scriptGenerationParams);
        }

        public override ScriptsExpression GenRawRanorexScripts(IRanorexScriptGenerationParams scriptGenerationParams)
        {
            string re = "Keyboard.Press(\"";
            var p = ParseExpression(this.expression);
            int indi = p.Item1;
            if (indi == 1)
            {
                var listActions = p.Item2;
                for (int fi = 0; fi < listActions.Length; fi++)
                {
                    string action = listActions[fi].Trim();
                    re += "{" + replaceReturnChar(RemoveSingleQuote(action)) + "}";
                }
                re += "\");";
            }
            else if (indi == 2)
            {
                string[] keys = p.Item2;
                string leftScript = ""; string rightScript = "";
                for (int fi = 0; fi < keys.Length - 1; fi++)
                {
                    string key = keys[fi].Trim();
                    leftScript += "{" + key + " down}";
                    rightScript = "{" + key + " up}" + rightScript;
                }
                re += leftScript;
                string lastKey = keys[keys.Length - 1].Trim();
                re += "{" + lastKey;
                if (Regex.IsMatch(lastKey, "\\w"))
                    re += "key";
                re += "}" + rightScript;
                re += "\");";
            }
            else
            {
                LogError(logger, expression);
                return null;
            }
            return new ScriptsExpression(re);
        }
        
        /// <summary>
        /// return array, the first is indicator
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Tuple<int, string[]> ParseExpression(string expression)
        {
            /**
             * case 'a';'b';'c';'Enter'
             */
            Regex regex1 = new Regex("'.*'(\\s*;\\s*'.*')*");

            /**
             * case {LControlKey; C}
             */
            Regex regex2 = new Regex("\\{(?<key1>(.*));(?<key2>(.*))\\}");
            Match match = regex2.Match(expression);

            if (regex1.IsMatch(expression))
            {
                string[] keys = Utils.SplitIgnoreInside(expression, ";", "'");
                return new Tuple<int, string[]>(1, keys); // 1 indicate the first case
            }
            else if (match.Success)
            {
                string k1 = match.Groups["key1"].Value.Trim();
                string k2 = match.Groups["key2"].Value.Trim();
                return new Tuple<int, string[]>(2, new string[2] { k1, k2 }); // 2 indicate the second case
            }
            return new Tuple<int, string[]>(0, null);
        }
    }
}
