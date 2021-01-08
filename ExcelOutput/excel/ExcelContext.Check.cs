using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ExcelTool
{
    partial class ExcelContext
    {
        /// <summary>
        /// 检查所有的表格配置。
        /// </summary>
        private void CheckAllTable()
        {
            List<Task> tasks = new List<Task>();
            foreach (var table in _excelDataTables.Values)
            {
                tasks.Add(
                    Task.Factory.StartNew(
                        () =>
                        {
                            ExcelDataTableCheck check = new ExcelDataTableCheck(this, table);
                            check.CheckDataTable();
                        }, CancellationToken.None, TaskCreationOptions.PreferFairness, _scheduler
                    )
                );
            }

            Task.WaitAll(tasks.ToArray());
        }
    }
}