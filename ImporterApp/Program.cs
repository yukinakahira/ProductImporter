﻿using ImporterApp;
using ImporterApp.Services;
using ImporterApp.Rules;
// メイン処理、起動用
class Program
{
    static void Main()
    {
        var executor = new ImporterExecutor();
<<<<<<< HEAD
        executor.Execute("staging.csv", "ユースジID3");//Usage→商品データ取り込み
=======
        executor.Execute("staging.csv", "RKE_PRODCT");
>>>>>>> trunk_20250623
    }
}

