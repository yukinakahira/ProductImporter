﻿using ImporterApp;
using ImporterApp.Services;
using ImporterApp.Rules;
using ImporterApp.Infrastructure; // Add this for BrandMapping
using ImporterApp.Models;        // Add this for ApprovalPending and TempProduct
// Ensure the following using is present if BrandMapping is in a different namespace
// using ImporterApp.BrandMappings; 
// Add this if BrandMapping is in ImporterApp.BrandMappings namespace


// メイン処理、起動用
class Program
{
    static void Main()
    {
        var executor = new ImporterExecutor();
        executor.Execute("staging.csv", "KM_PRODCT", "グループ会社ID");

      
    }
}


