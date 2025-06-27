using ImporterApp.Services;
using System.Text;
using System.Linq;

namespace ImporterApp.Services
{
    public static class SqlRule
    {
        // 生成并输出所有规则组的SQL语句
        public static void OutputRuleSql(IRuleEngine ruleEngine)
        {
            foreach (var group in ruleEngine.Rules)
            {
                var table = group.TargetTable;
                var column = group.TargetColumn;
                var itemId = group.ItemId;
                var resultValue = group.ResultValue;
                var outType = group.OutType;
                if (string.IsNullOrEmpty(table) || string.IsNullOrEmpty(column)) continue;

                // 生成SET部分
                var setPart = $"{column} = '{resultValue}'";
                // 生成WHERE部分
                var whereParts = group.Conditions.Select(cond =>
                {
                    var op = cond.Operator ?? "=";
                    var cmp = cond.CompareValue ?? "";
                    var colIdx = cond.ColumnIndex > 0 ? $"COL_{cond.ColumnIndex - 1}" : column;
                    return $"{colIdx} {op} '{cmp}'";
                });
                var whereClause = string.Join(" AND ", whereParts);
                var sql = $"UPDATE {table} SET {setPart} WHERE {whereClause};";
                Logger.Info($"[SQL] {sql}");
            }
        }
    }
}
