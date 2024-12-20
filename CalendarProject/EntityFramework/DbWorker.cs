﻿using System;
using System.Collections.Generic;
using System.Dynamic;
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
        /// Функция для добавления некоторого класса в БД
        /// </summary>
        public void DbAdd<T>(params T[] items) where T : class
        {
            DbContext.AddRange(items);
            DbContext.SaveChanges();

            DbAdded?.Invoke(this, items);
        }

        /// <summary>
        /// Функция для удаления элемента из БД
        /// </summary>
        public void DbDelete<T>(int id) where T : class
        {
            var item = DbContext.Set<T>().Find(id);
            if (item != null)
            {
                DbContext.Set<T>().Remove(item);
                DbContext.SaveChanges();

                DbDeleted?.Invoke(this, item);
            }
        }

        /// <summary>
        /// Функция для обновления записи в БД
        /// </summary>
        public void DbUpdate<T>(T item) where T : class
        {
            DbContext.Set<T>().Update(item);
            DbContext.SaveChanges();

            DbUpdated?.Invoke(this, item);
        }

        /// <summary>
        /// Функция для возврата таблицы из БД
        /// </summary>
        public List<T> DbGetTable<T>() where T : class
        {
            return DbContext.Set<T>().ToList();
        }

        ///<summary>
        /// Функция для выполнения SQl запроса к БД
        ///</summary>
        public List<T> DbExecuteSQL<T>(string query, params object[] parameters) where T : class
        {
            return DbContext.Set<T>().FromSqlRaw(query, parameters).ToList();
        }

        public event EventHandler<object> DbAdded;
        public event EventHandler<object> DbDeleted;
        public event EventHandler<object> DbUpdated;
    }
}