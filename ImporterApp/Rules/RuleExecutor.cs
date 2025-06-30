using System.Collections.Generic;
using ImporterApp.Models;
using ImporterApp.Services;
using ImporterApp.Infrastructure;

namespace ImporterApp.Rules
{
    public static class RuleExecutor
    {
        // ルールを実行し、Productモデルに反映する
        public static Product ExecuteRules(List<RuleGroup> rules, Dictionary<string, string> rowData, string userScenarioId)
        {
            Product product = new Product();
            foreach (var rule in rules)
            {
                // ここは実際の業務ロジックを拡張可能
                rule.Usage = rule.Usage ?? string.Empty; // Usageがnullの場合は空文字にする
                if (rule.Usage != userScenarioId)
                {
                    // ユーズIDが一致しない場合はスキップ
                    Logger.Info($"[RULE SKIP] RuleId={rule.RuleId} はuserSceanarioId={userScenarioId}にマッチしないためスキップされました");
                    continue;
                }
                // ルールの適用
                // Logger.Info($"[RULE EXEC] RuleId={rule.RuleId} が実行され、{rule.TargetTable} の {rule.TargetColumn} が変更されました");
                // Logger.Info($"CONDITIONS: {rule.Conditions.Count} 条件");
                // EvaluateConditions(rule.Conditions, rowData);
                // Logger.Info($"[RULE EXEC] RuleId={rule.RuleId} 条件評価完了");
                if (EvaluateConditions(rule.Conditions, rowData, userScenarioId))
                {
                    ApplyRuleToProduct(rule, rowData, product);
                }

                // Logger.Info($"[RULE EXEC] RuleId={rule.RuleId} がProductモデル{product.ProductCode}に適用されました");
            }
            return product; // 新しいProductオブジェクトを返す。実際の運用では変更後の製品を返す場合もある
        }

        // ルール条件の評価メソッド
        public static bool EvaluateConditions(List<RuleCondition> conditions, Dictionary<string, string> rowData, string userScenarioId)
        {
            // usage（userScenarioId）が指定されている場合、最初の条件のUsage列（またはRuleGroupのUsage）と一致しなければfalseを返す
            // ...既存の条件評価ロジック...
            bool? lastResult = null;
            for (int i = 0; i < conditions.Count; i++)
            {
                var cond = conditions[i];
                var condNext = (i + 1 < conditions.Count) ? conditions[i + 1] : null;
                // 現在の条件の値を取得
                string key = cond.ColumnIndex.ToString();
                string value1 = string.Empty;
                if (!string.IsNullOrEmpty(key))
                {
                    rowData.TryGetValue(key, out value1);
                }
                bool eval1 = Evaluate(value1, cond.Operator ?? string.Empty, cond.CompareValue ?? string.Empty);
                bool eval2 = false;
                if (condNext != null)
                {
                    string key2 = condNext.ColumnIndex.ToString();
                    string value2 = string.Empty;
                    if (!string.IsNullOrEmpty(key2)) rowData.TryGetValue(key2, out value2);
                    eval2 = Evaluate(value2, condNext.Operator ?? string.Empty, condNext.CompareValue ?? string.Empty);
                }
                switch (cond.Logic)
                {
                    case "AND":
                        lastResult = eval1 && eval2;
                        break;
                    case "OR":
                        lastResult = eval1 || eval2;
                        break;
                    default:
                        lastResult = eval1;
                        break;
                }
                Logger.Info($"[COND EVAL] Seq={cond.ConditionSeq}, Logic={cond.Logic}, Result={lastResult}");
                if (lastResult == false) break;
            }
            return lastResult ?? false;
        }

        // シンプルな比較メソッド
        private static bool Evaluate(string value, string op, string compare)
        {
            switch (op)
            {
                case "=":
                    return value == compare;
                case "<>":
                    return value != compare;
                case ">":
                    return double.TryParse(value, out var d1) && double.TryParse(compare, out var d2) && d1 > d2;
                case "<":
                    return double.TryParse(value, out var d3) && double.TryParse(compare, out var d4) && d3 < d4;
                case "LIKE":
                    if (value == null || compare == null) return false;
                    // SQL風: %abc, abc%, %abc%, abc
                    if (compare.StartsWith("%") && compare.EndsWith("%"))
                        return value.Contains(compare.Trim('%'));
                    else if (compare.StartsWith("%"))
                        return value.EndsWith(compare.TrimStart('%'));
                    else if (compare.EndsWith("%"))
                        return value.StartsWith(compare.TrimEnd('%'));
                    else
                        return value == compare;
                case "TRUE":
                    return value != null && value.ToUpper() == "TRUE";
                default:
                    return true;
            }
        }
        // 登録位置はrule.CSVから読み込んでNewAttributeMeaningRuleオブジェクトのTargetTable/TargetColumnに保存された値による
        // ここでrule.CSVから読み込んだ出力タイプが「そのまま登録」か「変換して登録」かを判定
        // 「そのまま登録」の場合、結果値の数字をインデックスとしてstaging.csvの該当列データを取得し、
        // そのデータをTargetTableのTargetColumnに登録
        // 「変換して登録」の場合、結果値の文字列をそのままTargetTableのTargetColumnに登録
        // TargetTableがPRODUCT_MSTならProductモデルの主属性に、
        // PRODUCT_EAVならProductモデルの拡張属性（List<ProductAttribute> Attributes）に登録
        // ルール実行：出力タイプとルール内容に基づき値を決定し、Productモデルに書き込み、productを返す
        public static void ApplyRuleToProduct(RuleGroup rule, Dictionary<string, string> rowData, Product product)
        {
            string value = string.Empty;
            if (rule.OutType == "そのまま登録")
            {
                var key = rule.ResultValue;
                rowData.TryGetValue(key, out value);
                Logger.Info($"[RULE APPLY] RuleId={rule.RuleId}、{rule.TargetTable}の{rule.TargetColumn}に値を設定：{value}");
            }
            else if (rule.OutType == "変換して登録")
            {
                value = rule.ResultValue ?? string.Empty;
            }
            // Productモデルに書き込み
            if (rule.TargetTable == "PRODUCT_MST")
            {
                switch (rule.TargetColumn)
                {
                    case "PRODUCT_CODE": product.ProductCode = value ?? string.Empty; break;
                    case "BRAND_ID": product.BrandId = value ?? string.Empty; break;
                    case "PRODUCT_NAME": product.ProductName = value ?? string.Empty; break;
                    case "CATEGORY": product.CategoryName = value ?? string.Empty; break;
                    case "STATUS": product.State = value ?? string.Empty; break;
                    default: break;
                }
            }
            else if (rule.TargetTable == "PRODUCT_EAV")
            {
                if (!string.IsNullOrEmpty(rule.ItemId) && !product.Attributes.Exists(a => a.AttributeId == rule.ItemId))
                {
                    // 在这里进行判断，做一个mapping
                    // 例如 size1、size2、size3 ，如果有size1——
                    // 这里可以调用 RuleEngine.MapAttributeId
                    product.Attributes.Add(new ProductAttribute { AttributeId = rule.ItemId, Value = value ?? string.Empty });
                }
            }
            //Logger.Info($"[RULE APPLY] RuleId={rule.RuleId}、{rule.TargetTable}の{rule.TargetColumn}に値を設定：{value}");
            // return product;
        }
    }
}
