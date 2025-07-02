﻿using ImporterApp;
using ImporterApp.Services;
using ImporterApp.Rules;
// メイン処理、起動用
class Program
{
    static void Main()
    {
        var executor = new ImporterExecutor();
        executor.Execute("RKE伝票.csv", "RKE");
    }
}

