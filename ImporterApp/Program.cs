﻿using ImporterApp;
// メイン処理、起動用
class Program
{
    static void Main()
    {
        var executor = new ImporterExecutor();
        executor.Execute("staging.csv", "ユースジID1");
    }
}

