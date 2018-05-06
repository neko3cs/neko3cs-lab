using System;
using System.Collections.Generic;
using System.Text;
using SQLite.Net;

namespace SampleMailer
{
    public interface ILocalDB
    {
        SQLiteConnection GetConnection();
    }
}
