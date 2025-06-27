using System.Collections.Generic;
using ImporterApp.Models;
using ImporterApp.Services;
using ImporterApp.Infrastructure;

namespace ImporterApp.Rlues
{
    public static class RuleExecutor
    {
        // 执行规则并输出日志
        public static Product ExecuteRules(List<RuleGroup> rules, Dictionary<string, string> rowData)
        {
            Product product = new Product();
            foreach (var rule in rules)
            {
                // 这里只是模拟执行，实际业务逻辑可扩展
                // Logger.Info($"[RULE EXEC] RuleId={rule.RuleId} 已被運行，{rule.TargetTable} 的 {rule.TargetColumn} 被修改");
                // Logger.Info($"CONDITIONS: {rule.Conditions.Count} 条件");
                // EvaluateConditions(rule.Conditions, rowData);
                // Logger.Info($"[RULE EXEC] RuleId={rule.RuleId} 条件评估完成");
                if (EvaluateConditions(rule.Conditions, rowData))
                {
                    ApplyRuleToProduct(rule, rowData, product);
                }

                // Logger.Info($"[RULE EXEC] RuleId={rule.RuleId} 已应用到产品模型{product.ProductCode}");
            }
            return product; // 返回一个新的Product对象，实际应用中可能需要返回修改后的产品
        }

        // 规则条件判定方法
        public static bool EvaluateConditions(List<RuleCondition> conditions, Dictionary<string, string> rowData)
        {
            // Logger.Info($"[RULE EXEC] Evaluating conditions for {conditions.Count} 条件");
            bool? lastResult = null;
            for (int i = 0; i < conditions.Count; i++)
            {
                var cond = conditions[i];
                // Logger.Info($"[COND EVAL] 处理条件 {i + 1}/{conditions.Count}: Seq={cond.ConditionSeq}, ColumnIndex={cond.ColumnIndex}, Operator={cond.Operator}, CompareValue={cond.CompareValue}, Logic={cond.Logic}");
                // 获取当前和下一个条件
                var condNext = (i + 1 < conditions.Count) ? conditions[i + 1] : null;
                // 获取当前条件的值
                string key = cond.ColumnIndex.ToString();
                string value1 = string.Empty;
                if (!string.IsNullOrEmpty(key))
                {
                    rowData.TryGetValue(key, out value1);
                    // Logger.Info($"[COND EVAL] VALUE1: {value1}, ColumnIndex: {cond.ColumnIndex}, Operator: {cond.Operator}, CompareValue: {cond.CompareValue}");
                }
                bool eval1 = Evaluate(value1, cond.Operator ?? string.Empty, cond.CompareValue ?? string.Empty);
                Logger.Info($"[COND EVAL] Seq={cond.ConditionSeq}, Logic={cond.Logic}, Eval1={eval1}");
                bool eval2 = false;
                if (condNext != null)
                {
                    string key2 = condNext.ColumnIndex.ToString();
                    string value2 = string.Empty;
                    if (!string.IsNullOrEmpty(key2)) rowData.TryGetValue(key2, out value2);
                    // Logger.Info($"[COND EVAL] VALUE2: {value2}, ColumnIndex: {condNext.ColumnIndex}, Operator: {condNext.Operator}, CompareValue: {condNext.CompareValue}");
                    eval2 = Evaluate(value2, condNext.Operator ?? string.Empty, condNext.CompareValue ?? string.Empty);
                    Logger.Info($"[COND EVAL] Seq={condNext.ConditionSeq}, Logic={condNext.Logic}, Eval2={eval2}");
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

        // 简单的比较方法
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
                    // SQL风格: %abc, abc%, %abc%, abc
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
        //todo
        //登陆位置取决于rule.CSV当中读取到并保存至NewAttributeMeaningRule对象的TargetTable的TargetColumn
        //在这里比较从rule.CSV当中读取到的出力タイプ是そのまま登録还是変換して登録
        //当そのまま登録的时候通过结果值的数字作为索引取staging.csv对应索引列的数据，
        //并将该数据登录到TargetTable的TargetColumn
        //当是変換して登録的时候，直接把结果值的字符串输入到TargetTable的TargetColumn
        //追加一句TargetTable为PRODUCT_MST的时候登录到Product模型的主属性，
        //为PRODUCT_EAV登录到Product模型的扩展属性也就是List<ProductAttribute> Attributes

        // 规则执行：根据出力タイプ和规则内容决定如何赋值，并写入Product模型，返回product
        public static void ApplyRuleToProduct(RuleGroup rule, Dictionary<string, string> rowData, Product product)
        {
            string value = string.Empty;
            if (rule.OutType == "そのまま登録")
            {
                var key = rule.ResultValue;
                rowData.TryGetValue(key, out value);
                Logger.Info($"[RULE APPLY] RuleId={rule.RuleId}，{rule.TargetTable}的{rule.TargetColumn}被赋值为：{value}");
            }
            else if (rule.OutType == "変換して登録")
            {
                value = rule.ResultValue ?? string.Empty;
            }
            // 写入Product模型
            if (rule.TargetTable == "PRODUCT_MST")
            {
                switch (rule.TargetColumn)
                {
                    case "PRODUCT_CODE": product.ProductCode = value ?? string.Empty; break;
                    case "BRAND_ID": product.BrandId = value ?? string.Empty; break;
                    case "PRODUCT_NAME": product.ProductName = value ?? string.Empty; break;
                    case "CATEGORY": product.Category = value ?? string.Empty; break;
                    case "STATUS": product.State = value ?? string.Empty; break;
                    default: break;
                }
            }
            else if (rule.TargetTable == "PRODUCT_EAV")
            {
                if (!string.IsNullOrEmpty(rule.ItemId) && !product.Attributes.Exists(a => a.AttributeId == rule.ItemId))
                {
                    product.Attributes.Add(new ProductAttribute { AttributeId = rule.ItemId, Value = value ?? string.Empty });
                }
            }
            Logger.Info($"[RULE APPLY] RuleId={rule.RuleId}，{rule.TargetTable}的{rule.TargetColumn}被赋值为：{value}");
            // return product;
        }
    }
}
