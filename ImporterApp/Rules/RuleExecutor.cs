using System.Collections.Generic;
using ImporterApp.Models;
using ImporterApp.Services.Shared;
using ImporterApp.Infrastructure;
using ImporterApp.Services.Mapping;

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
                    // Logger.Info($"[RULE SKIP] RuleId={rule.RuleId} はuserSceanarioId={userScenarioId}にマッチしないためスキップされました");
                    continue;
                }
                // ルールの適用
                // Logger.Info($"[RULE EXEC] RuleId={rule.RuleId} が実行され、{rule.TargetTable} の {rule.TargetColumn} が変更されました");
                // Logger.Info($"CONDITIONS: {rule.Conditions.Count} 条件");
                // EvaluateConditions(rule.Conditions, rowData);
                // Logger.Info($"[RULE EXEC] RuleId={rule.RuleId} 条件評価完了");
                if (RuleEngine.EvaluateConditions(rule.Conditions, rowData, userScenarioId))
                {
                    ProductPropertyMapping.ApplyRuleToProduct(rule, rowData, product);
                }
                else
                {
                    //Logger.Info($"[RULE NOT APPLIED] RuleId={rule.RuleId} 条件不成立、ルール適用されていません。");
                }

                // Logger.Info($"[RULE EXEC] RuleId={rule.RuleId} がProductモデル{product.ProductCode}に適用されました");
            }
            return product; // 新しいProductオブジェクトを返す。実際の運用では変更後の製品を返す場合もある
        }
    }
}
