using AnonChat.DAL.Interfaces;
using AnonChat.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AnonChat.BLL.Filters
{
    public class ExceptionFilter : Attribute, IExceptionFilter
    {
        IUnitOfWork Database { get; set; }

        public ExceptionFilter(IUnitOfWork uow)
        {
            Database = uow;
        }

        public void OnException(ExceptionContext filterContext)
        {
            LogToFile(filterContext);
            LogToDB(filterContext);
            filterContext.ExceptionHandled = true;
        }

        public void LogToFile(ExceptionContext filterContext)
        {
            StringBuilder builder = new StringBuilder();
            builder
                .AppendLine("----------")
                .AppendLine(DateTime.Now.ToString())
                .AppendFormat("Request Url:\t{0}", filterContext.HttpContext.Request.Path)
                .AppendLine()
                .AppendFormat("User Name:\t{0}", filterContext.HttpContext.User.Identity.Name)
                .AppendLine()
                .AppendFormat("Headers:\t{0}", filterContext.HttpContext.Request.Headers)
                .AppendLine()
                .AppendFormat("Query String:\t{0}", filterContext.HttpContext.Request.QueryString)
                .AppendLine()
                .AppendFormat("HttpVerb:\t{0}", filterContext.HttpContext.Request.Method)
                .AppendLine()
                .AppendFormat("Message:\t{0}", filterContext.Exception.Message)
                .AppendLine()
                .AppendFormat("Stack:\t{0}", filterContext.Exception.StackTrace)
                .AppendLine();

            string filePath = "Error.log";

            using (StreamWriter writer = System.IO.File.AppendText(filePath))
            {
                writer.Write(builder.ToString());
                writer.Flush();
            }
        }

        public void LogToDB(ExceptionContext exceptionContext)
        {
            LogDetail logDetail = new LogDetail()
            {
                RequestTime = DateTime.Now.ToString(),
                RequestUrl = exceptionContext.HttpContext.Request.Path,
                UserName = exceptionContext.HttpContext.User.Identity.Name,
                Headers = exceptionContext.HttpContext.Request.Headers.ToString(),
                QueryString = exceptionContext.HttpContext.Request.QueryString.ToString(),
                HttpVerb = exceptionContext.HttpContext.Request.Method,
                Message = exceptionContext.Exception.Message,
                Stack = exceptionContext.Exception.StackTrace
            };

            Database.LogDetails.Create(logDetail);
            Database.Save();
        }
    }
}
