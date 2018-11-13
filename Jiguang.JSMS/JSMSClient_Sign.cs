using Jiguang.JSMS.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Jiguang.JSMS
{
    public partial class JSMSClient
    {
        /// <summary>
        /// 创建签名
        /// </summary>
        /// <param name="signModel"></param>
        /// <returns></returns>
        public async Task<HttpResponse> AddSignAsync(SignModel signModel)
        {
            if (signModel==null||string.IsNullOrEmpty(signModel.Sign))
                throw new ArgumentNullException(nameof(signModel));

            MultipartFormDataContent content = new MultipartFormDataContent
            {
                new StringContent(signModel.Sign),
            };
            if (signModel.Image0 != null)
            {
                content.Add(new StreamContent(signModel.Image0));
            }
            if (signModel.Image1 != null)
            {
                content.Add(new StreamContent(signModel.Image1));
            }
            if (signModel.Image2 != null)
            {
                content.Add(new StreamContent(signModel.Image2));
            }
            if (signModel.Image3 != null)
            {
                content.Add(new StreamContent(signModel.Image3));
            }

            using (var resp = await httpClient.PostAsync("sign", content))
            {
                string respStr = await resp.Content.ReadAsStringAsync();
                return new HttpResponse(resp.StatusCode, resp.Headers, respStr);
            }
            
        }

        /// <summary>
        /// 新增签名
        /// </summary>
        /// <param name="signModel"></param>
        /// <returns></returns>
        public HttpResponse AddSign(SignModel signModel)
        {
            Task<HttpResponse> task = Task.Run(() => AddSignAsync(signModel));
            task.Wait();
            return task.Result;
        }

        /// <summary>
        /// 修改签名
        /// </summary>
        /// <param name="signId"></param>
        /// <param name="signModel"></param>
        /// <returns></returns>
        public async Task<HttpResponse> UpdSignAsync(int signId,SignModel signModel)
        {
            if (signModel == null || string.IsNullOrEmpty(signModel.Sign))
                throw new ArgumentNullException(nameof(signModel));

            MultipartFormDataContent content = new MultipartFormDataContent
            {
                new StringContent(signModel.Sign),
            };
            if (signModel.Image0 != null)
            {
                content.Add(new StreamContent(signModel.Image0));
            }
            if (signModel.Image1 != null)
            {
                content.Add(new StreamContent(signModel.Image1));
            }
            if (signModel.Image2 != null)
            {
                content.Add(new StreamContent(signModel.Image2));
            }
            if (signModel.Image3 != null)
            {
                content.Add(new StreamContent(signModel.Image3));
            }

            using (var resp = await httpClient.PostAsync($"sign/{signId}", content))
            {
                string respStr = await resp.Content.ReadAsStringAsync();
                return new HttpResponse(resp.StatusCode, resp.Headers, respStr);
            }
        }
        /// <summary>
        /// 修改签名
        /// </summary>
        /// <param name="signId"></param>
        /// <param name="signModel"></param>
        /// <returns></returns>
        public HttpResponse UpdSign(int signId,SignModel signModel)
        {
            Task<HttpResponse> task = Task.Run(() => UpdSignAsync(signId,signModel));
            task.Wait();
            return task.Result;
        }

        /// <summary>
        /// 查询签名 
        /// </summary>
        /// <param name="signId"></param>
        /// <returns></returns>
        public async Task<HttpResponse> QuerySignAsync(int signId)
        {
            using (var resp = await httpClient.GetAsync($"sign/{signId}"))
            {
                string respStr = await resp.Content.ReadAsStringAsync();
                return new HttpResponse(resp.StatusCode, resp.Headers, respStr);
            }
        }
        /// <summary>
        /// 查询签名 
        /// </summary>
        /// <param name="signId"></param>
        /// <returns></returns>
        public HttpResponse QuerySign(int signId)
        {
            Task<HttpResponse> task = Task.Run(() => QuerySignAsync(signId));
            task.Wait();
            return task.Result;
        }

        /// <summary>
        /// 删除签名
        /// </summary>
        /// <param name="signId"></param>
        /// <returns></returns>
        public async Task<HttpResponse> DelSignAsync(int signId)
        {
            using (var resp = await httpClient.DeleteAsync($"sign/{signId}"))
            {
                string respStr = await resp.Content.ReadAsStringAsync();
                return new HttpResponse(resp.StatusCode, resp.Headers, respStr);
            }
        }

        /// <summary>
        /// 删除签名
        /// </summary>
        /// <param name="signId"></param>
        /// <returns></returns>
        public HttpResponse Delign(int signId)
        {
            Task<HttpResponse> task = Task.Run(() => DelSignAsync(signId));
            task.Wait();
            return task.Result;
        }

    }
}
