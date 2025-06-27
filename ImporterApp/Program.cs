﻿using ImporterApp;
using ImporterApp.Services;
// メイン処理、起動用
class Program
{
    static void Main()
    {
        var executor = new ImporterExecutor();
        // 新增：初始化规则引擎并输出SQL
        var rulesPath = System.IO.Path.Combine(System.AppContext.BaseDirectory, "rules.csv");
        var meaningRulesPath = System.IO.Path.Combine(System.AppContext.BaseDirectory, "meaning_rules.csv");
        var ruleEngine = new RuleEngine(rulesPath, meaningRulesPath);
        
        Console.WriteLine("\n==================== GENERATED SQL RULES ====================");
        foreach (var group in ruleEngine.Rules)
        {
            var table = group.TargetTable;
            var column = group.TargetColumn;
            var itemId = group.ItemId;
            var resultValue = group.ResultValue;
            var outType = group.OutType;
            if (string.IsNullOrEmpty(table) || string.IsNullOrEmpty(column)) continue;

            var setPart = $"{column} = '{resultValue}'";
            var whereParts = group.Conditions.Select(cond =>
            {
                var op = cond.Operator ?? "=";
                var cmp = cond.CompareValue ?? "";
                var colIdx = cond.ColumnIndex > 0 ? $"COL_{cond.ColumnIndex - 1}" : column;
                return $"{colIdx} {op} '{cmp}'";
            });
            var whereClause = string.Join(" AND ", whereParts);
            var sql = $"UPDATE {table} SET {setPart} WHERE {whereClause};";
            Console.WriteLine($"[SQL] {sql}");
        }
        Console.WriteLine("================== END GENERATED SQL RULES =================\n");
        
        executor.Execute("staging.csv", "ユースジID1");
    }
}

