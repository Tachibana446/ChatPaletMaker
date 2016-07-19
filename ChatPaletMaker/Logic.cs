using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ChatPaletMaker
{
    class Logic
    {

        public static string Make(IEnumerable<string> lines)
        {
            string result = "";
            var flag = Flag.Neutral;

            foreach (var l in lines)
            {
                var line = Regex.Replace(l, @"\s+", " ").Trim();
                // 基本情報とライフパスをスキップ
                if (flag == Flag.Neutral && !Regex.IsMatch(line, "能力値と技能")) continue;
                if (Regex.IsMatch(line, "能力値と技能")) flag = Flag.StatusAndSkill;
                if (Regex.IsMatch(line, "エフェクト")) flag = Flag.Effect;
                if (Regex.IsMatch(line, "装備とコンボ")) flag = Flag.WeponAndCombo;
                if (Regex.IsMatch(line, "所持品")) break;

                var dice = "";
                var list = new List<string>();
                switch (flag)
                {
                    case Flag.StatusAndSkill:
                        if (Regex.IsMatch(line, "【")) result += line + "\n";
                        else
                        {
                            list = line.Split(' ').ToList();
                            if (list.Count < 2) break;
                            dice = list.Last();
                            list.Remove(dice);
                            dice = dice.Replace("r", "dx");
                            var type = Regex.Replace(list.First(), @"：SL\d*", "");
                            result += dice + " " + type + "\n";
                        }
                        break;
                    case Flag.Effect:
                        if (Regex.IsMatch(line, "スキル名"))
                        {
                            result += line + "\n";
                            break;
                        }

                        list = line.Split('/').ToList();
                        var first = list.First().Replace("《", "").Replace("》", "");
                        list = list.Skip(1).Select(s => s.Replace("$", "")).ToList();
                        result += first + string.Join("/", list.Skip(1)) + "\n";

                        break;
                    case Flag.WeponAndCombo:
                        if (Regex.IsMatch(line, "(武器とコンボ)"))
                        {
                            result += line + "\n";
                            break;
                        }
                        if (Regex.IsMatch(line, "(名称|価格合計)")) break;
                        list = line.Split(' ').ToList();
                        try { dice = list.First(s => Regex.IsMatch(s, @"\dr")); }
                        catch { break; }

                        list.Remove(dice);
                        dice = dice.Replace("r", "dx");
                        result += dice + " " + list.First() + " " + list.Last() + " \n";

                        if (Regex.IsMatch(line, "防具")) { flag = Flag.Neutral; break; }
                        break;
                    default:
                        break;
                }
            }
            return result;
        }
    }

    enum Flag
    {
        /// <summary>
        /// 能力値と技能
        /// </summary>
        StatusAndSkill,
        Effect,
        WeponAndCombo,
        Neutral
    }
}
