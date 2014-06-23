using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ImageResizer;
using PreScripds.Domain;
using PreScripds.Infrastructure;
using PreScripds.UI.Common;
using PreScripds.UI.Models;

namespace PreScripds.UI.Controllers
{
    public class BaseController : Controller
    {
        public ViewResult AppSettings()
        {
            var appSettings = new AppSettingModel()
            {
                AppPath = GetAppPath()
            };
            return View(appSettings);
        }

        public string GetAppPath()
        {
            return Request.ApplicationPath == "/" ? Request.ApplicationPath : Request.ApplicationPath + "/";
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var cntxt = System.Web.HttpContext.Current;
            base.OnActionExecuting(filterContext);
        }

        public ActionResult CheckSessionContext(string returnUrl = "")
        {
            if (SessionContext.CurrentUser == null)
                return RedirectToAction("Login", "Account");
            else if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        public LibraryAsset GetLibraryAsset(HttpPostedFileBase file, string userfileName = null, string userDesc = null, long? folderId = null)
        {
            var appFileSize = Constants.GetConfigValue<int>(Constants.ApplicationConstants.FILE_SIZE) * (1024 * 1024);
            var isFile = file != null;

            if (isFile)
            {
                if ((isFile && file.ContentLength > 0 && file.ContentLength <= appFileSize && file.FileName.IsNotEmpty()))
                {
                    var libraryAsset = new LibraryAsset();
                    string fileName = "";
                    int contentLength = 0;
                    string contentType = "";
                    byte[] array = null;
                    if (isFile)
                    {
                        if (userfileName == null)
                        {
                            fileName = Path.GetFileName(file.FileName);
                        }
                        else
                        {
                            var trimdUserFileName = userfileName.Replace(" ", string.Empty);
                            fileName = Path.GetFileName(trimdUserFileName);
                        }

                        contentLength = file.ContentLength;
                        contentType = file.ContentType;
                        array = file.ToByteArray();
                    }

                    libraryAsset.AssetThumbnail = ImageExtensions.ResizeImage(ImageExtensions.ByteArrayToImage(array), new Size(40, 40));
                    libraryAsset.AssetName = fileName;
                    libraryAsset.AssetSize = contentLength;
                    libraryAsset.AssetType = contentType;
                    libraryAsset.CreatedDate = DateTime.Now;
                    libraryAsset.AssetDescription = userDesc;
                    if (folderId.HasValue)
                        libraryAsset.LibraryFolderId = folderId.Value;
                    libraryAsset.AssetPath = ConfigurationManager.AppSettings["AppAssetPath"];
                    libraryAsset.Active = true;
                    libraryAsset.LibraryAssetFiles = new List<LibraryAssetFile>()
                            {
                                new LibraryAssetFile()
                                {
                                    Asset = array,
                                    CreatedDate = DateTime.Now
                                }
                            };

                    return libraryAsset;
                }
            }
            return null;
        }

        //public void EmbedImage(string imagePath)
        //{
        //    var request = System.Web.HttpContext.Current.Request;
        //    var urlHelper = new UrlHelper(request.RequestContext);
        //    var queryWidth = request.QueryString["width"].AsInt();
        //    var queryHeight = request.QueryString["height"].AsInt();
        //    //var resizeSetting = new ResizeSettings();
        //    //resizeSetting.Width = queryWidth;
        //    //resizeSetting.Height = queryHeight;
        //    ImageBuilder.Current.Build(imagePath, "~/ResizedImages",
        //                   new ResizeSettings("width=100&height=200&crop=auto"));
        //    //return "";
        //}

    }
}