using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CalendarProject.EntityFramework
{
    internal class DbWorker
    {
        private AppDbContext DbContext;

        public DbWorker(string path)
        {
            this.DbContext = new AppDbContext(path);

            DbInit();
        }

        private void DbInit()
        {
            DbContext.Database.Migrate();
            DbContext.SaveChanges();
        }

        /// <summary>
        /// Function for add any class in DataBase
        /// </summary>
        public void DbAdd<T>(params T[] items) where T : class
        {
            DbContext.AddRange(items);
            DbContext.SaveChanges();
        }

        ///<summary>
        /// Function for execute any SQL query
        ///</summary>
        public List<T> DbExecuteSQL<T>(string query) where T : class
        {
            return DbContext.Set<T>().FromSqlRaw(query).ToList();
        }
    }
}