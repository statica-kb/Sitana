﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Sitana.Framework;
using System.Web;


namespace Sitana.Framework.Diagnostics
{
    public class MintCrashService: CrashService
    {
        string _apiToken;
        string _appName;
        string _uid;
		string _phone;

        public MintCrashService(string apiToken, string appName)
        {
            _apiToken = apiToken;
            _appName = appName;
			_uid = Platform.DeviceId;
			_phone = Platform.OsName;
        }

        public override async Task<ExceptionData> SendOne(ExceptionData exceptionData)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://mint.splunk.com/api/errors");
            request.Headers.Add("X-Splunk-Mint-Auth-Token", _apiToken);
            request.Headers.Add("X-BugSense-Api-Key", _apiToken);
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";

            string data = GenerateJson(exceptionData);
			string requestString = "data=" + HttpUtility.UrlEncode(data);
			byte[] requestData = Encoding.UTF8.GetBytes(requestString);

            try
            {
                var requestStream = await request.GetRequestStreamAsync();
                await requestStream.WriteAsync(requestData, 0, requestData.Length);
                requestStream.Close();

                var response = await request.GetResponseAsync();

                StreamReader reader = new StreamReader(response.GetResponseStream());
                string responseString = await reader.ReadToEndAsync();

                response.Close();
            }
            catch
            {
                return exceptionData;
            }

            return null;
        }

        string GenerateJson(ExceptionData data)
        {
            string client = CreateJsonValue("client", CreateJsonValue("name", _appName), CreateJsonValue("version", data.AppVersion));
            string exception = CreateJsonValue("exception", CreateJsonValue("message", data.Message), CreateJsonValue("where", data.Source), CreateJsonValue("klass", data.Type), CreateJsonValue("backtrace", data.StackTrace) );
			string appEnvironment = CreateJsonValue("application_environment", CreateJsonValue("phone", _phone), CreateJsonValue("appver", data.AppVersion), CreateJsonValue("appname", _appName), CreateJsonValue("osver", data.OsVersion), CreateJsonValue("uid", _uid));

            return "{\n" + client + ",\n" + exception + ",\n" + appEnvironment + "\n}";
        }

        string CreateJsonValue(string name, params string[] values)
        {
            if(values.Length > 1)
            {
                string content = values[0];

                for(int idx = 1; idx < values.Length; ++idx)
                {
                    content += string.Format(",\n{0}", values[idx]);
                }

                return string.Format("\"{0}\": {{\n{1}\n}}", name, content);
            }
            else if (values.Length == 1)
            {
                return string.Format("\"{0}\": \"{1}\"", name, values[0]);
            }

            return string.Format("\"{0}\": \"\"", name);
        }
    }
}
