using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CopySDK.Models;

namespace CopySDK.Samples.MVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            //string verifier = Request.QueryString["oauth_verifier"];

            return View();
        }

        [HttpGet]
        public async Task<ActionResult> About()
        {
            ViewBag.Message = "Your app description page.";

            string verifier = Request.QueryString["oauth_verifier"];

            if (!string.IsNullOrEmpty(verifier))
            {
                CopyAuth copyConfig = (CopyAuth) Session["copyConfig"];

                CopyClient copyClient = await copyConfig.GetAccessTokenAsync(verifier);

                Session.Add("copyClient", copyClient);

                FileSystem rootFolder = await copyClient.GetRootFolder();
            }
            else if (Session["copyClient"] != null)
            {
                //CopyClient copyClient = (CopyClient) Session["copyClient"];

                //FileSystem rootFolder = await copyClient.GetRootFolder();
            }

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public async Task<ActionResult> Authenticate()
        {
            Scope scope = new Scope()
            {
                Profile = new ProfilePermission()
                {
                    Read = true,
                    Write = true
                },
                FileSystem = new FileSystemPermission()
                {
                    Read = true,
                    Write = true
                }
            };

            CopyAuth copyConfig = new CopyAuth("http://localhost/CopyMVC/Home/About", "cIAKv1kFCwXn2izGsMl8vZmfpfBcJSv1", "vNY1oLTr2WieLYxgCA6tDgdfCS1zTRA2IMzhmQLoQOS7nmIK", scope);

            await copyConfig.GetRequestTokenAsync();

            Session.Add("copyConfig", copyConfig);

            return Redirect(copyConfig.AuthCodeUri.AbsoluteUri);
        }

        public async Task<ActionResult> Upload(HttpPostedFileBase fileUpload)
        {
            byte[] fileBytes;

            using (Stream inputStream = fileUpload.InputStream)
            {
                MemoryStream memoryStream = new MemoryStream();

                inputStream.CopyTo(memoryStream);

                fileBytes = memoryStream.ToArray();
            }

            CopyClient copyClient = (CopyClient)Session["copyClient"];

            bool b = await copyClient.FileSystemManager.UploadNewFileAsync("/copy", fileUpload.FileName, fileBytes, false);

            return View("About");
        }
    }
}
