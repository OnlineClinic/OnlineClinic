using MyOnlineClinic.Migrator;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using PayPal.Api;
using MyOnlineClinic.Entity;
using MyOnlineClinic.Web.Models;
namespace MyOnlineClinic.Web.Helper
{
    public static class Common
    {
        public static string fromEmailAddress = "noreply@myonlineclinic.com.au";

        public static string FormatJsonString(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return string.Empty;
            }

            if (json.StartsWith("["))
            {
                // Hack to get around issue with the older Newtonsoft library
                // not handling a JSON array that contains no outer element.
                json = "{\"list\":" + json + "}";
                var formattedText = JObject.Parse(json).ToString(Formatting.Indented);
                formattedText = formattedText.Substring(13, formattedText.Length - 14).Replace("\n  ", "\n");
                return formattedText;
            }
            return JObject.Parse(json).ToString(Formatting.Indented);
        }

        /// <summary>
        /// Gets a random invoice number to be used with a sample request that requires an invoice number.
        /// </summary>
        /// <returns>A random invoice number in the range of 0 to 999999</returns>
        public static string GetRandomInvoiceNumber()
        {
            return new Random().Next(999999).ToString();
        }

        //For Store Procedure
        public static DataSet ExecuteStoredProcedure(EntitiesDB context, string storedProcedureName, IEnumerable<SqlParameter> parameters)
        {

            var ds = new DataSet();

            using (var conn = new SqlConnection(context.Database.Connection.ConnectionString))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = storedProcedureName;
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            cmd.Parameters.Add(parameter);
                        }
                    }

                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(ds);
                    }
                }
            }

            return ds;
        }

        /// <summary>
        /// Converts a DataTable to a list with generic objects
        /// </summary>
        /// <typeparam name="T">Generic object</typeparam>
        /// <param name="table">DataTable</param>
        /// <returns>List with generic objects</returns>
        public static List<T> DataTableToList<T>(this DataTable table) where T : class, new()
        {
            try
            {
                List<T> list = new List<T>();

                foreach (var row in table.AsEnumerable())
                {
                    T obj = new T();

                    foreach (var prop in obj.GetType().GetProperties())
                    {
                        try
                        {
                            PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                            propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    list.Add(obj);
                }

                return list;
            }
            catch
            {
                return null;
            }
        }

        public static MyOnlineClinic.Entity.PaymentHistory MapPayPalResponseWithHistory(PayPal.Api.Payment payment,CreditCardViewModel cardmodel)
        {
            MyOnlineClinic.Entity.PaymentHistory rModel = new MyOnlineClinic.Entity.PaymentHistory();
            rModel.AppointmentId = cardmodel.AppointmentId;
            rModel.AppointmentType = cardmodel.AppointmentType;
            rModel.PatientLoginId = cardmodel.PatientLoginId;
            rModel.DoctorLoginId = cardmodel.DoctorLoginId;
            rModel.AuthorizationId = payment.transactions.FirstOrDefault().related_resources.FirstOrDefault().authorization.id;
            rModel.PaymentId = payment.id;
            rModel.PaymentStatus = payment.state;
            rModel.SetCreated(cardmodel.LoginId);
            rModel.ReponseString = payment.ConvertToJson();
            return rModel; 
        }
        public static string DateToString(DateTime date, string type)
        {

            string retundatestring = string.Empty;
            if (type == "date")
            {
                retundatestring = date.ToString("dd/MM/yyyy");
            }
            else if (type == "MonthYear")
            {
                retundatestring = date.ToString("MM/yyyy");
            }
            else if (type == "dateTime")
            {
                retundatestring = date.ToString("dd/MM/yyyy hh:mm tt");
            }
            else if (type == "Time")
            {
                retundatestring = date.ToString("hh:mm tt");
            }
            return retundatestring;

        }
        
    }
}