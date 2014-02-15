using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
 using System.Text;
 using System.Xml;
using System.Xml.Serialization;
using System.ServiceModel.Web;
using System.Net;

namespace PreScripds.Infrastructure.Services
{
    public class Util
    {
        /// <summary>
        /// Serializes a class to xml text
        /// </summary>
        /// <param name="item">Class to be converted to XML</param>
        /// <returns>XML string representing class</returns>
        public static string SerializeObjectToXML(object item)
        {
            try
            {
                string xmlText;
                //Get the type of the object
                Type objectType = item.GetType();
                //create serializer object based on the object type
                XmlSerializer xmlSerializer = new XmlSerializer(objectType);
                //Create a memory stream handle the data
                MemoryStream memoryStream = new MemoryStream();
                //Create an XML Text writer to serialize data to
                using (XmlTextWriter xmlTextWriter =
                    new XmlTextWriter(memoryStream, Encoding.UTF8) { Formatting = Formatting.Indented })
                {
                    //convert the object to xml data
                    xmlSerializer.Serialize(xmlTextWriter, item);
                    //Get reference to memory stream
                    memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
                    //Convert memory byte array into xml text
                    xmlText = new UTF8Encoding().GetString(memoryStream.ToArray());
                    //clean up memory stream
                    memoryStream.Dispose();
                    return xmlText;
                }
            }
            catch (Exception e)
            {
                //There are a number of reasons why this function may fail
                //usually because some of the data on the class cannot
                //be serialized.
                System.Diagnostics.Debug.Write(e.ToString());
                return null;
            }
        }

        public static void CheckETag(IncomingWebRequestContext incomingRequest, OutgoingWebResponseContext outgoingResponse, Guid ServerETag) 
        { 
            string test = incomingRequest.Headers[HttpRequestHeader.IfNoneMatch]; 
            if (test == null)   
                return; 
            Guid? eTag = new Guid(test); 
            if (eTag != ServerETag) return; 
            outgoingResponse.SuppressEntityBody = true; 
            outgoingResponse.StatusCode = HttpStatusCode.NotModified; 
        }  
        public static void CheckModifiedSince(IncomingWebRequestContext incomingRequest, OutgoingWebResponseContext outgoingResponse, DateTime serverModifiedDate) 
        { 
            DateTime modifiedSince = Convert.ToDateTime(incomingRequest.Headers[HttpRequestHeader.LastModified]); 
            if (modifiedSince != serverModifiedDate) 
                return; 
            outgoingResponse.SuppressEntityBody = true; 
            outgoingResponse.StatusCode = HttpStatusCode.NotModified; 
        }
        public static void SetCaching(WebOperationContext context)
        {

            // set CacheControl header
            DateTime dt = DateTime.UtcNow.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            HttpResponseHeader cacheHeader = HttpResponseHeader.CacheControl;
            String cacheControlValue = String.Format("max-age={0}", 100);
            //context.OutgoingResponse.Headers.Add(cacheHeader, cacheControlValue);
            context.OutgoingResponse.Headers.Add("Expires", dt.ToUniversalTime().ToString("r"));
            // set cache validation 
            context.OutgoingResponse.LastModified = DateTime.Now.ToUniversalTime();//.ToString("r");
            String eTag = context.IncomingRequest.UriTemplateMatch.RequestUri.ToString();
            context.OutgoingResponse.ETag = eTag;
        }
    
    }
}