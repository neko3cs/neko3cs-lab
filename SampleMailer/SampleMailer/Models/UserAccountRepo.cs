using SQLite.Net;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SampleMailer.Models
{
    public class UserAccountRepo
    {
        private static readonly object Locker = new object();
        private readonly SQLiteConnection DB;

        public UserAccountRepo()
        {
            DB = DependencyService.Get<ILocalDB>().GetConnection();
            DB.CreateTable<UserAccount>();
        }

        /// <summary>
        /// ユーザアカウントのリストを取得します。
        /// </summary>
        public IEnumerable<UserAccount> GetUserAccounts()
        {
            lock (Locker)
            {
                return DB.Table<UserAccount>();
            }
        }

        /// <summary>
        /// ユーザアカウントを保存します。
        /// </summary>
        public int SaveUserAccount(UserAccount account)
        {
            lock (Locker)
            {
                bool existAccount = DB.Table<UserAccount>()
                    .Count(item => item.Name.Equals(account.Name) && item.Password.Equals(account.Password)) != 0;
                if (existAccount)
                {
                    return 0;
                }

                if (account.ID != 0)
                {
                    return DB.Update(account);
                }
                else
                {
                    return DB.Insert(account);
                }
            }
        }
    }
}
