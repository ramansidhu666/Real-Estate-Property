using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Configuration;
using Property.Models;
using System.Configuration;
using System.Net;
using System.Text;
using System.Data;
using Property.Service;

namespace Property.Infrastructure
{
    enum RoleAction
    {
        view,
        create,
        edit,
        delete,
        detail,
        download
    }

    public class CommonClass
    {
        public static string SendGCM_Notifications(string regId, string value, bool IsJson = false)
        {
            try
            {
                if (true)
                {
                    //regId = regId.TrimEnd(',').ToString();

                    //Bharat Testing
                    //regId = "APA91bGGmOXoREb_7Z8nHS2iFhdjIVvuCa888Xcebheh1opCLFFuWjiUkJl9tmKJ0mNRySd0Xri639Jc2kfRoGOF3FSaD_jiOB_dXHR-4nSnaLXVb6mW2foW0hZbUveYCjud2E3Y3vH61KyVLBco42SpMmR1dv6RsA";

                    var applicationID = ConfigurationManager.AppSettings["GCM_ApplicationID"].ToString();

                    var SENDER_ID = ConfigurationManager.AppSettings["GCM_SenderID"].ToString();
                    // var value = dtGCMRegistrationID.Rows[0].ToString();
                    //WebRequest tRequest;
                    var tRequest = (HttpWebRequest)WebRequest.Create("https://android.googleapis.com/gcm/send");
                    tRequest.Method = "post";
                    tRequest.ContentType = "application/json"; //" application/x-www-form-urlencoded;charset=UTF-8";
                    tRequest.Headers.Add(HttpRequestHeader.Authorization, "key=" + applicationID + "");
                    tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));

                    //Data_Post Format
                    string postData = "";
                    if (IsJson)
                    {
                        postData = "{ \"collapse_key\": \"PropertyApp\",  \"time_to_live\": 0,  \"delay_while_idle\": false,  \"data\": {    \"message\": " + value + ",    \"time\": \"" + System.DateTime.Now.ToString() + "\" },  \"registration_ids\":[\"" + regId.Replace("'", "\"") + "\"]}";
                    }
                    else
                    {
                        postData = "{ \"collapse_key\": \"PropertyApp\",  \"time_to_live\": 0,  \"delay_while_idle\": false,  \"data\": {    \"message\": \"" + value + "\",    \"time\": \"" + System.DateTime.Now.ToString() + "\" },  \"registration_ids\":[\"" + regId.Replace("'", "\"") + "\"]}";
                    }

                    Console.WriteLine(postData);
                    Byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                    tRequest.ContentLength = byteArray.Length;

                    Stream dataStream = tRequest.GetRequestStream();

                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();

                    WebResponse tResponse = tRequest.GetResponse();

                    dataStream = tResponse.GetResponseStream();

                    StreamReader tReader = new StreamReader(dataStream);

                    String sResponseFromServer = tReader.ReadToEnd();

                    tReader.Close();
                    dataStream.Close();
                    tResponse.Close();
                    return sResponseFromServer;
                }
            }
            catch (WebException e)
            {
                using (WebResponse response = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        string text = reader.ReadToEnd();
                        return text;
                    }
                }
            }

        }
        public static JsonReturnModel CreateMessage(string Status, object Message)
        {
            JsonReturnModel jsonReturn = new JsonReturnModel();
            jsonReturn.Status = Status;
            jsonReturn.Message = Message;
            return jsonReturn;
        }
        public static string GetAlphaNumericCode(int length = 6)
        {
            Random random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var result = new string(
                Enumerable.Repeat(chars, length)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());

            return result;
        }
        public static string GetNumericCode(int length = 4)
        {
            Random random = new Random();
            var chars = "0123456789";
            var result = new string(
                Enumerable.Repeat(chars, length)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());

            return result;
        }
        public static Image Base64ToImage(string base64String)
        {
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0,
              imageBytes.Length);

            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);
            return image;
        }
        public static string GetURL()
        {
            Boolean IsLive = Convert.ToBoolean(ConfigurationManager.AppSettings["IsLive"].ToString());
            string URL = "";
            if (IsLive)
            {
                URL = ConfigurationManager.AppSettings["LiveURL"].ToString();
            }
            else
            {
                URL = ConfigurationManager.AppSettings["LocalURL"].ToString();
            }
            return URL;
        }
        public static string GetCustomerImage(string imageName)
        {
            return GetURL() + "/CustomerImages/" + (imageName == "" || imageName == null ? "no-user-image.png" : imageName);
        }
        public static string ConvertDateToStr(DateTime dt)
        {
            string newDt = "";
            try
            {
                newDt = dt.ToString(ConstantModel.ProjectSettings.DateFormat);
                return (newDt == "01/01/1900" ? "" : newDt);
            }
            catch (Exception ex)
            {
                return newDt;
            }
        }
        public static bool CreateThumbnail(string ImageURL, int Width, int Height, bool maintainAspectRatio)
        {
            bool Success = false;
            //bool Success = true;
            string FullUrl = System.Web.HttpContext.Current.Server.MapPath(ImageURL);

            byte[] imageData = null;
            FileInfo fileInfo = new FileInfo(FullUrl);
            long imageFileLength = fileInfo.Length;
            FileStream fs = new FileStream(FullUrl, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            imageData = br.ReadBytes((int)imageFileLength);

            br.Dispose();
            fs.Close();

            // System.IO.File.Delete(FullUrl);

            Bitmap bmp = null;
            try
            {
                MemoryStream memStream = new MemoryStream(imageData);

                System.Drawing.Image img = System.Drawing.Image.FromStream(memStream);
                if (maintainAspectRatio)
                {
                    //AspectRatio aspectRatio = new AspectRatio();
                    //aspectRatio.WidthAndHeight(img.Width, img.Height, Width, Height);
                    //bmp = new Bitmap(img, aspectRatio.Width, aspectRatio.Height);
                    //bmp = new Bitmap(aspectRatio.Width, aspectRatio.Height);

                }
                else
                {
                    bmp = new Bitmap(img, Width, Height);

                }
                memStream.Dispose();
                // img.Dispose();
                bmp.Save(FullUrl);


                Success = true;
            }
            catch (Exception ex)
            {
                string ErrorMsg = ex.Message.ToString();
                //ErrorLogging.LogError(ex);
                Success = false;
            }
            return Success;
        }

        public static string ErrorLog(string ErrorMessage)
        {
            if (ErrorMessage.Contains("The DELETE statement conflicted with the REFERENCE constraint"))
            {
                return "fk";
            }
            else
            {
                return "";
            }
        }

       
    }
}

